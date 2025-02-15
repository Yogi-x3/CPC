using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClick : MonoBehaviour
{
    public Upgrades upgrades;
    public DialogueTrigger dialogueTrigger;
    public GameObject insufficientFunds;

    void OnMouseDown()
    {
        if (this.gameObject.name == ("CameraUpgrade")){
            {
                upgrades.CamUpgrade();

                if (upgrades.canPurchase == true)
                {
                    dialogueTrigger = this.gameObject.GetComponent<DialogueTrigger>();
                    dialogueTrigger.TriggerDialogue();
                }
                else
                {
                    dialogueTrigger = insufficientFunds.GetComponent<DialogueTrigger>();
                    dialogueTrigger.TriggerDialogue();
                }
            }
        }

        if (this.gameObject.name == ("StealthUpgrade"))
        {
            {
                upgrades.Stealth();

                if (upgrades.canPurchase == true)
                {
                    dialogueTrigger = this.gameObject.GetComponent<DialogueTrigger>();
                    dialogueTrigger.TriggerDialogue();
                }
                else
                {
                    dialogueTrigger = insufficientFunds.GetComponent<DialogueTrigger>();
                    dialogueTrigger.TriggerDialogue();
                }
            }
        }

        if (this.gameObject.name == ("SpeedUpgrade"))
        {
            {
                upgrades.SpeedUpgrade();

                if(upgrades.canPurchase == true)
                {
                    dialogueTrigger = this.gameObject.GetComponent<DialogueTrigger>();
                    dialogueTrigger.TriggerDialogue();
                }
                else
                {
                    dialogueTrigger = insufficientFunds.GetComponent<DialogueTrigger>();
                    dialogueTrigger.TriggerDialogue();
                }
            }
        }
    }

    //void OnMouseOver()
    //{
    //    Renderer renderer = this.gameObject.GetComponent<Renderer>();
    //    renderer.material.SetColor("_BaseColor", Color.red);
        
    //}

    //void OnMouseOut()
    //{
    //    Renderer renderer = this.gameObject.GetComponent<Renderer>();
    //    renderer.material.SetColor("_BaseColor", Color.grey);
    //}
}

