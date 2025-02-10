using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDetector : MonoBehaviour
{
    public GameObject player;
    public GameObject[] CopperPipes;
    public AudioSource beep;
    public float pipeDistance;
    private bool isBeeping = false;
    public PipePuzzle pipePuzzle;
    public bool detector = false;
       
    // Start is called before the first frame update
    void Start()
    {
        CopperPipes = GameObject.FindGameObjectsWithTag("Copper Pipe");
    }

    // Update is called once per frame
    void Update()
    {
        PipeDetection();
        if (pipeDistance < 8 && pipePuzzle.inPuzzle == !true && detector == true)
        {
            BeepManager();
        }
        else
        {
            //copperRing.Stop();
        }
    }

    void PipeDetection()
    {
        float closestDistance = Mathf.Infinity;

        foreach (GameObject pipe in CopperPipes)
        {
            float distanceToPipe = Vector3.Distance(player.transform.position, pipe.transform.position);
            if (distanceToPipe < closestDistance)
            {
                closestDistance = distanceToPipe;
            }
        }

        pipeDistance = closestDistance;
    }

    public void BeepManager()
    {
        if (isBeeping == false) 
        {
            StartCoroutine(Beeping());
        }
        //copperRing.Play();
    }
    
    public IEnumerator Beeping()
    {
        isBeeping = true;
        beep.volume = 1/pipeDistance;
        beep.Play();
        yield return new WaitForSeconds(pipeDistance/3);
        isBeeping = false;
    }

}
