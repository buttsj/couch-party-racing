using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpudHUDManager : MonoBehaviour {
   
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
            secondsRemain[0] = 0.0f;
            minutesRemain[0] = 0;
            potato.gameObject.GetComponent<SpudScript>().TimeRemaining = 0.0f;
            potato.gameObject.GetComponent<SpudScript>().GameOver = true;
        }
        secondsRemain[0] = potato.gameObject.GetComponent<SpudScript>().TimeRemaining;
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
        timeRemainingText.text = timeRemainDisplay;
    }
}
