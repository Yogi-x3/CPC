using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogueTrigger dialogueTrigger;
    public GameObject openingDialogue;
    
    void Start()
    {
        openingDialogue = this.gameObject;
        dialogueTrigger = openingDialogue.GetComponent<DialogueTrigger>();
        dialogueTrigger.TriggerDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
