using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioandFX : MonoBehaviour
{
    [Header("Scripts")]
    public Cinematic cinematicScript;
    public CinematicDialogue dialogueScript;
    public UI uiScript;
    public FirstPersonMovement movementScript;
    public PriestMovement priestMovementScript;
    public Menus menuScriot;

    [Header("Glow")]
    private float glowTimer;
    public GameObject[] enterPanel;
    public Renderer[] glowModel;
    private bool glowOn;
    private bool glowRun;

    [Header("Fire")]
    public ParticleSystem redFire;
    public ParticleSystem orangeFire;
    public ParticleSystem yellowFire;
    public Light fireLight;

    [Header("PoliceLight")]
    public Light policeLight;
    private bool redlight = true;
    public bool lightChanging;
    public AudioSource endingAudio;
    public AudioClip[] endingClips;
    private bool audioPlaying;
    
    
    [SerializeField] AudioMixer mixer;
    public AudioMixerGroup[] mixerGroups;
    public int mixerInt;
    const string MIXER_DISTORT = "DistortVolume";
    const string MIXER_MASTER = "MasterVolume";
    const string MIXER_NORMAL = "NormalVolume";
    public float volume;

    [Header("PopeFX")]
    public Texture2D[] popeTex;
    public Renderer popeBody;
    public AudioSource popeAudio;
    public AudioClip[] popeSounds;
    private bool isBreathing;

    [Header("WalkSound")]
    public bool isWalking;
    public AudioSource playerAudio;
    public AudioClip[] playerSounds;

    public AudioSource[] audioSources;

    [Header("Exterior")]
    public GameObject churchBells;
    public AudioSource bellAudio;
    public AudioSource rainAudio;
    private bool bellPlaying;
    public AudioSource graveSource;
    public GameObject grave;
    public bool gravePlaying;
    public bool graveSteppedOn;
    public float graveTimer;
    public ParticleSystem rainParticle;
    void Start()
    {
        policeLight.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (menuScriot.openingScene == true)
        {
            Steps();
            BellVolume();
        }
        else
        {
            PopeBreathing();
            Steps();
            FireFX();
            SetAudioVolume();
        }
    }

    public void PriestEmotions()
    {
        //called once the dialogue is finished
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
                if (dialogueScript.confessedMurder == true)
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
                if (dialogueScript.confessedMurder == true)
                {
                    popeBody.material.SetTexture("_Texture2D", popeTex[4]);
                }
                //neutral
                else
                {
                    popeBody.material.SetTexture("_Texture2D", popeTex[2]);
                }
            }
            //angry
            if (uiScript.sinMeter > 50)
            {
                popeBody.material.SetTexture("_Texture2D", popeTex[5]);
            }
        }
    }

    //scale fire in relation to sin
    public void FireFX()
    {
        float fireScale = 0.02f * uiScript.sinMeter;
        float redScale = 10 * fireScale;
        float orangeScale = 8 * fireScale;
        float yellowScale = 4 * fireScale;

        redFire.startSize = redScale;
        redFire.startLifetime = redScale;

        orangeFire.startSize = orangeScale;
        orangeFire.startLifetime = orangeScale;

        yellowFire.startSize = yellowScale;
        yellowFire.startLifetime = yellowScale;

        fireLight.intensity = uiScript.sinMeter;
       
        float gValue =  255 * (uiScript.sinMeter/100);
        var color = new Color32(255,(byte)gValue, 0, 255);

        fireLight.color = color;

    }

    //breathing sounds play, volume relative to distance from priest
    public void PopeBreathing()
    {
        float breathingThreshold = 15f;
        float breathingDistance = Vector3.Distance(movementScript.player.position, cinematicScript.popeHead.transform.position);

        float breathingVolume = 1.0f / (breathingDistance - 3f);
        if (breathingDistance < breathingThreshold)
        {
            //prevents play restarting
            if (!isBreathing)
            {
                popeAudio.clip = popeSounds[0];
                popeAudio.Play();
                isBreathing = true;
            }
            popeAudio.volume = breathingVolume;
        }
        //too quiet to hear so no need to play
        else if (breathingDistance > breathingThreshold)
        {
            popeAudio.Stop();
            isBreathing = false;
        }
    }

    //takes in a int from Ending script to choose correct clip
    public void EndingAudio(int track)
    {
        if (dialogueScript.dialogueOver == true)
        {
            if (!audioPlaying)
            {
                endingAudio.clip = endingClips[track];
                if (endingAudio.clip != null)
                {
                    audioPlaying = true;
                    endingAudio.Play();
                }
            }
        }
    }

    //alternates between light colours every second
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

    //increases glow opacity every x seconds, could be done with delta time but this suits style more
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

    //Only plays FX once the priest has been interacted with
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
            //reset to 0 to prevent white glow in confession
            foreach (GameObject panel in enterPanel)
            {
                panel.SetActive(false);
            }
            glowModel[0].material.SetFloat("_Opacity", 0f);
            glowModel[1].material.SetFloat("_Opacity", 0f);
        }
    }

    //footstep sound effect
    private void Steps()
    {

        if (movementScript.playerRb.velocity.magnitude > 1f)
        {
            if (!isWalking)
            {
                playerAudio.clip = playerSounds[0];
                playerAudio.Play();
                isWalking = true;
            }
        }
        else if (movementScript.playerRb.velocity.magnitude < 1f)
        {
            playerAudio.Stop();
            isWalking = false;

        }
    }

    public void BellVolume()
    {
        float bellDistance = Vector3.Distance(movementScript.player.position, churchBells.transform.position);
        
        float bellVolume = 3f / (bellDistance);
        bellAudio.volume = bellVolume;
        if (!bellPlaying)
        {
            bellAudio.Play();
            bellPlaying = true;
        }
    }

    void SetAudioVolume()
    {
        mixer.SetFloat(MIXER_DISTORT, volume);
        mixer.SetFloat(MIXER_NORMAL, 1- volume);
    }

    public void GraveVolume()
    {
        //float graveDistance = Vector3.Distance(movementScript.player.position, grave.transform.position);

        //float graveVolume = 1f / (graveDistance);
        //graveSource.volume = graveVolume;
        graveSource = grave.GetComponent<AudioSource>();
        if (gravePlaying)
        {
            graveSource.Play();
            gravePlaying = false;
        }
    }
}
