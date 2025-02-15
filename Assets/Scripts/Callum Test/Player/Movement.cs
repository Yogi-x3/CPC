using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float speed;
    private Vector2 move;

    private Quaternion targetRotation;
    public float rotationSpeed;

    public Camera cam;
    Vector3 mousePos;

    public bool camMode;
    public Transform mirrorHolder;
    public GameObject mirror;
    public GameObject mirrorObject;
    public CameraSwitch cameraSwitch;
    public bool mirrorMode = false;

    public Transform player;
    public float mouseSensitivity;
    float cameraVerticalRotation = 0f;
    float cameraHorizontalRotation = 0f;
    float mirrorRotation = 0f;
    bool lockedCursor = true;

    public ScoreManager scoreManager;
    public FloatSO scoreSO;
    public bool playerKilled = false;

    public Vents vents;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2> (); 
    }
    
    // Start is called before the first frame update
    void Start()
    {

        speed = scoreSO.PlayerSpeed;
        scoreManager.UpdateSpeed(speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerKilled != true)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && vents.inVent == false)
            {
                //cameraSwitch.ManageCamera();
                camMode = !camMode;
            }

            if (camMode == false)
            {
                MovePlayer();
                ControlMouse();
            }
            else if (mirrorMode == false)
            {
                FirstPersonCamera();
            }

            if (Input.GetMouseButton(1) && camMode == true)
            {
                MirrorControl();
            }
            else
            {
                mirrorMode = false;
            }
        } else
        
        speed = scoreSO.PlayerSpeed;

        mirrorObject.SetActive(mirrorMode);
        cam = Camera.main;
        scoreManager.UpdateSpeed(speed);
    }

    public void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    void ControlMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        rotationSpeed = 2000f;
        Vector3 mousePos = Input.mousePosition;
        mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x,mousePos.y, cam.transform.position.y - transform.position.y));
        targetRotation = Quaternion.LookRotation(mousePos- new Vector3(transform.position.x, 0, transform.position.z));
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

    }

    void FirstPersonCamera()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        rotationSpeed = 10f;
        float inputX = Input.GetAxis("Mouse X") * rotationSpeed;
        float inputY = Input.GetAxis("Mouse Y") * rotationSpeed;

        cameraVerticalRotation = 0f;

        player.Rotate(Vector3.up * inputX);
    }



    void MirrorControl()
    {
        mirrorMode = true;
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        mirrorRotation -= inputX;
        mirrorRotation = Mathf.Clamp(mirrorRotation, -50f, 50f);
        mirrorHolder.transform.localEulerAngles = Vector3.up * mirrorRotation;
    }




}
