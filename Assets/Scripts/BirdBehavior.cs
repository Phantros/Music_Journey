using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdBehavior : MonoBehaviour
{
    public Animator birdConvoAnim;
    public AudioManager audioManager;
    public ParticleSystem solvedParticles;
    public Animator doorAnimator;
    public AudioClip openDoorsClip, solvedPuzzleClip;
    public MelodyManager melodyManager;
    public List<Collider> solvedPuzzleElements;

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
                    StartCoroutine(SolvedPuzzle());
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
                    StartCoroutine(SolvedPuzzle());
                    birdClick = 1;
                }
                break;
        }
    }

    private IEnumerator SolvedPuzzle()
    {
        solvedParticles.Play();

        foreach(Collider solvedObjectCollider in solvedPuzzleElements)
        {
            solvedObjectCollider.enabled = false;
        }

        audioManager.PlaySolutionMelody(0.3f);
        while (audioManager.isPlayingMelody)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        audioSource.clip = solvedPuzzleClip;
        audioSource.Play();

        while (audioSource.isPlaying)
        {
            yield return null;
        }

        doorAnimator.SetBool("PuzzleSolved", true);
        audioSource.clip = openDoorsClip;
        audioSource.Play();
    }
}
