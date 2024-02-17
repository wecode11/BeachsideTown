using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphineEvent : MonoBehaviour
{
    public AudioClip splashClip;
    public AudioClip chirpClip;

    public void PlaySplash()
    {
        GetComponent<AudioSource>().PlayOneShot(splashClip);
    }

    public void PlayChirp()
    {
        GetComponent<AudioSource>().PlayOneShot(chirpClip);
    }
}
