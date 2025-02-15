using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CinematicDialogue : MonoBehaviour
{
    [Header("Scripts")]
    public Cinematic cinematicScript;
    public UI uiScript;
    public AudioandFX FXscript;

    [Header("Endings")]
    public bool isAbsolved;
    public bool dialogueOver = false;
    public bool confessedMurder;


    [Header("PopeDialogue")]
    public string[] dialogue;
    public string[] dialogue2;
    public string[] dialogue3;
    private int dtrack;
    public int d = 0;
    public int dLimit = 17;
    public TMP_Text dialogueObject;

    [Header("GoodDialogue")]
    public string[] goodDialogue;
    public string[] goodDialogue2;
    private int gdTrack;
    public int gd = 0;
    public TMP_Text goodText;

    [Header("BadDialogue")]
    public string[] badDialogue;
    public string[] badDialogue2;
    private int bdTrack;
    public int bd = 0;
    public TMP_Text badText;

    [Header("PlayerDialogue")]
    public GameObject dialogueHolder;
    public bool waitForSpeech;
    // Start is called before the first frame update
    void Start()
    {
        dtrack = 1;
        confessedMurder = false;
    }

    public void DialogueManager()
    {

        if (dtrack == 1)
        {
            dialogueObject.text = dialogue[d];
            badText.text = badDialogue[bd];
            goodText.text = goodDialogue[gd];
        }
        //use second set of dialogue unless the dialgoue is blank, then use first set
        if (dtrack == 2)
        {
            dialogueObject.text = dialogue2[d];

           
            if (badDialogue2[bd] != "")
            {
                badText.text = badDialogue2[bd];
            }
            else
            {
                badText.text = badDialogue[bd];
            }


            if (goodDialogue2[gd] != "")
            {
                goodText.text = goodDialogue2[gd];
            }
            else
            {
                goodText.text = goodDialogue[gd];
            }
        }
        //use third set of dialogue
        if (dtrack == 3)
        {
            dialogueObject.text = dialogue3[d];
        }
        //dont allow speech to go above limit
        if (d > dLimit)
        {
            d = dLimit;
        }
        //hide dialogue while coroutine running
        if (waitForSpeech == true)
        {
            dialogueHolder.SetActive(false);
        }
        else
        {
            dialogueHolder.SetActive(true);
        }

    }
    //set pope transform start point and progress dialogue
    public void Yes()
    {
        waitForSpeech = true;
        uiScript.popeAnimation.SetBool("Yes", true);
        d = 1;
        cinematicScript.popeStartPoint.position = cinematicScript.Pope.transform.position;
        StartCoroutine(cinematicScript.Delay(cinematicScript.BoothOpen, 7f));
    }

    //return to start state
    public void No()
    {
        uiScript.cinematicMode = false;
    }

    //get button from event system checking the tag to see which option has been chosen
    public void DialogueOption()
    {
        var button = EventSystem.current.currentSelectedGameObject;
        //Only yes and no accesible before in the booth
        if (!cinematicScript.isConfessing)
        {
            if (button.tag == "Good")
            {
                Yes();
            }
            if (button.tag == "Bad")
            {
                No();
            }
        }
        //triggers coroutine to hide dialogue on click
        if (cinematicScript.isConfessing)
        {
            StartCoroutine(SpeechDelay());

            if (button.tag == "Good")
            {
                //track one unless confessing to murder
                if (d == 13 && confessedMurder)
                {
                    dtrack = 2;
                }
                else
                {
                    dtrack = 1;
                }
                //Skipping over "and nothig else" repeats
                if (d == 8 || d == 9)
                {
                    d = 10;
                    gd = 9;
                    bd = 9;
                }
            }

            if (button.tag == "Bad")
            {
                //confessing to murder
                if (d == 10)
                {
                    confessedMurder = true;
                }
                //gloating. Set sin to 0 so as to not trigger damnation
                if (d == 11 && confessedMurder)
                {
                    dialogueOver = true;
                    uiScript.sinMeter = 0;
                }
            }

            //Calculate sin and reset
            uiScript.storedSin = uiScript.sinMeter;
            uiScript.sinLerpTimer = 0f;

            //keep sin in whole numbers. Check which option chosen to know if to add or subtract
            float calcuatedSin = 10 * (1 / uiScript.sinTimer);

            float roundedSin = Mathf.Round(calcuatedSin);

            float sinClamp = Mathf.Clamp(roundedSin, 1, 10);

            if (button.tag == "Good")
            {
                uiScript.actualSin -= sinClamp;
            }

            if (button.tag == "Bad")
            {
                uiScript.actualSin += sinClamp;
            }

            //progress dialogue
            gd++;
            bd++;
            d++;

            if (button.tag == "Bad")
            {
                //if bad option, check whether pope dialgoue is empty
                if (dialogue3[d] != "")
                {
                    dtrack = 3;
                }
                else if (dialogue3[d] == "")
                {
                    dtrack = 2;
                }

                if (dialogue3[d] == "" && dialogue2[d] == "")
                {
                    dtrack = 1;
                }

                //dont agree to confess
                if (d == 6)
                {
                    dialogueOver = true;
                }
            }

            //End of dialgoue, either good or bad
            if (d == 16)
            {
                isAbsolved = true;
                dialogueOver = true;
            }
            //release from booth unless Damnation ending
            if (dialogueOver && uiScript.sinMeter < 50) 
            {
                cinematicScript.LeaveBooth();
            }
        }
    }

    //allow time for player to read text before countdown
    public IEnumerator SpeechDelay()
    {
        waitForSpeech = true;
        yield return new WaitForSeconds(3f);
        waitForSpeech = false;
    }
}
