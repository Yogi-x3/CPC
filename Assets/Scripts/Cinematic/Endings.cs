using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Endings : MonoBehaviour
{
    [Header("Scripts")]
    public CinematicDialogue dialogueScript;
    public Cinematic cinematicScript;
    public AudioandFX FXscript;
    public UI uiScript;
    public PostFX postScript;
    public FirstPersonMovement movementScript;

    [Header("Smoke")]
    public GameObject smokeObject;
    public ParticleSystem smoke;
    public bool smokePlaying = false;

    [Header("HeatDistortion")]
    public GameObject distortionPlane;
    public Renderer distortion;
    private float cellTimer;
    private float cells = 1.7f;

    [Header("EndScreen")]
    public GameObject endScreen;
    public TMP_Text endScreenText;
    public Renderer boothModel;
    private int aud;
    public Collider endingdCollider;
    public Renderer endingPlane;
    public GameObject glitterParticle;
    public Image jumpScare;
    private float jumpScareTimer;
    // Start is called before the first frame update
    void Start()
    {
        distortionPlane.SetActive(false);
        endScreen.SetActive(false);
        endingdCollider.isTrigger = false;
        endingPlane.material.SetColor("_Color", Color.black);
        endingPlane.material.SetColor("_Emmision", Color.black);
        glitterParticle.SetActive(false);
        jumpScare.color = new Color(jumpScare.color.r, jumpScare.color.g, jumpScare.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueScript.dialogueOver == true)
        {
            EndingSequences();
        } else
        {
            smokeObject = GameObject.FindGameObjectWithTag("Smoke");
            smoke = smokeObject.GetComponent<ParticleSystem>();
            smoke.Stop();
        }




    }
    //calculate ending to play based on Sin levels
    public void EndingSequences()
    {
        endingPlane.material.SetColor("_Color", Color.white);
        endingPlane.material.SetColor("_Emmision", Color.white);
        endingdCollider.isTrigger = true;
        FXscript.policeLight.enabled = true;
        
        dialogueScript.waitForSpeech = true;
        if (dialogueScript.isAbsolved == true)
        {
            if (uiScript.actualSin >= 50f)
            {
                Damnation();
            }

            if (uiScript.sinMeter <= 0)
            {
                Absolved();
            }
            //different endings depending on murder confession
            if (0f < uiScript.actualSin && uiScript.actualSin < 50f)
            {
                if (dialogueScript.confessedMurder)
                {
                    Police();
                }
                else
                {
                    Neutral();
                }
            }
        }
        //only absolved if you reach the end of the dialogue
        else
        {
            Guilt();
        }
        FXscript.EndingAudio(aud);
    }

    public void Absolved()
    {
        postScript.Absolved();
        FXscript.policeLight.color = Color.white;
        glitterParticle.SetActive(true);
        endScreenText.text = "ABSOLVED";
        aud = 0;
        if (Input.GetKeyDown(KeyCode.W))
        {
            cinematicScript.KickOut();
        }

    }

    public void Police()
    {
        endScreenText.text = "POLICE";
        if (!FXscript.lightChanging)
        {
            StartCoroutine(FXscript.ChangeLight());
        }
        aud = 1;

    }

    public void Neutral()
    {
        FXscript.policeLight.color = Color.white;
        endScreenText.text = "NEUTRAL";
        aud = 2;

    }

    public void Guilt()
    {
        movementScript.moveSpeed = 5f;
        FXscript.policeLight.color = Color.white;
        postScript.Guilt();
        endScreenText.text = "GUILT";
        aud = 3;


    }

    //enable heat disotortion and increase distortion and cell size over time, until a max point
    void Damnation()
    {
        float maxJumpscareTime = 1;
        if (jumpScareTimer < maxJumpscareTime)
        {
            jumpScareTimer += Time.deltaTime;
        }
        jumpScare.color = new Color(jumpScare.color.r, jumpScare.color.g, jumpScare.color.b, 1 - jumpScareTimer);
        endScreenText.text = "DAMNATION";
        aud = 4;
        distortionPlane.SetActive(true);
        smokeObject.SetActive(false);
        //prevents smoke from restarting
        if (smokePlaying == false)
        {
            smokePlaying = true;
            smoke.Play(smokePlaying);
        }
        float startCell = 1.7f;
        float maxCell = 5f;
        float distortionSize = Mathf.Lerp(0, 1, cellTimer);
        distortion.material.SetFloat("_HeatDistortion", distortionSize);

        cells = Mathf.Lerp(startCell, maxCell, cellTimer);
        boothModel.material.SetFloat("_Cell_size", cells);

        FXscript.popeBody.material.SetTexture("_Texture2D", FXscript.popeTex[6]);

        if (cells < maxCell)
        {
            cellTimer += Time.deltaTime / 20;
        }
        //once at max effect bring end screem
        if (cells == maxCell)
        {
            endScreen.SetActive(true);
        }

    }
    //delay to end screen to allow player time to process
    public void EndScreen()
    {
        endScreen.SetActive(true);
        uiScript.cinematicMode = true;
    }

}
