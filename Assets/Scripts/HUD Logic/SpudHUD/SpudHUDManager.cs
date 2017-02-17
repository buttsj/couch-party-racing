using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpudHUDManager : MonoBehaviour {

    private bool GameOver;
    public bool IsGameOver { get { return GameOver; } set { GameOver = value; } }
    
    public GameObject potato;

    public Text timeRemainingText;
    List<float> secondsRemain;
    List<int> minutesRemain;

    // Use this for initialization
    void Start()
    {
        secondsRemain = new List<float>();
        minutesRemain = new List<int>();
        secondsRemain.Add(potato.GetComponent<SpudScript>().TimeRemaining);
        minutesRemain.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (potato.GetComponent<SpudScript>().IsTagged && !GameOver)
        {
            UpdateTimerUI();
        }
    }


    public void UpdateTimerUI()
    {
        string minuteRemainText;
        string secondsRemainText;
        secondsRemain[0] -= Time.deltaTime;
        if (potato.GetComponent<SpudScript>().GameOver)
        {
            secondsRemain[0] = 0.0f;
            minutesRemain[0] = 0;
            GameOver = true;
        }
        if (secondsRemain[0] < 10)
        {
            secondsRemainText = secondsRemain[0].ToString("F2");
        }
        else
        {
            secondsRemainText = secondsRemain[0].ToString("F2");
        }
        if (minutesRemain[0] < 10)
        {
            minuteRemainText = "0" + minutesRemain[0];
        }
        else
        {
            minuteRemainText = minutesRemain[0].ToString();
        }
        string timeRemainDisplay = minuteRemainText + ":" + secondsRemainText;
        potato.GetComponent<SpudScript>().TimeRemaining = secondsRemain[0];
        timeRemainingText.text = timeRemainDisplay;
    }
}
