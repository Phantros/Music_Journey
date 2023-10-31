using UnityEngine;
using System.Collections;

public class MoveStartPot : MonoBehaviour
{
    public GameObject startTreeStump; // The object you want to move towards

    private float stumpXOffset = -0.04f;
    private float stumpYOffset = 1.52f;
    private Vector3 startPotPos;
    private Vector3 startTreeStumpPos;
    private float moveDuration = 2.0f; // Adjust the duration as needed
    private float timer = 0f;

    private bool isMoving = false;

    void Start()
    {
        startPotPos = transform.position;
        startTreeStumpPos = startTreeStump.transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            timer += Time.deltaTime;
            float t = timer / moveDuration;
            transform.position = Vector3.Lerp(startPotPos, new Vector3(startTreeStumpPos.x - stumpXOffset, startTreeStumpPos.y + stumpYOffset, startTreeStumpPos.z), t);

            if (t >= 1.0f)
            {
                isMoving = false;
                startTreeStump.GetComponent<BoxCollider>().enabled = false;
                this.gameObject.GetComponent<BoxCollider>().enabled = false; 
            }
        }
    }

    public void StartMoving()
    {
        if (!isMoving)
        {
            startPotPos = transform.position;
            isMoving = true;
            timer = 0f;
        }
    }
}