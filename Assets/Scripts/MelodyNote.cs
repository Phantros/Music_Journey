using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Import the InputSystem namespace

public class MelodyNote : MonoBehaviour
{
    public int index;
    public AudioClip melodyClip;

    public Collider flowerCollider;// Reference to the MP3 audio clip.
    public List<GameObject> melodyBobbingNotes;

    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();

        // Add an AudioSource component if one doesn't exist.
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void Update()
    {
        // Check if an audio clip is assigned.
        if (melodyClip != null)
        {
            // Create a ray from the center of the screen
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            // Check if the ray hits this object
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                // Check if the left mouse button is pressed.
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (hit.collider == flowerCollider)
                    {
                        animator.SetTrigger("Clicked");

                        // Play the assigned audio clip.
                        audioSource.clip = melodyClip;
                        audioSource.Play();
                        foreach (GameObject bobbingNotes in melodyBobbingNotes)
                        {
                            bobbingNotes.SetActive(true);
                        }
                    }
                }
            }
        }
        else
        {
            return;
        }

        if(audioSource.isPlaying == false)
        {
            foreach (GameObject bobbingNotes in melodyBobbingNotes)
            {
                bobbingNotes.SetActive(false);
            }
        }
    }

    public void PlayAudio()
    {
        animator.SetTrigger("Clicked");

        if(melodyClip != null && audioSource != null)
        {
            audioSource.clip = melodyClip;
            audioSource.Play();

            foreach (GameObject bobbingNotes in melodyBobbingNotes)
            {
                bobbingNotes.SetActive(true);
            }
        }
    }
}
