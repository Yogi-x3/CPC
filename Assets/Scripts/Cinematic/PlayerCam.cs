using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Scripts")]
    public Cinematic cinematic;
    public UI uiScript;
    public FirstPersonMovement movementScript;

    [Header("MouseSens")]
    public float sensX;
    public float sensY;

    [Header("Rotation")]
    private Transform orientation;
    float xRotation;
    float yRotation;


    // Start is called before the first frame update
    void Start()
    {
        orientation = movementScript.player;
}

    // Update is called once per frame
    void Update()
    {
        //allow player to look around with right click
        if (uiScript.cinematicMode && !Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (!uiScript.cinematicMode || Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxisRaw("MouseX") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("MouseY") * Time.deltaTime * sensY;

            yRotation += mouseX;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
