using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggering : MonoBehaviour
{
    [Header("Scripts")]
    public Cinematic cinematicScript;
    public UI uiScript;
    public CinematicDialogue dialogueScript;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoothCollider") && cinematicScript.boothOpen == true && !dialogueScript.dialogueOver)
        {
            cinematicScript.canEnter = true;
            uiScript.cinematicMode = true;
            cinematicScript.EnterBooth();
        }
    }
}
