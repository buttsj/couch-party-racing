using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    private float boost;
    public GameObject kart;
    public Text boostText;
    public Text lapText;
    public Text timerText;
    float secondsCount;
    int minuteCount;
	// Use this for initialization
	void Start () {
        
    }

    // Update is called once per frame
    void Update() {
        boost = kart.GetComponent<Kart>().Boost;
        boostText.text = ((int)boost).ToString();
        if (kart.GetComponent<Kart>().LapNumber == 0)
        {
            lapText.text = "1 / 3";
        }
        else if (kart.GetComponent<Kart>().LapNumber > 3) {
            lapText.text = "3 / 3";
        }
        else
        {
            lapText.text = kart.GetComponent<Kart>().LapNumber.ToString() + " / 3";
        }

        if(kart.GetComponent<Kart>().LapNumber < 4)
            UpdateTimerUI();
    }

    public void UpdateTimerUI()
    {
        string minuteText;
        string secondsText;
        secondsCount += Time.deltaTime;

        if (secondsCount < 10)
        {
            secondsText = "0" + secondsCount.ToString("F2");
        }
        else {
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

        timerText.text = minuteText + ":" + secondsText;
        kart.GetComponent<Kart>().TimeText = timerText.text;
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
    }
}
