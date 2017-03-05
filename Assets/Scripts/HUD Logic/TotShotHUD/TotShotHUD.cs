using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotShotHUD : MonoBehaviour
{
    public Text redScoreText;
    public Text blueScoreText;
    public int redScoreInt;
    public int blueScoreInt;
    public Text time;
    float secondsRemain;
    float minutesRemain;
    string seconds;
    string minutes;
    float timer = 3f;
    bool isCountingDown = true;

    public int RedScore { get { return redScoreInt; } set { redScoreInt = value; } }
    public int BlueScore { get { return blueScoreInt; } set { blueScoreInt = value; } }

    void Start()
    {
        redScoreInt = 0;
        blueScoreInt = 0;
        secondsRemain = 0;
        minutesRemain = 1;
        seconds = "00";
        minutes = "1";
        time.text = minutes + ":" + seconds;
    }

    void Update()
    {
        redScoreText.text = redScoreInt.ToString();
        blueScoreText.text = blueScoreInt.ToString();
        if (!isCountingDown) {
            UpdateTimerUI();
        }else
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 1)
        {
            isCountingDown = false;
        }
    }

    public void UpdateTimerUI()
    {
        if (!isOver())
        {
            secondsRemain -= Time.deltaTime;
            seconds = ((int)secondsRemain).ToString();
            if (secondsRemain < 10)
            {
                seconds = "0" + ((int)secondsRemain).ToString();
            }
            if (secondsRemain <= 0)
            {
                minutesRemain--;
                secondsRemain = 59.9f;
                seconds = "59";
            }
            minutes = minutesRemain.ToString();

            time.text = minutes + ":" + seconds;
        }
    }

    public bool isOver()
    {
        return (seconds == "00" && minutes == "0");
    }

}
