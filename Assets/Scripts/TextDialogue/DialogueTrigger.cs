using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    public ObjectDialogue dialogue;
    
    // Start is called before the first frame update
    //void Start()
    //{
    //    Scene scene = SceneManager.GetActiveScene();
    //    if (scene.name == "Shop")
    //    {
    //        Debug.Log(scene.name);
    //        TriggerDialogue();
    //    }
    //}
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
