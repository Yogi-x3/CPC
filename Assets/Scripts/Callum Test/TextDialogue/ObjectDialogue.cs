using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectDialogue
{
    // Start is called before the first frame update
    public string name;
    [TextArea(3,10)]
    public string[] sentences;
}
