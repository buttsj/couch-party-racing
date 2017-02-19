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
            potato.gameObject.GetComponent<SpudScript>().GameOver = true;
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
                boostText1.text = kart1.GetComponent<Kart>().Boost.ToString();
                scoreText1.text = ((SpudRunGameState)kart1.GetComponent<Kart>().GameState).SpudScore.ToString("F2");
                break;
            case 1:
                scoreText2.text = ((SpudRunGameState)kart2.GetComponent<Kart>().GameState).SpudScore.ToString("F2");
                break;
        }

    }
}
