using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public MelodyPuzzle melodyPuzzle;

    public bool isPlayingMelody = false;

    // Play audio clips from MelodyNotes in the current order.
    public void PlayCurrentMelody(float delay)
    {
        // Check if the PlayMelody coroutine is already running
        if (isPlayingMelody)
        {
            return;
        }

        StartCoroutine(PlayMelodyCoroutine(delay));
    }

    private IEnumerator PlayMelodyCoroutine(float delay)
    {
        isPlayingMelody = true;

        foreach (MelodyNote melodyNote in melodyPuzzle.currentOrder)
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
            return;
        }

        StartCoroutine(PlaySolutionCoroutine(delay));
    }

    private IEnumerator PlaySolutionCoroutine(float delay)
    {
        isPlayingMelody = true;

        foreach (MelodyNote melodyNote in melodyPuzzle.solutionOrder)
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
