using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    public FirstPersonMovement movementScript;

    private string thisScene;
    public Image menu;
    public bool menuDisabled;
    public float menuTimer;
    public GameObject startButton;
    // Start is called before the first frame update
    void Start()
    {
        thisScene = SceneManager.GetActiveScene().name;
        Debug.Log(thisScene);

        if (thisScene == "Cinematic Open")
        {

        }

        if (thisScene == "Cinematic")
        {
            menuDisabled = true;
        }
    }
    
    void Update()
    {
        if (thisScene == "Cinematic Open")
        {
            DisableMenu();
        }
       
    }
    public void Restart()
    {
        SceneManager.LoadScene("Cinematic Open");
    }

    public void MenuDisabled()
    {
        menuTimer = 0;
        menuDisabled = true;
        startButton.SetActive(false);
    }

    public void DisableMenu()
    {
        if (menuDisabled)
        {
            float maxTimer = 1;


            if (menuTimer < maxTimer)
            {
                movementScript.enabled = false;
                menuTimer += 0.5f * Time.deltaTime;
            } else
            {
                movementScript.enabled = true;
            }
            float menuOpacity = 1 - menuTimer;
            menu.color = new Color(menu.color.r, menu.color.g, menu.color.b, menuOpacity);
        }
    }
}
