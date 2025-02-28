using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public AudioSource sniffSource;
    // Start is called before the first frame update
    public void Sniff()
    {
        sniffSource.Play();
    }
}
