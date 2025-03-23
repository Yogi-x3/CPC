using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Triggering : MonoBehaviour
{
    [Header("Scripts")]
    public Cinematic cinematicScript;
    public UI uiScript;
    public CinematicDialogue dialogueScript;
    public Endings endings;
    public Menus menuScript;
    public AudioandFX FXscript;

    public List<GameObject> graveList;
    public GameObject desecratorText;
    private int desecratorInt;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoothCollider") && cinematicScript.boothOpen == true && !dialogueScript.dialogueOver)
        {
            cinematicScript.canEnter = true;
            uiScript.cinematicMode = true;
            cinematicScript.EnterBooth();
        }

        if (other.CompareTag("LevelEnd") && dialogueScript.dialogueOver)
        {
            endings.EndScreen();   
        }

        if (other.CompareTag("EnterChurch"))
        {
            menuScript.isLoading = true;
        }

        if (other.CompareTag("Grave"))
        {
            FXscript.graveSteppedOn = true;
            if (FXscript.grave != other.gameObject)
            {
                FXscript.grave = other.gameObject;
                FXscript.graveSource = FXscript.grave.GetComponent<AudioSource>();
                FXscript.gravePlaying = true;
                FXscript.GraveVolume();
            }
        }
        AddOrRemove(other.gameObject);

    }

    public void AddOrRemove(GameObject grave)
    {
        if (graveList.Contains(grave))
        {

        }
        else
        {
            graveList.Add(grave);
        }

        if (graveList.Count == 8)
        {
            menuScript.desecratorInt = 1;
            PlayerPrefs.SetInt("desecrator", menuScript.desecratorInt);
            PlayerPrefs.Save();
            menuScript.Quit();
        }
    }
}
