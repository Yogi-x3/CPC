using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menus : MonoBehaviour
{
    public FirstPersonMovement movementScript;
    public PlayerCam camScript;

    private string thisScene;
    public Image menu;
    public bool menuDisabled;
    public float menuTimer;
    public GameObject startButton;
    public TMP_Text loadingText;
    public bool isLoading;
    private bool textdisplay;

    public bool openingScene;
    // Start is called before the first frame update
    void Start()
    {
        thisScene = SceneManager.GetActiveScene().name;
        Debug.Log(thisScene);
        isLoading = false;

        if (thisScene == "Cinematic Open")
        {
            openingScene = true;
        }

        if (thisScene == "Cinematic")
        {
            menuDisabled = true;
            openingScene= false;
        }
    }
    
    void Update()
    {
        if (thisScene == "Cinematic Open")
        {
            DisableMenu();
            StartCoroutine(LoadCinematic());
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
            if (!textdisplay)
            {
                loadingText.enabled = false;
            }

            if (menuTimer < maxTimer)
            {
                movementScript.enabled = false;
                menuTimer += 0.5f * Time.deltaTime;
            } else if (!textdisplay)
            {
                movementScript.enabled = true;
            }
            float menuOpacity = 1 - menuTimer;
            menu.color = new Color(menu.color.r, menu.color.g, menu.color.b, menuOpacity);
        }
    }

    public IEnumerator LoadCinematic()
    {
        if (isLoading == true)
        {
            textdisplay = true;
            isLoading = false;
            Debug.Log("go");
            loadingText.text = "loading...";
            loadingText.enabled =true;
            movementScript.enabled = false;
            camScript.enabled = false;
            movementScript.playerRb.velocity = new Vector3(0, 0f, 0);
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Cinematic");

        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
