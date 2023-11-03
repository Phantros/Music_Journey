using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoorTrigger : MonoBehaviour
{
    public Animator doorAnimator;
    public AudioClip openDoorsClip;
    public GameObject winTrigger;

    private AudioSource audioSource;

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayDoorSound();
            winTrigger.SetActive(true);
            doorAnimator.SetBool("PuzzleSolved", true);          
        }
    }

    private void PlayDoorSound()
    {
        audioSource.clip = openDoorsClip;
        audioSource.Play();
    }
}
