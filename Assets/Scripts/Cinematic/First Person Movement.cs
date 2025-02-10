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
        MyInput();
        SpeedControl();

        rb.drag = groundDrag;

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

        //if (verticalInput != 0 || horizontalInput != 0)
        //{
        //    camAnim.SetBool("IsWalking", true);
        //}
        //else
        //{
        //    camAnim.SetBool("IsWalking", false);
        //}
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
