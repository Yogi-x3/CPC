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
    public AudioandFX FXscript;

    private string thisScene;
    public bool menuDisabled;
    public float menuTimer;
    public GameObject startButton;
    public GameObject QuitButton;
    public GameObject logo;
    public TMP_Text loadingText;
    public bool isLoading;
    private bool textdisplay;

    public GameObject desecratorText;
    public int desecratorInt;

    public bool openingScene;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("desecrator"))
        {
            desecratorInt = PlayerPrefs.GetInt("desecrator");
        }
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

        if (desecratorInt == 1)
        {
            desecratorText.SetActive(true);
        } else
        {
            desecratorText.SetActive(false);
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
        QuitButton.SetActive(false);
        logo.SetActive(false);
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
                camScript.enabled = false;
                menuTimer += 0.5f * Time.deltaTime;
            } else if (!textdisplay)
            {
                movementScript.enabled = true;
                camScript.enabled = true;
            }
        }
    }

    public IEnumerator LoadCinematic()
    {
        if (isLoading == true)
        {
            FXscript.rainAudio.Stop();
            FXscript.bellAudio.Stop();
            FXscript.rainParticle.Pause();
            textdisplay = true;
            isLoading = false;
            Debug.Log("go");
            loadingText.text = "LOADING...";
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

    public void Sorry()
    {
        desecratorInt =  0;
        PlayerPrefs.SetInt("desecrator", desecratorInt);
        PlayerPrefs.Save();
    }
}
