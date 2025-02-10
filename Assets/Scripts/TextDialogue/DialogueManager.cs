using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        sentences = new Queue<string>();
    }



    public void StartDialogue (ObjectDialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {

            sentences.Enqueue(sentence); 
        }
        DisplayNextSentence();
    }


    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }

    }

    public void EndDialogue()
    {
        Debug.Log("no");
        animator.SetBool("isOpen", false);
    }
    // Update is called once per frame

}
