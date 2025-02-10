using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vents : MonoBehaviour
{
    [Header("Vent 1")]
    public GameObject[] vent1;
    public GameObject ventEntry;
    public GameObject ventExit;
    private GameObject player;
    public bool inVent;
    public Movement movement;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inVent = false;
    }

    // Update is called once per frame
    void Update()
    {

        Hide();
        SwitchVents();
    }

    void Hide()
    {
        RaycastHit hit;
        Debug.DrawRay(player.transform.position, player.transform.forward *4f, Color.red);
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, 4f))
            {
                if (hit.collider.CompareTag("Vent"))
                {
                    if (Input.GetKeyDown(KeyCode.L))
                    {
                    GameObject interactedVent = hit.collider.gameObject;
                    ventEntry = interactedVent;

                    // Determine the exit vent
                    int entryIndex = System.Array.IndexOf(vent1, ventEntry);
                    if (entryIndex >= 0) // Valid vent found in array
                    {
                        int exitIndex = 1 - entryIndex; // Flip index (0 -> 1, 1 -> 0)
                        ventExit = vent1[exitIndex];
                    }
                    if (inVent == false)
                    {
                        movement.camMode = true;
                        inVent = true;
                        player.transform.position = interactedVent.transform.position;
                    }
                    else    
                    {
                        movement.camMode = false;
                        inVent = false;
                        player.transform.position = interactedVent.transform.position + new Vector3(4,0,0);
                    }
                    }
                }
            }
        Debug.DrawRay(player.transform.position, player.transform.forward * 1f, Color.red);
    }

    void SwitchVents()
    {
        if (inVent && Input.GetKeyDown(KeyCode.K))
        {
            player.transform.position = ventExit.transform.position;
        }
    }
}
