using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Cinematic : MonoBehaviour
{
    [Header("Cinematic")]
    public Animator blackBars;
    public bool cinematicMode;
    public float interactionDistance = 5f;
    private float glowTimer;
    public float sinMeter;
    private float actualSin;
    private float storedSin;
    public float sinLerpTimer;
    public Image sinBar;
    public GameObject sinBarHolder;
    public Image sinTimeBar;
    public float sinTimer;
    private float FOVtimer;
    public float FOVwaitTime = 1f;
    public GameObject dialogueHolder;
    public GameObject priestDialogueHolder;
    public ParticleSystem redFire;
    public ParticleSystem orangeFire;
    public ParticleSystem yellowFire;
    private float fireScale;

    [Header("Endings")]
    private bool isAbsolved;
    public GameObject smokeObject;
    public ParticleSystem smoke;
    private float cellTimer;
    private float maxCell = 5;
    private float cells;
    public GameObject distortionPlane;
    public Renderer distortion;
    public float distortionSize;
    public GameObject endScreen;
    public TMP_Text endScreenText;
    private bool dialogueOver = false;
    public Light policeLight;
    private bool redlight = true;
    private bool lightChanging;
    public AudioSource audio;
    public AudioClip[] audioclips;
    private int aud;
    private bool audioPlaying;

    [Header("Booth")]
    public GameObject[] enterPanel;
    public bool kickedOut;
    public GameObject curtain;
    public Animator curtainAnimator;
    private bool curtainAnimating;
    public Renderer boothModel;
    public Renderer[] glowModel;
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
    private bool glowOn;
    private bool glowRun;


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

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        curtainAnimator.SetBool("InBooth", false);
        window = GameObject.FindGameObjectWithTag("Window");
        booth = GameObject.FindGameObjectWithTag("Slot");
        curtain = GameObject.FindGameObjectWithTag("Curtain");
        curtainAnimator = curtain.GetComponent<Animator>();
        glowTimer = 0f;
        sinMeter = 0f;
        dtrack = 1;
        confessedMurder = false;
        cells = 1.7f;
        distortionPlane.SetActive(false);
        endScreen.SetActive(false);
        policeLight.enabled = false;
        audioPlaying = false;
        audio.Stop();
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

        DialgoueManager();

        PopeWalk();

        if (dialogueOver == true)
        {
            Endings();
        }

        SinBars();
        PopeBreathing();
        
    }

    void CinematicMode()
    {
        RaycastHit hit;


        Debug.DrawRay(cam.transform.position, cam.transform.forward * 100f, Color.red);
        if (boothOpen == false)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f))
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

        if (cam.fieldOfView != 80)
        {
            dialogueHolder.SetActive(false);
            priestDialogueHolder.SetActive(false);
        }
        else if (!kickedOut)
        {
            dialogueHolder.SetActive(true);
            priestDialogueHolder.SetActive(true);
        }



        if (cinematicMode)
        {
            Vector3 targetPostition = new Vector3(cam.transform.position.x, agent.transform.position.y, cam.transform.position.z);
            agent.transform.LookAt(targetPostition);

            blackBars.SetBool("isCinematic", true);
            popeAnimation.SetBool("isWalking", false);

            FOVtimer += Time.deltaTime;
            cam.fieldOfView = Mathf.Lerp(60, 80, FOVtimer);

            if (FOVtimer > FOVwaitTime)
            {
                FOVtimer = FOVwaitTime;
            }

            float cinematicDistance = 7f;

            if (Vector3.Distance(player.transform.position, agent.transform.position) < cinematicDistance)
            {
                player.transform.position = player.transform.position + (Pope.transform.forward);
            }


        }

        if (!cinematicMode)
        {
            popeAnimation.SetBool("isWalking", true);
            cam.fieldOfView = 60;
            FOVtimer -= 2 * Time.deltaTime;

            cam.fieldOfView = Mathf.Lerp(60, 80, FOVtimer);

            if (FOVtimer < 0)
            {
                FOVtimer = 0;
            }

        }
        blackBars.SetBool("isCinematic", cinematicMode);
        curtainAnimator.SetBool("InBooth", isConfessing);
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

    public void DialgoueManager()
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
        //float distCovered = (Time.time - popeStartTime) * moveSpeed * 0.75f;
        //float fractionOfJourney = distCovered / popeJourneyDistance;

        if (boothOpen)
        {
            var priestMovement = agent.GetComponent<PriestMovement>();
            priestMovement.enabled = false;
            float distCovered = (Time.time - popeStartTime) * moveSpeed * 0.75f;
            float fractionOfJourney = distCovered / popeJourneyDistance;


            Pope.transform.position = Vector3.Lerp(popeStartPoint.position, outerBooth.transform.position, fractionOfJourney);
            Vector3 targetPostition = new Vector3(player.transform.position.x, Pope.transform.position.y, player.transform.position.z);
            Pope.transform.LookAt(targetPostition);
        }

        if (isMoving)
        {
            float distCovered = (Time.time - popeStartTime) * moveSpeed * 0.75f;
            float fractionOfJourney = distCovered / popeJourneyDistance;



            Pope.transform.position = Vector3.Lerp(outerBooth.transform.position, popeBooth.transform.position, fractionOfJourney);
            Vector3 targetPostition = new Vector3(window.transform.position.x, Pope.transform.position.y, window.transform.position.z);
            Pope.transform.LookAt(targetPostition);
        }
    }

    public void PlayerWalk()
    {
        d = 17;
        float distCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = distCovered / journeyDistance;


        player.transform.position = Vector3.Lerp(boothOpening.position, booth.transform.position, fractionOfJourney);
        Vector3 targetPostition = new Vector3(window.transform.position.x, player.transform.position.y, window.transform.position.z);
        player.transform.LookAt(targetPostition);
    }

    public void EnterBooth()
    {
        startTime = Time.time;
        journeyDistance = Vector3.Distance(boothOpening.position, booth.transform.position);
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


            if (!dialogueOver)
            {
                smoke.Stop();
            }

        }

        OpenBoothVFX();

    }


    void OpenBoothVFX()
    {
        if (boothOpen == true && !isMoving)
        {
            foreach (GameObject panel in enterPanel)
            {
                panel.SetActive(true);
                if (glowRun == false)
                {
                    StartCoroutine(GlowEffect());
                }
            }
        }
        else
        {
            foreach (GameObject panel in enterPanel)
            {
                panel.SetActive(false);
            }
            glowModel[0].material.SetFloat("_Opacity", 0f);
            glowModel[1].material.SetFloat("_Opacity", 0f);
        }
    }

    void KickOut()
    {
        Vector3 m_NewForce = new Vector3(-100.0f, 0.0f, 0.0f);
        playerRb.AddForce(m_NewForce, ForceMode.Impulse);
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

    public IEnumerator GlowEffect()
    {
        glowModel[0].material.SetFloat("_Opacity", 0.1f * glowTimer);
        glowModel[1].material.SetFloat("_Opacity", 0.1f * glowTimer);

        glowRun = true;
        if (glowTimer == 0)
        {
            glowOn = true;
        }
        if (glowTimer >= 10f)
        {
            glowOn = false;
        }
        if (glowOn == true)
        {
            glowTimer += 1f;
            yield return new WaitForSeconds(0.2f);
        }
        else if (glowOn == false)
        {
            glowTimer -= 1f;
            yield return new WaitForSeconds(0.2f);

        }

        glowRun = false;
    }

    public void GoodOption()
    {

        if (!isConfessing)
        {
            Yes();
        }
        else
        {
            StartCoroutine(SpeechDelay());
            if (d == 13 && confessedMurder)
            {
                dtrack = 2;
            }
            else
            {
                dtrack = 1;
            }


            //Skipping over "and nothig else"
            if (d == 8 || d == 9)
            {
                d = 10;
                gd = 9;
                bd = 9;
            }

            storedSin = sinMeter;
            sinLerpTimer = 0f;
            

            float calcuatedSin = 10 * (1 / sinTimer);

            float roundedSin = Mathf.Round(calcuatedSin);

            float sinClamp = Mathf.Clamp(roundedSin, 1, 10);

            actualSin -= sinClamp;

            gd++;
            bd++;
            d++;

            if (d == 16)
            {

                isAbsolved = true;
                dialogueOver = true;
                if (sinMeter <= 50)
                {
                    LeaveBooth();
                }
            }
        }

    }

    public void BadOption()
    {


        if (!isConfessing)
        {
            No();
        }
        else
        {
            StartCoroutine(SpeechDelay());

            if (d == 10)
            {
                confessedMurder = true;
            }

            if (d == 11 && confessedMurder)
            {
                LeaveBooth();

            }   

            storedSin = sinMeter;
            sinLerpTimer = 0f;

            dtrack++;

            float calcuatedSin = 10 * (1 / sinTimer);

            float roundedSin = Mathf.Round(calcuatedSin);

            float sinClamp = Mathf.Clamp(roundedSin, 1, 10);

            actualSin += sinClamp;

            gd++;
            bd++;
            d++;


            //Check if the next dialogiue option is empty
            if (dialogue3[d] == "")
            {
                dtrack = 2;
            }

            if (dialogue3[d] == "" && dialogue2[d] == "")
            {
                dtrack = 1;
            }

            if (d == 6)
            {
                LeaveBooth();
                dialogueOver = true;
            }

            if (d == 16)
            {
                isAbsolved = true;
                dialogueOver = true;
                if (sinMeter <= 50)
                {
                    LeaveBooth();
                }
            }

        }
    }

    public void LeaveBooth()
    {
        d = 17;
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

        if (!isAbsolved)
        {
            Guilt();
        }

        Audio();

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
        if (!lightChanging)
        {
            StartCoroutine(ChangeLight());
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
        distortionSize = Mathf.Lerp(0, 1, cellTimer);
        distortion.material.SetFloat("_HeatDistortion", distortionSize);

        cells = Mathf.Lerp(1.7f, maxCell, cellTimer);
        boothModel.material.SetFloat("_Cell_size", cells);
        popeBody.material.SetTexture("_Texture2D", popeTex[6]);

        if (cells < maxCell)
        {
            cellTimer += Time.deltaTime / 20;

            Debug.Log(cellTimer);
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

    public IEnumerator ChangeLight()
    {
        lightChanging = true;
        redlight = !redlight;

        if (redlight == true)
        {
            policeLight.color = Color.red;
        }

        if (redlight == false)
        {
            policeLight.color = Color.blue;
        }

        yield return new WaitForSeconds(1f);

        lightChanging = false;

    }

    public void Audio()
    {
        if (dialogueOver == true)
        {
            if (!audioPlaying)
            {

                audio.clip = audioclips[aud];
                if (audio.clip != null)
                {
                    audioPlaying = true;
                    audio.Play();
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

        FireFX();
        PriestEmotions();
    }

    public void PopeBreathing()
    {
        float breathingThreshold = 15f;
        float breathingDistance = Vector3.Distance(player.transform.position, agent.transform.position);

        float breathingVolume = 1.0f / (breathingDistance - 3f);
        if (breathingDistance < breathingThreshold)
        {
            
            if (!isBreathing) 
            {
                popeAudio.clip = popeSounds[0];
                popeAudio.Play();
                isBreathing = true;
            }
            popeAudio.volume = breathingVolume;
        }
        else if (breathingDistance > breathingThreshold)
        {
            popeAudio.Stop();
            isBreathing = false;
        }
    }

    public void FireFX()
    {
        fireScale = 0.02f * sinMeter;
        float redScale = 10 * fireScale;
        float orangeScale = 8 * fireScale;
        float yellowScale = 4 * fireScale;

        redFire.startSize = redScale;
        redFire.startLifetime = redScale;

        orangeFire.startSize = orangeScale;
        orangeFire.startLifetime = orangeScale;

        yellowFire.startSize = yellowScale;
        yellowFire.startLifetime = yellowScale;

    }

    public void PriestEmotions()
    {
        if (!dialogueOver)
        {


            if (sinMeter < 10)
            {
                popeBody.material.SetTexture("_Texture2D", popeTex[0]);
            }

            if (sinMeter > 10 && sinMeter < 25)
            {

                if (confessedMurder == true)
                {
                    popeBody.material.SetTexture("_Texture2D", popeTex[3]);
                }
                else
                {
                    popeBody.material.SetTexture("_Texture2D", popeTex[1]);
                }
            }

            if (sinMeter > 25 && sinMeter < 50)
            {
                if (confessedMurder == true)
                {
                    popeBody.material.SetTexture("_Texture2D", popeTex[4]);
                }
                else
                {
                    popeBody.material.SetTexture("_Texture2D", popeTex[2]);
                }
            }

            if (sinMeter > 50)
            {
                popeBody.material.SetTexture("_Texture2D", popeTex[5]);
            }
        }
    }
}
