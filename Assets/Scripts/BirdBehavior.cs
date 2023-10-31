using UnityEngine;
using UnityEngine.InputSystem;

public class BirdBehavior : MonoBehaviour
{
    public Animator birdConvoAnim;
    public MelodyManager melodyManager;
    public AudioManager audioManager;
    public ParticleSystem solvedParticles;
    public Animator doorAnimator;
    public AudioClip openDoorsClip;

    private AudioSource audioSource;
    private int birdClick = 0;

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
        switch (birdClick)
        {
            case 0:
                if (melodyManager.IsPuzzleSolved())
                {
                    birdConvoAnim.SetBool("QuickSolve", true);
                    audioManager.PlaySolutionMelody(0.3f);
                    solvedParticles.Play();
                    doorAnimator.SetBool("PuzzleSolved", true);
                    PlayDoorSound();
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
                    audioManager.PlayCurrentMelody(0.3f);
                    birdClick = 1;
                }
                else
                {
                    birdConvoAnim.SetBool("Solved", true);
                    birdConvoAnim.SetTrigger("SecondClick");
                    audioManager.PlaySolutionMelody(0.3f);
                    solvedParticles.Play();
                    PlayDoorSound();
                    doorAnimator.SetBool("PuzzleSolved", true);
                    birdClick = 1;
                }
                break;
        }
    }

    private void PlayDoorSound()
    {
        audioSource.clip = openDoorsClip;
        audioSource.Play();
    }
}
