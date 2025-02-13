using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cinematic : MonoBehaviour
{
    [Header("Cinematic")]
    public Animator blackBars;
    public bool cinematicMode;
    public float interactionDistance = 5f;
    public float sinMeter;
    private float actualSin;
    private float storedSin;
    public float sinLerpTimer;
    public Image sinBar;
    public GameObject sinBarHolder;
    public Image sinTimeBar;
    public float sinTimer;
    private float FOVtimer;
    public GameObject dialogueHolder;
    public GameObject priestDialogueHolder;

    [Header("Endings")]
    private bool isAbsolved;
    public GameObject smokeObject;
    public ParticleSystem smoke;
    private float cellTimer;
    //private float maxCell = 5;
    private float cells;
    public GameObject distortionPlane;
    public Renderer distortion;
    //public float distortionSize;
    public GameObject endScreen;
    public TMP_Text endScreenText;
    public bool dialogueOver = false;
    public Light policeLight;

    private int aud;
    private bool audioPlaying;

    [Header("Booth")]
    public bool kickedOut;
    public GameObject curtain;
    public Animator curtainAnimator;
    private bool curtainAnimating;
    public Renderer boothModel;
    public Transform[] Openings;
    public Transform boothOpening;
    public GameObject window;
    public GameObject booth;

    [Header("Player")]
    public Camera cam;
    public GameObject player;
    public Rigidbody playerRb;
    private float startTime;
    private float journeyDistance;
    public float moveSpeed;
    public FirstPersonMovement movementScript;

    [Header("Checks")]
    public bool isMoving;
    public bool isConfessing;
    public bool boothOpen;
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
    public Renderer popeBody;
    public Texture2D[] popeTex;
    public Transform popeHead;
    public GameObject popeBooth;
    public GameObject outerBooth;
    public float popeStartTime;
    private float popeJourneyDistance;
    public Transform popeStartPoint;
    public GameObject agent;
    public Animator popeAnimation;
    public AudioSource popeAudio;
    public AudioClip[] popeSounds;
    private bool isBreathing;

    public delegate void TestDelegate();
    public TestDelegate methodToCall;
    public bool delegateCoroutineRunning;

    public AudioandFX FXscript;
    public CinematicDialogue dialogueScript;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        curtainAnimator.SetBool("InBooth", false);
        window = GameObject.FindGameObjectWithTag("Window");
        booth = GameObject.FindGameObjectWithTag("Slot");
        curtain = GameObject.FindGameObjectWithTag("Curtain");
        curtainAnimator = curtain.GetComponent<Animator>();
        sinMeter = 0f;
        dtrack = 1;
        confessedMurder = false;
        cells = 1.7f;
        distortionPlane.SetActive(false);
        endScreen.SetActive(false);
        policeLight.enabled = false;
        audioPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Test
        CinematicMode();


        if (canEnter == true)
        {
            PlayerWalk();
            Confessing();
        }

        ClosestBooth();

        DialogueManager();

        PopeWalk();

        if (dialogueOver == true)
        {
            Endings();
        }

        SinBars();
        
    }

    void CinematicMode()
    {
        //interact with Priest
        RaycastHit hit;
        float rayDistance = 100f;

        Debug.DrawRay(cam.transform.position, cam.transform.forward * rayDistance, Color.red);
        if (boothOpen == false)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rayDistance))
            {

                if (hit.collider.CompareTag("Pope"))
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        cinematicMode = true;
                        d = 0;


                    }
                }
            }
        }

        //displaying text if FOV is 80
        if (cam.fieldOfView != 80)
        {
            dialogueHolder.SetActive(false);
            priestDialogueHolder.SetActive(false);
        }
        else
        {
            dialogueHolder.SetActive(true);
            priestDialogueHolder.SetActive(true);
        }



        if (cinematicMode)
        {
            //priest looks at player
            Vector3 targetPostition = new Vector3(cam.transform.position.x, agent.transform.position.y, cam.transform.position.z);
            agent.transform.LookAt(targetPostition);

            //set player minimum distance from priest
            float cinematicDistance = 7f;

            if (Vector3.Distance(player.transform.position, agent.transform.position) < cinematicDistance)
            {
                player.transform.position = player.transform.position + (Pope.transform.forward);
            }

            //Zoom FOV when in interaction
            FOVtimer += Time.deltaTime;
            float FOVwaitTime = 1f;

            if (FOVtimer > FOVwaitTime)
            {
                FOVtimer = FOVwaitTime;
            }

        }

        else
        {
            FOVtimer -= 2 * Time.deltaTime;

            if (FOVtimer < 0)
            {
                FOVtimer = 0;
            }

        }
        
        cam.fieldOfView = Mathf.Lerp(60, 80, FOVtimer);
        blackBars.SetBool("isCinematic", cinematicMode);
        curtainAnimator.SetBool("InBooth", isConfessing);
        popeAnimation.SetBool("isWalking", !cinematicMode);
        movementScript.enabled = !cinematicMode;

    }

    public void Yes()
    {

        waitForSpeech = true;
        popeAnimation.SetBool("Yes", true);
        d = 1;
        popeStartPoint.position = Pope.transform.position;

        StartCoroutine(Delay(BoothOpen, 7f));


    }

    public void No()
    {
        cinematicMode = false;
    }

    public void Confessing()
    {
        if (player.transform.position == booth.transform.position)
        {
            
            isConfessing = true;
            canEnter = false;
            d = 2;

        }


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


    public void PopeWalk()
    {

        var priestMovement = agent.GetComponent<PriestMovement>();
        priestMovement.enabled = false;

        float popeMoveSpeed = movementScript.moveSpeed * 0.25f;
        float distCovered = (Time.time - popeStartTime) * popeMoveSpeed;
        float fractionOfJourney = distCovered / popeJourneyDistance;

        Vector3 targetPostition = new Vector3(player.transform.position.x, Pope.transform.position.y, player.transform.position.z);
        Pope.transform.LookAt(targetPostition);

        if (boothOpen)
        {
            var targetLocation = outerBooth.transform.position;
            Pope.transform.position = Vector3.Lerp(popeStartPoint.position, targetLocation, fractionOfJourney);

        }

        if (isMoving)
        {
            var targetLocation = popeBooth.transform.position;
            Pope.transform.position = Vector3.Lerp(outerBooth.transform.position, targetLocation, fractionOfJourney);
        }
    }

    public void PlayerWalk()
    {
        d = 17;
        float automaticMoveSpeed = movementScript.moveSpeed / 3;
        float distCovered = (Time.time - startTime) * automaticMoveSpeed;
        float fractionOfJourney = distCovered / journeyDistance;


        player.transform.position = Vector3.Lerp(boothOpening.position, booth.transform.position, fractionOfJourney);
        Vector3 targetPostition = new Vector3(window.transform.position.x, player.transform.position.y, window.transform.position.z);
        player.transform.LookAt(targetPostition);
    }

    public void EnterBooth()
    {
        startTime = Time.time;
        journeyDistance = Vector3.Distance(boothOpening.position, booth.transform.position);
        boothOpen = false;
    }

    public void ClosestBooth()
    {
        if (canEnter != true)
        {
            float distanceTobooth1 = Vector3.Distance(player.transform.position, Openings[0].transform.position);
            float distanceTobooth2 = Vector3.Distance(player.transform.position, Openings[1].transform.position);

            if (distanceTobooth1 < distanceTobooth2)
            {
                boothOpening = Openings[0];
                Openings[1].gameObject.SetActive(false);
                Openings[0].gameObject.SetActive(true);
            }
            else
            {
                boothOpening = Openings[1];
                Openings[0].gameObject.SetActive(false);
                Openings[1].gameObject.SetActive(true);
            }

            window = GameObject.FindGameObjectWithTag("Window");
            booth = GameObject.FindGameObjectWithTag("Slot");
            curtain = GameObject.FindGameObjectWithTag("Curtain");
            curtainAnimator = curtain.GetComponent<Animator>();
            smokeObject = GameObject.FindGameObjectWithTag("Smoke");
            smoke = smokeObject.GetComponent<ParticleSystem>();

            smoke.Stop();

        }

        FXscript.OpenBoothVFX();

    }


    void KickOut()
    {
        Vector3 kickOutForce = new Vector3(-100.0f, 0.0f, 0.0f);
        playerRb.AddForce(kickOutForce, ForceMode.Impulse);
        cinematicMode = false;
    }



    public void BoothOpen()
    {
        popeAnimation.SetBool("Yes", false);
        cinematicMode = false;
        waitForSpeech = false;
        gd++;
        bd++;
        boothOpen = true;
        
        popeStartTime = Time.time;
        popeJourneyDistance = Vector3.Distance(popeStartPoint.position, outerBooth.transform.position);

    }


    public IEnumerator Delay(TestDelegate method, float time)
    {
        delegateCoroutineRunning = true;
        yield return new WaitForSeconds(time);
        method();
        delegateCoroutineRunning = false;
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
                    LeaveBooth();

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
                    LeaveBooth();
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
                    LeaveBooth();
                }
            }
        }

    }

    public void LeaveBooth()
    {
        int blankDialogue = 17;
        d = blankDialogue;
        kickedOut = true;
        if (!delegateCoroutineRunning)
        {
            StartCoroutine(Delay(KickOut, 3f));
        }
        dialogueHolder.SetActive(false);
        isConfessing = false;
    }

    public void Endings()
    {

        if (isAbsolved == true)
        {
            if (actualSin >= 50f)
            {
                Damnation();
            }

            if (sinMeter <= 0)
            {
                Absolved();
            }

            if (0f < actualSin && actualSin < 50f)
            {
                if (confessedMurder)
                {
                    Police();
                }
                else
                {
                    Neutral();
                }
            }
        }

        else
        {
            Guilt();
        }

        FXscript.EndingAudio(aud);

    }

    public void Absolved()
    {
        endScreenText.text = "ABSOLVED";
        aud = 0;
        CutToBlack();
    }

    public void Police()
    {

        policeLight.enabled = true;
        endScreenText.text = "POLICE";
        if (!FXscript.lightChanging)
        {
            StartCoroutine(FXscript.ChangeLight());
        }
        aud = 1;
        CutToBlack();
    }

    public void Neutral()
    {
        endScreenText.text = "NEUTRAL";
        aud = 2;
        CutToBlack();
    }

    public void Guilt()
    {
        endScreenText.text = "GUILT";
        aud = 3;
        CutToBlack();
    }

    void Damnation()
    {
        endScreenText.text = "DAMNATION";
        aud = 4;
        distortionPlane.SetActive(true);
        smoke.Play();
        
        float maxCell = 5f;
        float distortionSize = Mathf.Lerp(0, 1, cellTimer);
        distortion.material.SetFloat("_HeatDistortion", distortionSize);

        cells = Mathf.Lerp(1.7f, maxCell, cellTimer);
        boothModel.material.SetFloat("_Cell_size", cells);
        
        popeBody.material.SetTexture("_Texture2D", popeTex[6]);

        if (cells < maxCell)
        {
            cellTimer += Time.deltaTime / 20;
        }

        if (cells == maxCell)
        {
            CutToBlack();
        }

    }

    public void EndScreen()
    {
        endScreen.SetActive(true);
    }

    public void CutToBlack()
    {
        if (!delegateCoroutineRunning)
        {
            StartCoroutine(Delay(EndScreen, 5f));
        }
    }


    public IEnumerator SpeechDelay()
    {
        waitForSpeech = true;
        yield return new WaitForSeconds(3f);
        waitForSpeech = false;
    }

    public void SinBars()
    {
        if (isConfessing)
        {
            sinTimeBar.enabled = true;
            sinBarHolder.SetActive(true);
            sinBar.fillAmount = sinMeter / 50;

            if (!waitForSpeech)
            {
                sinTimer += Time.deltaTime;
                float clampedTime = Mathf.Clamp(sinTimer, 0.0f, 10.0f);
                sinTimeBar.enabled = true;

                sinTimeBar.transform.localScale = new Vector3(1 - clampedTime / 10, 1, 1);
            }
            else
            {
                sinTimer = 0.0f;
                sinTimeBar.enabled = false;
            }


            if (actualSin < 0.0f)
            {
                actualSin = 0.0f;
            }

            if (sinLerpTimer < 1f)
            {
                sinLerpTimer += Time.deltaTime;
                sinMeter = Mathf.Lerp(storedSin, actualSin, sinLerpTimer);
            }
        }
        else
        {

            sinTimeBar.enabled = false;
            sinBarHolder.SetActive(false);
        }

        FXscript.FireFX();
        FXscript.PriestEmotions();
    }

}
