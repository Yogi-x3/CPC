using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggering : MonoBehaviour
{

    public Cinematic cinematicScript;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoothCollider") && cinematicScript.boothOpen == true && !cinematicScript.kickedOut)
        {


            cinematicScript.canEnter = true;
            cinematicScript.isMoving = true;
            cinematicScript.cinematicMode = true;
            cinematicScript.EnterBooth();
            //cinematicScript.popeStartTime = Time.time;
        }
    }
}
