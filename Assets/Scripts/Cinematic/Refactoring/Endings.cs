using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Endings : MonoBehaviour
{
    public CinematicDialogue dialogueScript;
    public Cinematic cinematicScript;
    public AudioandFX FXscript;
    public UI uiScript;

    public GameObject smokeObject;
    public ParticleSystem smoke;
    private float cellTimer;
    private float cells;
    public GameObject distortionPlane;
    public Renderer distortion;
    public GameObject endScreen;
    public TMP_Text endScreenText;
    public bool smokePlaying = false;
    public Renderer boothModel;
    private int aud;
    private bool damnation = false;
    private bool endingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        cells = 1.7f;
        distortionPlane.SetActive(false);
        endScreen.SetActive(false);
        endingCoroutine = false;
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

    public void EndingSequences()
    {

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

        FXscript.policeLight.enabled = true;
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
        damnation = true;
        endScreenText.text = "DAMNATION";
        aud = 4;
        distortionPlane.SetActive(true);

        if (smokePlaying == false)
        {
            smokePlaying = true;
            smoke.Play(smokePlaying);
        }

        float maxCell = 5f;
        float distortionSize = Mathf.Lerp(0, 1, cellTimer);
        distortion.material.SetFloat("_HeatDistortion", distortionSize);

        cells = Mathf.Lerp(1.7f, maxCell, cellTimer);
        boothModel.material.SetFloat("_Cell_size", cells);

        FXscript.popeBody.material.SetTexture("_Texture2D", FXscript.popeTex[6]);

        if (cells < maxCell)
        {
            cellTimer += Time.deltaTime / 20;
        }

        if (cells == maxCell)
        {
            CutToBlack();
        }

    }

    public IEnumerator EndScreen()
    {
        yield return new WaitForSeconds(5f);
        endScreen.SetActive(true);
    }

    public void CutToBlack()
    {
        if (!endingCoroutine)
        {
            endingCoroutine = true;
            StartCoroutine(EndScreen());
        }
    }
}
