using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCam : MonoBehaviour
{
    [Header("Scripts")]
    public Cinematic cinematic;
    public UI uiScript;
    public FirstPersonMovement movementScript;
    public Menus menuScript;

    [Header("MouseSens")]
    public float sensX;
    public float sensY;

    public Slider sensSlider;

    [Header("Rotation")]
    private Transform orientation;
    float xRotation;
    float yRotation;


    // Start is called before the first frame update
    void Start()
    {
        orientation = movementScript.player;
        sensSlider.value = PlayerPrefs.GetFloat("sensitivity");
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
            float mouseX = Input.GetAxisRaw("MouseX") * Time.deltaTime * sensX * sensSlider.value;
            float mouseY = Input.GetAxisRaw("MouseY") * Time.deltaTime * sensY * sensSlider.value;

            yRotation += mouseX;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (!menuScript.menuDisabled || !menuScript.notPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }



    }   

    public void SetSens()
    {
        PlayerPrefs.SetFloat("sensitivity", sensSlider.value);
        PlayerPrefs.Save();
    }

}
