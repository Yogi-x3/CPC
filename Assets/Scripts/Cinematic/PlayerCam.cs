using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public Cinematic cinematic;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (cinematic.cinematicMode && !Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (!cinematic.cinematicMode || Input.GetMouseButton(1))
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
