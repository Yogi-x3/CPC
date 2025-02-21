using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    private string thisScene;
    // Start is called before the first frame update
    void Start()
    {
        thisScene = SceneManager.GetActiveScene().name;
        Debug.Log(thisScene);
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(thisScene);
    }
}
