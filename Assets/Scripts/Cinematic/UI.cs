using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("Scripts")]
    public AudioandFX FXscript;
    public CinematicDialogue dialogueScript;
    public FirstPersonMovement movementScript;
    public Endings endingScript;
    public Cinematic cinematicScript;
    public PriestMovement priestMovementScript;
    public PostFX postScript;

    [Header("Cinematics")]
    public Camera cam;
    public bool cinematicMode;
    public GameObject priestDialogueHolder;
    private float FOVtimer;
    public Animator blackBars;
    public GameObject curtain;
    public Animator curtainAnimator;
    public Animator popeAnimation;
    private bool newIdle;
    public AnimationClip[] clips;
    public Transform popeGrate;

    [Header("Sin")]
    public float sinMeter;
    public float actualSin;
    public float storedSin;
    public float sinLerpTimer;
    public Image sinBar;
    public GameObject sinBarHolder;
    public GameObject sinTimeBar;
    public Image sinTimeBarLight;
    public float sinTimer;
    public Image clockHand;
    // Start is called before the first frame update
    void Start()
    {
        sinMeter = 0f;
        curtain = GameObject.FindGameObjectWithTag("Curtain");
        curtainAnimator = curtain.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CinematicMode();

        SinBars();


    }

    public void CinematicMode()
    {
        //interact with Priest
        RaycastHit hit;
        float rayDistance = 100f;

        Debug.DrawRay(cam.transform.position, cam.transform.forward * rayDistance, Color.red);
        //cant be interacted past Yes
        if (cinematicScript.boothOpen == false)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rayDistance))
            {

                if (hit.collider.CompareTag("Pope"))
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        cinematicMode = true;
                    }
                }
            }
        }

        //displaying text if FOV is 80
        if (cam.fieldOfView != 80)
        {
            dialogueScript.dialogueHolder.SetActive(false);
            priestDialogueHolder.SetActive(false);
        }
        else if (!dialogueScript.waitForSpeech)
        {
            dialogueScript.dialogueHolder.SetActive(true);
            priestDialogueHolder.SetActive(true);
        }



        if (cinematicMode)
        {
            //priest looks at player
            if (!cinematicScript.isConfessing)
            {
                Vector3 targetPostition = new Vector3(cam.transform.position.x, cinematicScript.Pope.transform.position.y, cam.transform.position.z);
                cinematicScript.Pope.transform.LookAt(targetPostition);
            } else
            {
                Vector3 targetPostition = new Vector3(popeGrate.position.x, cinematicScript.Pope.transform.position.y, popeGrate.transform.position.z);
                cinematicScript.Pope.transform.LookAt(targetPostition);
            }

            //set player minimum distance from priest
            float cinematicDistance = 7f;

            if (Vector3.Distance(movementScript.player.transform.position, priestMovementScript.agentObject.transform.position) < cinematicDistance)
            {
                movementScript.player.transform.position = movementScript.player.transform.position + (cinematicScript.Pope.transform.forward);
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
        //bars appear in cinematic mode, and movement disabled
        cam.fieldOfView = Mathf.Lerp(60, 80, FOVtimer);
        postScript.DOFDistance = Mathf.Lerp(20, 3, FOVtimer);
        postScript.paniniDistance = Mathf.Lerp(0, 1, FOVtimer);
        blackBars.SetBool("isCinematic", cinematicMode);
        curtainAnimator.SetBool("InBooth", cinematicScript.isConfessing);
        popeAnimation.SetBool("isWalking", !cinematicMode);
        popeAnimation.SetBool("isConfessing", cinematicScript.isConfessing);
        movementScript.enabled = !cinematicMode;

    }

    public void SinBars()
    {
        //cant change sin before confessing so uneeded to run
        if (cinematicScript.isConfessing)
        {
            sinTimeBar.SetActive(true);
            sinBarHolder.SetActive(true);
            FXscript.fireLight.enabled = true;
            sinBar.fillAmount = sinMeter / 80;
            FXscript.FireFX();
            StartCoroutine(IdleAnimation());

            //counts down timer while player is answering
            if (!dialogueScript.waitForSpeech)
            {
                sinTimer += Time.deltaTime;
                float clampedTime = Mathf.Clamp(sinTimer, 0.0f, 10.0f);
                //sinTimeBar.enabled = true;

                sinTimeBarLight.fillAmount = 1 - clampedTime / 10;
                float angle = (10 - clampedTime) * 36;
                clockHand.transform.rotation = Quaternion.Euler(new Vector3(clockHand.transform.rotation.x, clockHand.transform.rotation.y, angle));
            }
            else
            {
                sinTimer = 0.0f;
                sinTimeBar.SetActive(false);
            }

            //dont allow sin to drop below 0
            if (actualSin < 0.0f)
            {
                actualSin = 0.0f;
            }
            //cant lerp above 1 so no need to continue
            if (sinLerpTimer < 1f)
            {
                sinLerpTimer += Time.deltaTime * 0.5f;
                sinMeter = Mathf.Lerp(storedSin, actualSin, sinLerpTimer);
            }
        }
        else
        {
            FXscript.fireLight.enabled = false;
            sinTimeBar.SetActive(false);
            sinBarHolder.SetActive(false);
        }
        FXscript.PriestEmotions();
    }

    public IEnumerator IdleAnimation()
    {
        if (newIdle == false)
        {
            newIdle = true;
            int idle = Random.Range(0, 3);
            popeAnimation.SetInteger("Idle int", idle);
            popeAnimation.SetBool("newIdle", true);

            float animationTime = clips[idle].length;
            yield return new WaitForSeconds(animationTime);
            
            popeAnimation.SetBool("newIdle", false);
            float idleTimer = Random.Range(3, 7);
            yield return new WaitForSeconds(idleTimer);
            newIdle = false;
        }
    }
}
