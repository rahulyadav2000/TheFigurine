using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioClip[] audioClip;   // array for audio clips to be played in the game

    private void Awake()
    {
        instance = this;
    }

    public void ButtonPressSound(int index) // function to play the click button sound through UI button
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioClip[index]);
        }
    }
}
