using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip audioTurnTetro;
    public AudioClip audioClearLine;
    public AudioClip audioButton;
    public AudioClip audioGameOver;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AudioPlay(string what)
    {
        switch (what)
        {
            case "Turn":
                audioSource.clip = audioTurnTetro;
                break;
            case "Clear":
                audioSource.clip = audioClearLine;
                break;
            case "Button":
                audioSource.clip = audioButton;
                break;
            case "Over":
                FindObjectOfType<Director>().GetComponent<AudioSource>().Stop();
                audioSource.clip = audioGameOver;
                break;
        }

        audioSource.Play();
    }
}
