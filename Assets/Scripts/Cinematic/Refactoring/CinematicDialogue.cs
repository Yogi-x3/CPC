using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CinematicDialogue : MonoBehaviour
{

    public Cinematic cinematicScript;

    public GameObject dialogueHolder;


    [Header("Endings")]
    public bool isAbsolved;
    public bool dialogueOver = false;


    [Header("PopeDialogue")]
    public string[] dialogue;
    public string[] dialogue2;
    public string[] dialogue3;

    private int dtrack;

    public int d = 0;
    public TMP_Text dialogueObject;

    [Header("PlayerDialogue")]
    public string[] goodDialogue;
    public string[] goodDialogue2;
    public string[] badDialogue;
    public string[] badDialogue2;

    private int gdTrack;
    private int bdTrack;

    public int gd = 0;
    public int bd = 0;
    public TMP_Text goodText;
    public TMP_Text badText;

    public bool confessedMurder;
    public bool waitForSpeech;

    public GameObject player;






    public AudioandFX FXscript;
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

        if (dtrack == 3)
        {
            dialogueObject.text = dialogue3[d];
        }

        if (d > 17)
        {
            d = 17;
        }

        if (waitForSpeech)
        {
            dialogueHolder.SetActive(false);
        }
        else
        {
            dialogueHolder.SetActive(true);
        }

    }

    public void Yes()
    {

        waitForSpeech = true;
        cinematicScript.popeAnimation.SetBool("Yes", true);
        d = 1;
        cinematicScript.popeStartPoint.position = cinematicScript.Pope.transform.position;

        StartCoroutine(cinematicScript.Delay(cinematicScript.BoothOpen, 7f));


    }

    public void No()
    {
        cinematicScript.cinematicMode = false;
    }

    public void DialogueOption()
    {
        var button = EventSystem.current.currentSelectedGameObject;

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
                //gloating
                if (d == 11 && confessedMurder)
                {
                    dialogueOver = true;
                }
            }

            //Calculate sin
            cinematicScript.storedSin = cinematicScript.sinMeter;
            cinematicScript.sinLerpTimer = 0f;


            float calcuatedSin = 10 * (1 / cinematicScript.sinTimer);

            float roundedSin = Mathf.Round(calcuatedSin);

            float sinClamp = Mathf.Clamp(roundedSin, 1, 10);

            if (button.tag == "Good")
            {
                cinematicScript.actualSin -= sinClamp;
            }

            if (button.tag == "Bad")
            {
                cinematicScript.actualSin += sinClamp;
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

            //End of dialgoue
            if (d == 16)
            {
                isAbsolved = true;
                dialogueOver = true;
            }

            if (dialogueOver && cinematicScript.sinMeter < 50) 
            {
                cinematicScript.LeaveBooth();
            }
            
        }

    }

    public IEnumerator SpeechDelay()
    {
        waitForSpeech = true;
        yield return new WaitForSeconds(3f);
        waitForSpeech = false;
    }
}
