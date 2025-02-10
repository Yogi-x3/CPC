using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    private FloatSO scoreSO;
    //[SerializeField]
    //private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text speedText;
    [SerializeField]
    private TMP_Text moneyText;


    public PipeWeapon pipeWeapon;
    // Start is called before the first frame update
    void Start()
    {
        //scoreText.text = scoreSO.PipeValue + "";
        speedText.text = scoreSO.PlayerSpeed + "";
        moneyText.text = scoreSO.Money + "";
    }

    // Update is called once per frame
    void Update()
    {
        //scoreText.text = scoreSO.PipeValue + "";
        speedText.text = scoreSO.PlayerSpeed + "";
        moneyText.text = scoreSO.Money + "";
    }
    //public void UpdateScore(float newValue)
    //{
    //    scoreSO.PipeValue = newValue;
    //    scoreText.text = scoreSO.PipeValue + "";

    //}
    //public void UpdateBool(bool newBool)
    //{
    //    scoreSO.IsWeaponActive = newBool;
    //    boolText.text = scoreSO.IsWeaponActive + "";

    //}

    public void UpdateSpeed(float newValue)
    {
        scoreSO.PlayerSpeed = newValue;
        speedText.text = scoreSO.PlayerSpeed + "";

    }

    public void UpdateMoney(float newValue)
    {
        scoreSO.Money += newValue;
        moneyText.text = scoreSO.Money + "";

    }

    public void DecreaseMoney(float newValue)
    {
        scoreSO.Money -= newValue;
        moneyText.text = scoreSO.Money + "";

    }

    public void UpdateCam(bool newBool)
    {
        scoreSO.CamPurchased = newBool;
    }

    public void UpdateStealth(bool newBool)
    {
        scoreSO.StealthPurchased = newBool;
    }
}
