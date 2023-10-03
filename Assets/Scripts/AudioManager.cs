using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public MelodyManager melodyManager;

    // Play audio clips from MelodyNotes in the current order.
    public IEnumerator PlayMelody(float delay)
    {
        foreach (MelodyNote melodyNote in melodyManager.currentOrder)
        {
            if (melodyNote != null)
            {
                // Play the audio clip associated with the MelodyNote.
                melodyNote.PlayAudio();

                yield return new WaitForSeconds(delay);
            }
        }
    }
}
