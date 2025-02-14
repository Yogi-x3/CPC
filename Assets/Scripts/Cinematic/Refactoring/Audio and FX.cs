using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioandFX : MonoBehaviour
{
    public Cinematic cinematicScript;
    public CinematicDialogue dialogueScript;
    public UI uiScript;

    private float glowTimer;
    public ParticleSystem redFire;
    public ParticleSystem orangeFire;
    public ParticleSystem yellowFire;
    private float fireScale;

    public Light policeLight;
    private bool redlight = true;
    public bool lightChanging;
    public AudioSource audio;
    public AudioClip[] audioclips;
    private bool audioPlaying;

    public GameObject[] enterPanel;
    public Renderer[] glowModel;
    public GameObject player;

    private bool glowOn;
    private bool glowRun;



    private bool confessedMurder;

    public Texture2D[] popeTex;
    public Renderer popeBody;
    public GameObject agent;
    public AudioSource popeAudio;
    public AudioClip[] popeSounds;
    private bool isBreathing;

    void Start()
    {
        policeLight.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        PopeBreathing();
    }

    public void PriestEmotions()
    {
        //so dmanation can ovveride all
        if (!dialogueScript.dialogueOver)
        {

            //happy
            if (uiScript.sinMeter < 10)
            {
                popeBody.material.SetTexture("_Texture2D", popeTex[0]);
            }

            if (uiScript.sinMeter > 10 && uiScript.sinMeter < 25)
            {
                //uneasy
                if (confessedMurder == true)
                {
                    popeBody.material.SetTexture("_Texture2D", popeTex[3]);
                }
                //grimace
                else
                {
                    popeBody.material.SetTexture("_Texture2D", popeTex[1]);
                }
            }

            if (uiScript.sinMeter > 25 && uiScript.sinMeter < 50)
            {
                //worried
                if (confessedMurder == true)
                {
                    popeBody.material.SetTexture("_Texture2D", popeTex[4]);
                }
                //neutral
                else
                {
                    popeBody.material.SetTexture("_Texture2D", popeTex[2]);
                }
            }
            //anggry
            if (uiScript.sinMeter > 50)
            {
                popeBody.material.SetTexture("_Texture2D", popeTex[5]);
            }
        }
    }

    public void FireFX()
    {
        fireScale = 0.02f * uiScript.sinMeter;
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

    public void EndingAudio(int track)
    {
        if (dialogueScript.dialogueOver == true)
        {
            if (!audioPlaying)
            {
                audio.clip = audioclips[track];
                if (audio.clip != null)
                {
                    audioPlaying = true;
                    audio.Play();
                }
            }
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

    public IEnumerator GlowEffect()
    {
        glowModel[0].material.SetFloat("_Opacity", 0.1f * glowTimer);
        glowModel[1].material.SetFloat("_Opacity", 0.1f * glowTimer);

        float delay = 0.2f;
        float maxGlow = 10f;
        glowRun = true;
        if (glowTimer == 0)
        {
            glowOn = true;
        }
        if (glowTimer >= maxGlow)
        {
            glowOn = false;
        }

        if (glowOn == true)
        {
            glowTimer += 1f;
        }
        else
        {
            glowTimer -= 1f;
        }

        yield return new WaitForSeconds(delay);
        glowRun = false;
    }

    public void OpenBoothVFX()
    {
        if (cinematicScript.boothOpen == true)
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
}
