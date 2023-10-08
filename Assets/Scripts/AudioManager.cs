using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public MelodyManager melodyManager;

    private bool isPlayingMelody = false;

    // Play audio clips from MelodyNotes in the current order.
    public void PlayCurrentMelody(float delay)
    {
        // Check if the PlayMelody coroutine is already running
        if (isPlayingMelody)
        {
            Debug.Log("PlayMelody is already running. Skipping.");
            return;
        }

        StartCoroutine(PlayMelodyCoroutine(delay));
    }

    private IEnumerator PlayMelodyCoroutine(float delay)
    {
        isPlayingMelody = true;

        foreach (MelodyNote melodyNote in melodyManager.currentOrder)
        {
            if (melodyNote != null)
            {
                // Play the audio clip associated with the MelodyNote.
                melodyNote.PlayAudio();

                yield return new WaitForSeconds(delay);
            }
        }

        isPlayingMelody = false;
    }

    public void PlaySolutionMelody(float delay)
    {
        // Check if the PlayMelody coroutine is already running
        if (isPlayingMelody)
        {
            Debug.Log("PlayMelody is already running. Skipping.");
            return;
        }

        StartCoroutine(PlaySolutionCoroutine(delay));
    }

    private IEnumerator PlaySolutionCoroutine(float delay)
    {
        isPlayingMelody = true;

        foreach (MelodyNote melodyNote in melodyManager.solutionOrder)
        {
            if (melodyNote != null)
            {
                // Play the audio clip associated with the MelodyNote.
                melodyNote.PlayAudio();

                yield return new WaitForSeconds(delay);
            }
        }

        isPlayingMelody = false;
    }
}
