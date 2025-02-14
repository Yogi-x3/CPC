using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cinematic : MonoBehaviour
{
    [Header("Scripts")]
    public AudioandFX FXscript;
    public CinematicDialogue dialogueScript;
    public FirstPersonMovement movementScript;
    public Endings endingScript;
    public UI uiScript;
    public PriestMovement priestMovementScript;

    [Header("Booth")]
    public GameObject curtain;
    public Transform[] Openings;
    public Transform boothOpening;
    public GameObject window;
    public GameObject booth;

    [Header("PlayerMovement")]
    private float startTime;
    private float journeyDistance;

    [Header("Checks")]
    public bool isConfessing;
    public bool boothOpen;
    public bool canEnter;
    private bool priestMoving = false;

    [Header("PopeMovement")]
    public GameObject Pope;
    public Transform centreBooth;
    public Transform outerBooth;
    private float popeStartTime;
    private float popeJourneyDistance;
    public Transform popeStartPoint;


    public delegate void TestDelegate();
    public TestDelegate methodToCall;
    public bool delegateCoroutineRunning;

    // Start is called before the first frame update
    void Start()
    {
        window = GameObject.FindGameObjectWithTag("Window");
        booth = GameObject.FindGameObjectWithTag("Slot");
    }

    // Update is called once per frame
    void Update()
    {

        uiScript.CinematicMode();

        if (canEnter == true)
        {
            PlayerWalk();
            Confessing();
        }

        ClosestBooth();

        dialogueScript.DialogueManager();
        
        //allows priest to walk freely until player agrees
        if (priestMoving == true)
        {
            PopeWalk();
        }
    

        uiScript.SinBars();
        
    }
    //diables confessing and player lerp once in position
    public void Confessing()
    {
        if (movementScript.player.position == booth.transform.position)
        {
            dialogueScript.waitForSpeech = false;
            isConfessing = true;
            canEnter = false;
            dialogueScript.d = 2;
        }
    }

    public void PopeWalk()
    {
        //disabled pope roaming
        priestMovementScript.enabled = false;

        float popeMoveSpeed = movementScript.moveSpeed * 0.25f;
        float distCovered = (Time.time - popeStartTime) * popeMoveSpeed;
        float fractionOfJourney = distCovered / popeJourneyDistance;

        Vector3 targetPostition = new Vector3(movementScript.player.position.x, Pope.transform.position.y, movementScript.player.position.z);
        Pope.transform.LookAt(targetPostition);
        //to front of booth
        if (boothOpen)
        {
            var targetLocation = outerBooth.position;
            Pope.transform.position = Vector3.Lerp(popeStartPoint.position, targetLocation, fractionOfJourney);

        }
        //inside booth
        if (priestMoving && !boothOpen)
        {
            var targetLocation = centreBooth.position;
            Pope.transform.position = Vector3.Lerp(outerBooth.position, targetLocation, fractionOfJourney);
        }
    }
    //player lerped into booth
    public void PlayerWalk()
    {
        dialogueScript.d = 17;
        float automaticMoveSpeed = movementScript.moveSpeed / 3;
        float distCovered = (Time.time - startTime) * automaticMoveSpeed;
        float fractionOfJourney = distCovered / journeyDistance;


        movementScript.player.position = Vector3.Lerp(boothOpening.position, booth.transform.position, fractionOfJourney);
        Vector3 targetPostition = new Vector3(window.transform.position.x, movementScript.player.position.y, window.transform.position.z);
        movementScript.player.LookAt(targetPostition);
    }
    //calculates lerp. disables boothOpen so not continually triggered
    public void EnterBooth()
    {
        startTime = Time.time;
        journeyDistance = Vector3.Distance(boothOpening.position, booth.transform.position);
        boothOpen = false;
    }
    //Disables booth player is farthest from, showing enter sign and diasbling uneeded FX e,g smoke
    public void ClosestBooth()
    {
        if (canEnter != true)
        {
            float distanceTobooth1 = Vector3.Distance(movementScript.player.transform.position, Openings[0].transform.position);
            float distanceTobooth2 = Vector3.Distance(movementScript.player.transform.position, Openings[1].transform.position);

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
            uiScript.curtain = GameObject.FindGameObjectWithTag("Curtain");
            uiScript.curtainAnimator = uiScript.curtain.GetComponent<Animator>();
        }
        //Play FX to signify player can enter
        FXscript.OpenBoothVFX();

    }

    //forces player from booth
    void KickOut()
    {
        int blankDialogue = 17;
        dialogueScript.d = blankDialogue;
        Vector3 kickOutForce = new Vector3(-100.0f, 0.0f, 0.0f);
        movementScript.playerRb.AddForce(kickOutForce, ForceMode.Impulse);
        uiScript.cinematicMode = false;
    }


    //prepares dialogue for when re enabled, and starts pope journey
    public void BoothOpen()
    {
        uiScript.popeAnimation.SetBool("Yes", false);
        uiScript.cinematicMode = false;
        dialogueScript.gd++;
        dialogueScript.bd++;
        boothOpen = true;
        priestMoving = true;
        
        popeStartTime = Time.time;
        popeJourneyDistance = Vector3.Distance(popeStartPoint.position, outerBooth.transform.position);

    }

    //Master coroutine
    public IEnumerator Delay(TestDelegate method, float time)
    {
        delegateCoroutineRunning = true;
        yield return new WaitForSeconds(time);
        method();
        delegateCoroutineRunning = false;
    }
    //isconfessing opens curtain, kicks player after delay
    public void LeaveBooth()
    {
        isConfessing = false;
        if (!delegateCoroutineRunning)
        {
            StartCoroutine(Delay(KickOut, 3f));
        }
        dialogueScript.dialogueHolder.SetActive(false);
    }

}
