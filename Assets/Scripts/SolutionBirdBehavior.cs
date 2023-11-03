using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SolutionBirdBehavior : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioSource audioSource;

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
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Create a ray from the camera to the mouse cursor position
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object is the same as this object
                if (hit.collider.gameObject == gameObject)
                {
                    PerformInteraction();
                }
            }
        }
    }

    private void PerformInteraction()
    {
        audioManager.PlaySolutionMelody(0.3f);
    }
}
