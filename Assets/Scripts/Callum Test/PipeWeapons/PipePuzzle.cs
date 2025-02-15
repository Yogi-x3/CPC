using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipePuzzle : MonoBehaviour
{
    private PipeDetector pipeDetector;
    private Movement movement;
    public GameObject[] Panels;
    public GameObject player;
    public float interactionDistance = 5f;
    private GameObject activePanel;
    public float unscrewvalue;
    private bool unscrewing = false;
    private bool coroutineRunning = false;
    public bool leftvoid;
    public bool rightvoid;
    public bool inPuzzle;

    
    public Image progressFill;
    public Image LCtrl;
    public Image RCtrl;
    // Start is called before the first frame update
    void Start()
    {
        pipeDetector = player.gameObject.GetComponent<PipeDetector>();
        movement = player.gameObject.GetComponent<Movement>();
        Panels = GameObject.FindGameObjectsWithTag("Panel");
        LCtrl.enabled = false;
        RCtrl.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (pipeDetector.pipeDistance < interactionDistance && movement.camMode == true)
        {
            CheckPanelHover();
        }
        if (unscrewing == true && unscrewvalue < 50)
        {
            CoroutineMonitor();
            Spam();
        }

        if (unscrewvalue == 50)
        {
            RemovePanel(activePanel);
        }

        ProgressBar();
    }


    void CoroutineMonitor()
    {
        if (coroutineRunning == !true)
        {
            StartCoroutine(PanelUnscrewing());
        }
    }

    void RemovePanel(GameObject panel)
    {
        // Example action: Deactivate the panel
        panel.SetActive(false);
        Debug.Log(panel.name + " has been unscrewed and deactivated!");
        unscrewvalue = 0;
        unscrewing = false;
        progressFill.enabled = false;
        LCtrl.enabled = false;
        RCtrl.enabled = false;
        inPuzzle = false;

        // You can perform additional actions on 'panel' here
        // For instance, you could start the pipe puzzle or trigger animations, etc.
    }

    void PipePuzzzle()
    {

    }

    void CheckPanelHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Panel"))
            {
                if (Input.GetMouseButtonDown(0)) // Left mouse button click
                {
                    activePanel = hit.collider.gameObject;
                    unscrewing = true;
                    unscrewvalue = 0;
                    coroutineRunning = false;
                    progressFill.enabled = true;
                    LCtrl.enabled = true;
                    RCtrl.enabled = true;
                    inPuzzle = true;
                }
            }
        }
    }

    public IEnumerator PanelUnscrewing()
    {
        coroutineRunning = true;
        Debug.Log(unscrewvalue);
        if (unscrewvalue > 0)
        {
            unscrewvalue -= 1;
        }
        yield return new WaitForSeconds(0.25f);
        coroutineRunning = false;
    }

    public void Spam()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && leftvoid == false)
        {
            unscrewvalue += 1;
            leftvoid = true;
            rightvoid = false;
            LCtrl.enabled = false;
            RCtrl.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.RightControl) && rightvoid == false)
        {
            unscrewvalue += 1;
            rightvoid = true;
            leftvoid = false;
            RCtrl.enabled = false;
            LCtrl.enabled = true;
        }

        progressFill.enabled = true;

    }


    public void ProgressBar()
    {

        progressFill.fillAmount = unscrewvalue/50;
    }

}
