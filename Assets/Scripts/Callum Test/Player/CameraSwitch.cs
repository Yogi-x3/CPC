using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject topDownCamera;
    public GameObject firstPersonCamera;
    public int Manager;
    public Movement movement;


    // Start is called before the first frame update
    void Start()
    {
        Cam1();   
    }

    void Update()
    {
        ManageCamera();
    }

    private void Cam1()
    {
        topDownCamera.SetActive(true);
        topDownCamera.tag = "MainCamera";
        firstPersonCamera.SetActive(false);
        firstPersonCamera.tag = "OnHold";
    }

    private void Cam2()
    {
        topDownCamera.SetActive(false);
        topDownCamera.tag = "OnHold";
        firstPersonCamera.SetActive(true);
        firstPersonCamera.tag = "MainCamera";
    }

    public void ManageCamera()
    {
        if (movement.camMode == true)
        {
            Cam2 ();
            //Manager = 1;
        }
        else
        {
            Cam1 ();
            //Manager = 0;
        }
    }
}
