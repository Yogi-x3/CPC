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
    public CinematicDialogue dialogueScript;

    private string thisScene;
    public bool menuDisabled;
    public float menuTimer;
    public GameObject startButton;
    public GameObject QuitButton;
    public GameObject logo;
    public TMP_Text loadingText;
    public bool isLoading;
    private bool textdisplay;
    public bool notPaused;
    public GameObject pauseMenu;
    public AudioListener audio;

    public GameObject desecratorText;
    public int desecratorInt;
    public TMP_Text counterText;
    public GameObject counter;
    public int counterInt;

    public bool openingScene;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefsCheck();
        thisScene = SceneManager.GetActiveScene().name;
        Debug.Log(thisScene);
        isLoading = false;
        notPaused = true;


        if (thisScene == "Cinematic Open")
        {
            openingScene = true;
        }

        if (thisScene == "Cinematic")
        {
            if (counterInt == 5)
            {
                dialogueScript.alternateDialogue = true;
            } else
            {
                dialogueScript.alternateDialogue = false;
            }
            menuDisabled = true;
            openingScene= false;
        }
    }
    
    void Update()
    {
        if (thisScene == "Cinematic Open")
        {
            counterText.text = "Endings: " + counterInt.ToString() + "/5";
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene("Cinematic Open");
    }

    public void MenuDisabled()
    {
        counter.SetActive(false);
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

    public void Pause()
    {
        notPaused = !notPaused;
        if (!notPaused)
        {
            audio.enabled = false;
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }

        if (notPaused)
        {
            audio.enabled = true;
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
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
    
    public void ResetCounter()
    {
        PlayerPrefs.DeleteKey("Neutral");
        PlayerPrefs.DeleteKey("Absolved");
        PlayerPrefs.DeleteKey("Guilt");
        PlayerPrefs.DeleteKey("Police");
        PlayerPrefs.DeleteKey("Damnation");
        counterInt = 0;
        PlayerPrefs.Save();
    }

    private void PlayerPrefsCheck()
    {
        counterInt = 0;
        if (PlayerPrefs.HasKey("desecrator"))
        {
            desecratorInt = PlayerPrefs.GetInt("desecrator");
        }

        if (PlayerPrefs.HasKey("Guilt"))
        {
            counterInt += 1;
        }

        if (PlayerPrefs.HasKey("Absolved"))
        {
            counterInt += 1;
        }

        if (PlayerPrefs.HasKey("Damnation"))
        {
            counterInt += 1;
        }

        if (PlayerPrefs.HasKey("Neutral"))
        {
            counterInt += 1;
        }

        if (PlayerPrefs.HasKey("Police"))
        {
            counterInt += 1;
        }
    }
}
