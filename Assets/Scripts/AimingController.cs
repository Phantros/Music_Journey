using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class AimingController : MonoBehaviour
{
    public LayerMask InteractableLayer; // The layer containing interactable objects.
    public GameObject crosshair; // Reference to your crosshair UI element.
    public MelodyManager melodyManager;
    public AudioManager audioManager;
    private Transform currentAimedObject; // The currently aimed-at object (if any).
    private Transform firstAimedObject; // The first object the player aims at.
    private Transform secondAimedObject; // The second object the player aims at.
    private float raiseAmount = 0.3f;
    private List<Transform> raisedObjects = new List<Transform>();


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

    private void Update()
    {
        // Create a ray from the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // Declare a RaycastHit variable to store information about the hit
        RaycastHit hit;

        // Check if the ray hits an object on the interactable layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, InteractableLayer))
        {
            // Store the currently aimed-at object
            currentAimedObject = hit.transform;

            // Make the crosshair change color or appearance to indicate targeting
            // You can also implement a UI element for this.
            // For simplicity, you can scale the crosshair up slightly.
            crosshair.transform.localScale = Vector3.one * 1.2f;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (currentAimedObject != null)
                {
                    if (currentAimedObject.name == "Parrot_Solution_1")
                    {
                        StartCoroutine(audioManager.PlayMelody(0.5f));

                        if (melodyManager.IsPuzzleSolved())
                        {
                            Debug.Log("Solved");
                        }
                    }
                    if (currentAimedObject.GetComponent<MelodyNote>() != null)
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
                                // Object is currently swapping, do nothing.
                                break;
                            case ObjectState.Lowering:
                                // Object is already lowering, do nothing.
                                break;
                        }
                    }
                    else
                    {
                        // Handle interactions with non-MelodyNote objects here.
                    }
                }
            }

        }
        else
        {
            // No object is being aimed at
            currentAimedObject = null;

            // Reset the crosshair to its original appearance
            crosshair.transform.localScale = Vector3.one;
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
                int indexA = firstAimedObject.GetComponent<MelodyNote>().Index;
                int indexB = secondAimedObject.GetComponent<MelodyNote>().Index;
                melodyManager.SwapPieces(indexA, indexB);

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
}
