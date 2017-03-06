using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoPlayerSpudHUD : MonoBehaviour {
   
    public GameObject potato;
    public Text timeRemainingText;
    float secondsRemain;
    float minutesRemain;
    public Text boostText1;
    public Text boostText2;
    public Text scoreText1;
    public Text scoreText2;
    public Text RoundText;
    public Image spark1;
    public Image oil1;
    public Image boostImg1;
    public Image marble1;
    public Image marble2;
    public Image spark2;
    public Image oil2;
    public Image boostImg2;
    public GameObject kart1;
    public GameObject kart2;
    // Use this for initialization
    void Start()
    {
        secondsRemain = potato.GetComponent<SpudScript>().TimeRemaining;
        minutesRemain = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateKartHUD(0);
        UpdateKartHUD(1);
        if (potato.GetComponent<SpudScript>().IsTagged && !potato.gameObject.GetComponent<SpudScript>().GameOver)
        {
            UpdateTimerUI();
        }
        RoundText.text = potato.GetComponent<SpudScript>().RoundCount.ToString();
    }


    public void UpdateTimerUI()
    {
        string minuteRemainText;
        string secondsRemainText;
        potato.gameObject.GetComponent<SpudScript>().TimeRemaining -= Time.deltaTime;
        if (potato.gameObject.GetComponent<SpudScript>().TimeRemaining <= 0.0f)
        {
            secondsRemain = 0.0f;
            minutesRemain = 0;
            potato.gameObject.GetComponent<SpudScript>().TimeRemaining = 0.0f;
        }
        secondsRemain = potato.gameObject.GetComponent<SpudScript>().TimeRemaining;
        if (secondsRemain < 10)
        {
            secondsRemainText = secondsRemain.ToString("F2");
        }
        else
        {
            secondsRemainText = secondsRemain.ToString("F2");
        }
        if (minutesRemain < 10)
        {
            minuteRemainText = "0" + minutesRemain;
        }
        else
        {
            minuteRemainText = minutesRemain.ToString();
        }
        string timeRemainDisplay = minuteRemainText + ":" + secondsRemainText;
        timeRemainingText.text = timeRemainDisplay;
    }

    void UpdateKartHUD(int kartNumber)
    {
        switch (kartNumber)
        {
            case 0:
                boostText1.text = ((int)kart1.GetComponent<Kart>().Boost).ToString();
                scoreText1.text = ((SpudRunGameState)kart1.GetComponent<Kart>().GameState).SpudScore.ToString("F2");
                UpdatePowerup(kart1, 1);
                break;
            case 1:
                boostText2.text = ((int)kart2.GetComponent<Kart>().Boost).ToString();
                scoreText2.text = ((SpudRunGameState)kart2.GetComponent<Kart>().GameState).SpudScore.ToString("F2");
                UpdatePowerup(kart2, 2);
                break;
        }

    }

    void UpdatePowerup(GameObject kart, int playerNumber)
    {

        switch (kart.GetComponent<Kart>().Ability.ToString())
        {
            case "Boost":
                if (playerNumber == 1)
                {
                    spark1.enabled = false;
                    oil1.enabled = false;
                    boostImg1.enabled = true;
                    marble1.enabled = false;
                }
                else
                {
                    spark2.enabled = false;
                    oil2.enabled = false;
                    boostImg2.enabled = true;
                    marble2.enabled = false;
                }
                break;
            case "Spark":
                if (playerNumber == 1)
                {
                    spark1.enabled = true;
                    oil1.enabled = false;
                    boostImg1.enabled = false;
                    marble1.enabled = false;
                }
                else
                {
                    spark2.enabled = true;
                    oil2.enabled = false;
                    boostImg2.enabled = false;
                    marble2.enabled = false;
                }
                break;
            case "Oil":
                if (playerNumber == 1)
                {
                    oil1.enabled = true;
                    spark1.enabled = false;
                    boostImg1.enabled = false;
                    marble1.enabled = false;
                }
                else
                {
                    oil2.enabled = true;
                    spark2.enabled = false;
                    boostImg2.enabled = false;
                    marble2.enabled = false;
                }
                break;

            case "Marble":
                if (playerNumber == 1)
                {
                    oil1.enabled = false;
                    spark1.enabled = false;
                    boostImg1.enabled = false;
                    marble1.enabled = true;
                }
                else
                {
                    oil2.enabled = false;
                    spark2.enabled = false;
                    boostImg2.enabled = false;
                    marble2.enabled = true;
                }
                break;

            case "Null":
                if (playerNumber == 1)
                {
                    oil1.enabled = false;
                    spark1.enabled = false;
                    boostImg1.enabled = false;
                    marble1.enabled = false;
                }
                else
                {
                    oil2.enabled = false;
                    spark2.enabled = false;
                    boostImg2.enabled = false;
                    marble2.enabled = false;
                }
                break;
        }
    }
}
