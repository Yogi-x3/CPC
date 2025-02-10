using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public Movement movement;

    private int index;
    private Dictionary<int, float> randomOffsets = new Dictionary<int, float>();
    private int previousCharacterCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
        //WobblyText();
    }

    void InitializeRandomOffsets(int characterCount)
    {
        var textInfo = textComponent.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            // Assign a random offset between -1.0f and 1.0f to each character
            randomOffsets[i] = Random.Range(-1.0f, 1.0f);
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    void RotatingText()
    {
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        if (textInfo.characterCount != previousCharacterCount)
        {
            InitializeRandomOffsets(textInfo.characterCount);
            previousCharacterCount = textInfo.characterCount;
        }

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }

            // Get the vertices of the character
            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            // Calculate the center point of the character
            Vector3 charCenter = (verts[charInfo.vertexIndex] + verts[charInfo.vertexIndex + 2]) / 2;

            // Calculate the rotation angle with a unique random offset for each character
            float angle = Mathf.Sin(Time.time * 2f + randomOffsets[i]) * 15f;

            // Create a rotation quaternion around the Z-axis
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            for (int j = 0; j < 4; j++)
            {
                // Move vertex to origin (relative to charCenter), apply rotation, then move back
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = charCenter + rotation * (orig - charCenter);
            }
        }

        

        // Update the mesh
        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }
    void WobblyText()
    {
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }
            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 10f, 0);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}