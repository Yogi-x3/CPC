using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Scripts")]
    public Cinematic cinematicScript;
    public UI uiScript;

    [Header("Movement")]
    public Transform player;
    public Rigidbody playerRb;
    Vector3 movementDir;
    public float moveSpeed;
    public float groundDrag;
    float horizontalInput;
    float verticalInput;

    [Header("WalkSound")]
    public AudioSource playerAudio;
    public AudioClip[] playerSounds;
    private bool isWalking;

    
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        //cant move while conversing
        if (uiScript.cinematicMode == false)
        {
            MyInput();
            SpeedControl();
            playerRb.drag = groundDrag;

        }

        //Steps();

    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        movementDir = player.forward *verticalInput + player.right * horizontalInput;

        playerRb.AddForce(movementDir.normalized * moveSpeed * 10f, ForceMode.Force);

    }
    //prevents player surpassing max speed
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            playerRb.velocity = new Vector3(limitedVel.x, playerRb.velocity.y, limitedVel.z);
        }



    }
    //footstep sound effect
    private void Steps()
    {

        if (playerRb.velocity.magnitude > 1f)
        {
            if (!isWalking)
            {
                playerAudio.clip = playerSounds[0];
                playerAudio.Play();
                isWalking = true;
            }
        }
        else if (playerRb.velocity.magnitude < 1f)
        {
            playerAudio.Stop();
            isWalking = false;

        }
    }
}
