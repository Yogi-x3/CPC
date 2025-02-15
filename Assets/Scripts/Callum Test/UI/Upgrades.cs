using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Upgrades : MonoBehaviour
{
    public ScoreManager scoreManager;
    public FloatSO scoreSO;
    public float playerSpeed;
    public float playerHealth;
    public float money;

    private float cost;
    private bool camUpgraded;
    private bool stealthUpgrade;
    public DialogueTrigger dialogueTrigger;
    public GameObject backButton;
    private bool levelSelect;

    public bool canPurchase;
    public GameObject levelSelectUI;
    // Start is called before the first frame update
    void Start()
    {
        playerSpeed = scoreSO.PlayerSpeed;
        money = scoreSO.Money;
        levelSelect = false;
        levelSelectUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = scoreSO.PlayerSpeed;
        money = scoreSO.Money;
        camUpgraded = scoreSO.CamPurchased;
        stealthUpgrade = scoreSO.StealthPurchased;
}

    public void SpeedUpgrade()
    {
        if (money >= 400 && playerSpeed < 15)
        {
            canPurchase = true;
            playerSpeed += 1;
            cost = 400;
            scoreManager.UpdateSpeed(playerSpeed);
            scoreManager.DecreaseMoney(cost);
            Debug.Log(money);
        } else
        {
            canPurchase = false;
        }
    }

    public void LeavingText()
    {
        dialogueTrigger = backButton.GetComponent<DialogueTrigger>();
        dialogueTrigger.TriggerDialogue();
        levelSelect = true;
    }

    public void LevelSelect()
    {
        Debug.Log(levelSelect);
        if (levelSelect == true)
        {
            levelSelectUI.SetActive(true);
        }
    }

    public void CloseLevelSelect()
    {
        if (levelSelect == true)
        {
            levelSelectUI.SetActive(false);
            levelSelect = false;
        }
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    
    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void CamUpgrade()
    {
        if (money >= 1000 && camUpgraded == false)
        {
            canPurchase = true;
            camUpgraded = true;
            cost = 1000;
            scoreManager.DecreaseMoney(cost);
            scoreManager.UpdateCam(camUpgraded);
        } else
        {
            canPurchase=false;
        }
    }

    public void Stealth()
    {
        if (money >= 1000 && stealthUpgrade == false)
        {
            canPurchase = true;
            stealthUpgrade = true;
            cost = 1000;
            scoreManager.DecreaseMoney(cost);
            scoreManager.UpdateStealth(stealthUpgrade);
        }
        else
        {
            canPurchase=false;
        }
    }
}
