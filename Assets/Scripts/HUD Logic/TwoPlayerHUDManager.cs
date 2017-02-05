using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoPlayerHUDManager : MonoBehaviour
{

    private float boost1;
    private float boost2;
    private string poweruptype;
    public GameObject kart1;
    public GameObject kart2;
    public Text boostText;
    public Text lapText;
    public Text timerText;
    public Text boostText2;
    public Text lapText2;
    public Text timerText2;
    float secondsCount;
    int minuteCount;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateKartHUD(1);
        UpdateKartHUD(2);
    }

    void UpdateKartHUD(int kartNumber) {
        switch (kartNumber) {
            case 1: 
                boost1 = kart1.GetComponent<Kart>().Boost;
                boostText.text = ((int)boost1).ToString();
                poweruptype = kart1.GetComponent<Kart>().Powerup;
                if (kart1.GetComponent<Kart>().LapNumber == 0)
                {
                    lapText.text = "1 / 3";
                }
                else if (kart1.GetComponent<Kart>().LapNumber > 3)
                {
                    lapText.text = "3 / 3";
                }
                else
                {
                    lapText.text = kart1.GetComponent<Kart>().LapNumber.ToString() + " / 3";
                }

                if (kart1.GetComponent<Kart>().LapNumber < 4)
                    UpdateTimerUI(1);
                break;
            case 2:
                boost2 = kart2.GetComponent<Kart>().Boost;
                boostText2.text = ((int)boost2).ToString();
                poweruptype = kart2.GetComponent<Kart>().Powerup;
                if (kart2.GetComponent<Kart>().LapNumber == 0)
                {
                    lapText2.text = "1 / 3";
                }
                else if (kart2.GetComponent<Kart>().LapNumber > 3)
                {
                    lapText2.text = "3 / 3";
                }
                else
                {
                    lapText2.text = kart2.GetComponent<Kart>().LapNumber.ToString() + " / 3";
                }

                if (kart2.GetComponent<Kart>().LapNumber < 4)
                    UpdateTimerUI(2);
                break;

        }

    }

    public void UpdateTimerUI(int playerNumber)
    {
        string minuteText;
        string secondsText;
        secondsCount += Time.deltaTime;

        if (secondsCount < 10)
        {
            secondsText = "0" + secondsCount.ToString("F2");
        }
        else
        {
            secondsText = secondsCount.ToString("F2");
        }

        if (minuteCount < 10)
        {
            minuteText = "0" + minuteCount;
        }
        else
        {
            minuteText = minuteCount.ToString();
        }
        switch (playerNumber) {
            case 1:
                timerText.text = minuteText + ":" + secondsText;
                kart1.GetComponent<Kart>().TimeText = timerText.text;
                if (secondsCount >= 60)
                {
                    minuteCount++;
                    secondsCount = 0;
                }
                break;

            case 2:
                timerText2.text = minuteText + ":" + secondsText;
                kart2.GetComponent<Kart>().TimeText = timerText2.text;
                if (secondsCount >= 60)
                {
                    minuteCount++;
                    secondsCount = 0;
                }
                break;
        }
    }

}
