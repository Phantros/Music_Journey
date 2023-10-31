using UnityEngine;
using UnityEngine.InputSystem;

public class StartAreaController : MonoBehaviour
{
    public GameObject objectToMove;
    public GameObject targetObject;

    private GameObject firstAimedObject;
    private GameObject secondAimedObject;
    private MoveStartPot moveObjectScript;
    private bool isRaised = false;
    private Vector3 originalPosition;
    private float raiseAmount = 0.5f;
    private bool objectRaised = false;
    private bool clickedOnObjectToMove = false;

    void Start()
    {
        moveObjectScript = objectToMove.GetComponent<MoveStartPot>();
        originalPosition = objectToMove.transform.position;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject == targetObject && clickedOnObjectToMove)
                {
                    StartMovingObject();
                }

                if (hit.transform.gameObject == objectToMove)
                {
                    if (!objectRaised)
                    {
                        // Raise the object by the specified amount
                        objectToMove.transform.position += new Vector3(0, raiseAmount, 0);
                        isRaised = true;
                        objectRaised = true;
                        clickedOnObjectToMove = true;
                    }
                    else
                    {
                        // Lower the object back to its original position
                        objectToMove.transform.position = originalPosition;
                        isRaised = false;
                        objectRaised = false;
                        clickedOnObjectToMove = false;
                    }
                }
            }
        }
    }


    public void StartMovingObject()
    {
        moveObjectScript.StartMoving();
    }
}
