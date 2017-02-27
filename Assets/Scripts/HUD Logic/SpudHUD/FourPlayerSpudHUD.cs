using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FourPlayerSpudHUD : MonoBehaviour
{

    public GameObject potato;
    public Text timeRemainingText;
    float secondsRemain;
    float minutesRemain;
    public Text boostText1;
    public Text boostText2;
    public Text boostText3;
    public Text boostText4;
    public Text scoreText1;
    public Text scoreText2;
    public Text scoreText3;
    public Text scoreText4;
    public Image spark1;
    public Image oil1;
    public Image boostImg1;
    public Image spark2;
    public Image oil2;
    public Image boostImg2;
    public Image spark3;
    public Image oil3;
    public Image boostImg3;
    public Image spark4;
    public Image oil4;
    public Image boostImg4;
    public Image marble1;
    public Image marble2;
    public Image marble3;
    public Image marble4;
    public GameObject kart1;
    public GameObject kart2;
    public GameObject kart3;
    public GameObject kart4;
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
        UpdateKartHUD(2);
        UpdateKartHUD(3);
        if (potato.GetComponent<SpudScript>().IsTagged && !potato.gameObject.GetComponent<SpudScript>().GameOver)
        {
            UpdateTimerUI();
        }
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
            case 2:
                boostText3.text = ((int)kart3.GetComponent<Kart>().Boost).ToString();
                scoreText3.text = ((SpudRunGameState)kart3.GetComponent<Kart>().GameState).SpudScore.ToString("F2");
                UpdatePowerup(kart3, 3);
                break;
            case 3:
                boostText4.text = ((int)kart4.GetComponent<Kart>().Boost).ToString();
                scoreText4.text = ((SpudRunGameState)kart4.GetComponent<Kart>().GameState).SpudScore.ToString("F2");
                UpdatePowerup(kart4, 4);
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
                else if (playerNumber == 2)
                {
                    spark2.enabled = false;
                    oil2.enabled = false;
                    boostImg2.enabled = true;
                    marble2.enabled = false;
                }
                else if (playerNumber == 3)
                {
                    spark3.enabled = false;
                    oil3.enabled = false;
                    boostImg3.enabled = true;
                    marble3.enabled = false;
                }
                else
                {
                    spark4.enabled = false;
                    oil4.enabled = false;
                    boostImg4.enabled = true;
                    marble4.enabled = false;
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
                else if (playerNumber == 2)
                {
                    spark2.enabled = true;
                    oil2.enabled = false;
                    boostImg2.enabled = false;
                    marble2.enabled = false;
                }
                else if (playerNumber == 3)
                {
                    spark3.enabled = true;
                    oil3.enabled = false;
                    boostImg3.enabled = false;
                    marble3.enabled = false;
                }
                else
                {
                    spark4.enabled = true;
                    oil4.enabled = false;
                    boostImg4.enabled = false;
                    marble4.enabled = false;
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
                else if (playerNumber == 2)
                {
                    oil2.enabled = true;
                    spark2.enabled = false;
                    boostImg2.enabled = false;
                    marble2.enabled = false;
                }
                else if (playerNumber == 3)
                {
                    oil3.enabled = true;
                    spark3.enabled = false;
                    boostImg3.enabled = false;
                    marble3.enabled = false;
                }
                else
                {
                    oil4.enabled = true;
                    spark4.enabled = false;
                    boostImg4.enabled = false;
                    marble4.enabled = false;
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
                else if (playerNumber == 2)
                {
                    oil2.enabled = false;
                    spark2.enabled = false;
                    boostImg2.enabled = false;
                    marble2.enabled = true;
                }
                else if (playerNumber == 3)
                {
                    oil3.enabled = false;
                    spark3.enabled = false;
                    boostImg3.enabled = false;
                    marble3.enabled = true;
                }
                else
                {
                    oil4.enabled = false;
                    spark4.enabled = false;
                    boostImg4.enabled = false;
                    marble4.enabled = true;
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
                else if (playerNumber == 2)
                {
                    oil2.enabled = false;
                    spark2.enabled = false;
                    boostImg2.enabled = false;
                    marble2.enabled = false;
                }
                else if (playerNumber == 3)
                {
                    oil3.enabled = false;
                    spark3.enabled = false;
                    boostImg3.enabled = false;
                    marble3.enabled = false;
                }
                else
                {
                    oil4.enabled = false;
                    spark4.enabled = false;
                    boostImg4.enabled = false;
                    marble4.enabled = false;
                }
                break;
        }
    }
}
