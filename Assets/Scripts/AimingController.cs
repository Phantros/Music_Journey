using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class AimingController : MonoBehaviour
{
    public GameObject crosshair; // Reference to your crosshair UI element.
    public MelodyManager melodyManager;
    public AudioManager audioManager;
    public List<Collider> flowerPotsColliders;
    public TextMeshProUGUI textMeshPro;
    public Canvas signCanvas;
    public AudioClip liftPotClip;
    public AudioClip swapPotsClip;
    public Animator birdConvoAnim;


    private AudioSource audioSource;
    private Transform currentAimedObject; // The currently aimed-at object (if any).
    private Transform firstAimedObject; // The first object the player aims at.
    private Transform secondAimedObject; // The second object the player aims at.
    private float raiseAmount = 0.3f;
    private List<Transform> raisedObjects = new List<Transform>();
    private Transform lastAimedObject;
    private Collider lastAimedCollider;
    private int birdClick = 0;

    // Enum to represent the state of an object.
    private enum ObjectState
    {
        Lowered,
        Raising,
        Raised,
        Swapping,
        Lowering
    }

    private Dictionary<Transform, ObjectState> objectStates = new Dictionary<Transform, ObjectState>();

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        else
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    private void Update()
    {
        // Create a ray from the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // Declare a RaycastHit variable to store information about the hit
        RaycastHit hit;

        // Check if the ray hits an object on the interactable layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Interactable", "Highlight")))
        {
            // Store the currently aimed-at object
            currentAimedObject = hit.transform;
            lastAimedCollider = hit.collider;

            // Make the crosshair change color or appearance to indicate targeting
            crosshair.transform.localScale = Vector3.one * 1.2f;

            if (currentAimedObject != lastAimedObject)
            {
                lastAimedObject = currentAimedObject;
            }

            if (currentAimedObject.CompareTag("HighLightAble"))
            {
                SetLayerRecursively(currentAimedObject.gameObject, "Highlight");
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (currentAimedObject != null)
                {
                    if (currentAimedObject.name == "Tutorial")
                    {
                        audioManager.PlaySolutionMelody(0.5f);

                        if (signCanvas.gameObject.activeSelf == false)
                        {
                            signCanvas.gameObject.SetActive(true);
                        }
                        else
                        {
                            signCanvas.gameObject.SetActive(false);
                        }
                    }

                    if (currentAimedObject.name == "Bird_My_Melody")
                    {

                        switch (birdClick)
                        {
                            case 0:
                                if (melodyManager.IsPuzzleSolved())
                                {
                                    birdConvoAnim.SetBool("QuickSolve", true);
                                    audioManager.PlaySolutionMelody(0.3f);
                                }
                                else
                                {
                                    birdConvoAnim.SetBool("QuickSolve", false);
                                }
                                    birdConvoAnim.SetTrigger("FirstClick");
                                    birdClick++;
                                    break;
                            case 1:
                                if (!melodyManager.IsPuzzleSolved())
                                {
                                    birdConvoAnim.SetBool("Solved", false);
                                    birdConvoAnim.SetTrigger("SecondClick");
                                    birdClick = 1;
                                }
                                else
                                {
                                    birdConvoAnim.SetBool("Solved", true);
                                    birdConvoAnim.SetTrigger("SecondClick");
                                    audioManager.PlaySolutionMelody(0.3f);
                                    birdClick = 1;
                                }
                                break;
                        }
                    }

                    if (currentAimedObject.name == "Bird_Solution_Melody")
                    {
                        audioManager.PlaySolutionMelody(0.3f);
                    }

                    if (lastAimedCollider != null & flowerPotsColliders.Contains(lastAimedCollider))
                    {
                        ObjectState objectState;

                        if (!objectStates.TryGetValue(currentAimedObject, out objectState))
                        {
                            objectState = ObjectState.Lowered;
                        }

                        switch (objectState)
                        {
                            case ObjectState.Lowered:
                                // If the player clicks on a lowered MelodyNote object, raise it.
                                RaiseObjectGradually(currentAimedObject);
                                objectStates[currentAimedObject] = ObjectState.Raising;
                                break;
                            case ObjectState.Raising:
                                // Object is already raising, do nothing.
                                break;
                            case ObjectState.Raised:
                                // If the player clicks on a raised MelodyNote object again, lower it.
                                LowerObjectGradually(currentAimedObject);
                                objectStates[currentAimedObject] = ObjectState.Lowering;
                                break;
                            case ObjectState.Swapping:
                                //Objects are swapping, do nothing.
                                break;
                            case ObjectState.Lowering:
                                // Object is already lowering, do nothing.
                                break;
                        }
                    }
                }
            }
        }
        else
        {
            currentAimedObject = null;
            lastAimedCollider = null;

            // Reset the crosshair to its original appearance
            crosshair.transform.localScale = Vector3.one;

            if (lastAimedObject != null)
            {
                //lastHitCollider.gameObject.layer = interactableMask;
                SetLayerRecursively(lastAimedObject.gameObject, "Interactable");
                lastAimedObject = null;
            }
        }
    }

    // Function to gradually raise an object in the y-direction.
    private void RaiseObjectGradually(Transform obj)
    {
        if (!raisedObjects.Contains(obj))
        {
            StartCoroutine(RaiseObjectCoroutine(obj));
            raisedObjects.Add(obj);
        }
    }

    private IEnumerator PlaySwapSound()
    {
        yield return new WaitForSeconds(0.3f);
        audioSource.clip = swapPotsClip;
        audioSource.Play();
    }
    // Coroutine to gradually raise an object.
    private IEnumerator RaiseObjectCoroutine(Transform obj)
    {
        ObjectState objectState;
        if (!objectStates.TryGetValue(obj, out objectState))
        {
            objectState = ObjectState.Lowered;
        }

        float duration = 0.2f; // Duration for raising or lowering.
        float startTime = Time.time;
        Vector3 initialPosition = obj.position;
        Vector3 raisedPosition = initialPosition + Vector3.up * raiseAmount;

        //AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = liftPotClip;
        audioSource.Play();

        ParticleSystem potParticles = currentAimedObject.GetComponentInChildren<ParticleSystem>();


        if (potParticles != null)
        {
            potParticles.Play();
        }

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            obj.position = Vector3.Lerp(initialPosition, raisedPosition, t);
            yield return null;
        }

        obj.position = raisedPosition;

        // Change the object state to "Raised" when done raising.
        objectStates[obj] = ObjectState.Raised; // Set the state to Raised here

        if (firstAimedObject == null)
        {
            firstAimedObject = obj;
        }

        else if (secondAimedObject == null)
        {
            secondAimedObject = obj;
            // When secondAimedObject is raised, swap pieces and interpolate positions.
            if (objectStates[secondAimedObject] == ObjectState.Raised)
            {
                int indexA = firstAimedObject.GetComponent<MelodyNote>().index;
                int indexB = secondAimedObject.GetComponent<MelodyNote>().index;
                melodyManager.SwapPieces(indexA, indexB);

                Debug.Log("A" + indexA + "B" + indexB);

                // Interpolate positions between firstAimedObject and secondAimedObject.
                StartCoroutine(InterpolateObjectPositions(firstAimedObject, secondAimedObject));

                firstAimedObject = null;
                secondAimedObject = null;
            }
        }
    }

    // Function to gradually lower an object back to its original position.
    private void LowerObjectGradually(Transform obj)
    {
        if (raisedObjects.Contains(obj))
        {
            StartCoroutine(LowerObjectCoroutine(obj));
            raisedObjects.Remove(obj);
        }
    }

    // Coroutine to gradually lower an object.
    private IEnumerator LowerObjectCoroutine(Transform obj)
    {
        if (firstAimedObject != null)
        {
            firstAimedObject = null;
        }

        ObjectState objectState;
        if (!objectStates.TryGetValue(obj, out objectState))
        {
            objectState = ObjectState.Raised;
        }

        float duration = 0.2f; // Duration for raising or lowering.
        float startTime = Time.time;
        Vector3 initialPosition = obj.position;
        Vector3 loweredPosition = initialPosition - Vector3.up * raiseAmount;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            obj.position = Vector3.Lerp(initialPosition, loweredPosition, t);
            yield return null;
        }

        obj.position = loweredPosition;

        // Change the object state to "Lowered" when done lowering.
        objectStates[obj] = ObjectState.Lowered; // Set the state to Lowered here
    }

    // Coroutine to interpolate object positions.
    private IEnumerator InterpolateObjectPositions(Transform objA, Transform objB)
    {
        float duration = 0.5f; // Duration for interpolation.
        float startTime = Time.time;
        Vector3 initialPositionA = objA.position;
        Vector3 initialPositionB = objB.position;

        StartCoroutine(PlaySwapSound());

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            objA.position = Vector3.Lerp(initialPositionA, initialPositionB, t);
            objB.position = Vector3.Lerp(initialPositionB, initialPositionA, t);
            yield return null;
        }

        objA.position = initialPositionB; // Set final positions.
        objB.position = initialPositionA;


        // Lower both objects back down to the lowered state.
        LowerObjectGradually(objA);
        LowerObjectGradually(objB);
    }

    // Function to set the layer of a GameObject and all its children recursively
    private void SetLayerRecursively(GameObject obj, string layerName)
    {
        obj.layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layerName);
        }
    }
}
