using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CinematicDialogue : MonoBehaviour
{

    public Cinematic cinematicScript;

    [Header("Cinematic")]
    public bool cinematicMode;
    public float sinMeter;
    private float actualSin;
    private float storedSin;
    public float sinLerpTimer;

    public float sinTimer;

    public GameObject dialogueHolder;


    [Header("Endings")]
    private bool isAbsolved;
    public bool dialogueOver = false;


    [Header("Booth")]
    public GameObject booth;

    [Header("Player")]

    public GameObject player;



    [Header("Checks")]
    public bool isConfessing;
    public bool canEnter;


    [Header("PopeDialogue")]
    public string[] dialogue;
    public string[] dialogue2;
    public string[] dialogue3;

    private int dtrack;

    private int d = 0;
    public TMP_Text dialogueObject;

    [Header("PlayerDialogue")]
    public string[] goodDialogue;
    public string[] goodDialogue2;
    public string[] badDialogue;
    public string[] badDialogue2;

    private int gdTrack;
    private int bdTrack;

    private int gd = 0;
    private int bd = 0;
    public TMP_Text goodText;
    public TMP_Text badText;

    private bool confessedMurder;
    private bool waitForSpeech;

    [Header("Pope")]
    public GameObject Pope;
    public Transform popeHead;
    public float popeStartTime;
    public Transform popeStartPoint;
    public Animator popeAnimation;





    public AudioandFX FXscript;
    // Start is called before the first frame update
    void Start()
    {
        dtrack = 1;
        confessedMurder = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void Confessing()
    {
        if (player.transform.position == booth.transform.position)
        {

            cinematicScript.isConfessing = true;
            cinematicScript.canEnter = false;
            d = 2;

        }


    }

    public void GoodOption()
    {
        var button = EventSystem.current.currentSelectedGameObject;

        if (!isConfessing)
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

        if (isConfessing)
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
                    cinematicScript.LeaveBooth();

                }
            }

            //Calculate sin
            storedSin = sinMeter;
            sinLerpTimer = 0f;


            float calcuatedSin = 10 * (1 / sinTimer);

            float roundedSin = Mathf.Round(calcuatedSin);

            float sinClamp = Mathf.Clamp(roundedSin, 1, 10);

            if (button.tag == "Good")
            {
                actualSin -= sinClamp;
            }

            if (button.tag == "Bad")
            {
                actualSin += sinClamp;
            }

            //progress dialogue
            gd++;
            bd++;
            d++;

            if (button.tag == "Bad")
            {
                //if bad option, check whether pope dialgoue is empty
                if (dialogue3[d] == "")
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
                    cinematicScript.LeaveBooth();
                    dialogueOver = true;
                }
            }

            //End of dialgoue
            if (d == 16)
            {
                isAbsolved = true;
                dialogueOver = true;
                //Stay in booth for Damnation
                if (sinMeter <= 50)
                {
                    cinematicScript.LeaveBooth();
                }
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
