using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public Transform orientation;

    public float groundDrag;

    float horizontalInput;
    float verticalInput;

    public AudioSource playerAudio;
    public AudioClip[] playerSounds;
    public bool isWalking;
    public Cinematic cinematicScript;
    public Animator camAnim;


    Vector3 movementDir;

    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (cinematicScript.cinematicMode == false)
        {
            MyInput();

            rb.drag = groundDrag;

        }
        
        SpeedControl();

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
        movementDir = orientation.forward *verticalInput + orientation.right * horizontalInput;

        rb.AddForce(movementDir.normalized * moveSpeed * 10f, ForceMode.Force);

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        if (cinematicScript.cinematicMode)
        {
            rb.velocity = new Vector3(0, 0, 0);
        }


        if(rb.velocity.magnitude > 1f)
        {
            if (!isWalking)
            {
                playerAudio.clip = playerSounds[0];
                playerAudio.Play();
                isWalking = true;
            }
        }
        else if (rb.velocity.magnitude < 1f)
        {
            playerAudio.Stop();
            isWalking = false;

        }
    }
}
