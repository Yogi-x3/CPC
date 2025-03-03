using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public AudioSource sniffSource;
    public AudioClip[] animationClips;
    // Start is called before the first frame update
    public void Sniff()
    {
        sniffSource.clip = animationClips[0];
        sniffSource.Play();
    }

    public void Watch()
    {
        sniffSource.clip = animationClips[1];
        sniffSource.Play();
    }

    public void Gasp()
    {
        sniffSource.clip = animationClips[2];
        sniffSource.Play();
    }
}
