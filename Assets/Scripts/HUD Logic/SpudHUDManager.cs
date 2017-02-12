using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpudHUDManager : MonoBehaviour {

    private float boost;
    private string poweruptype;
    public GameObject kart;
    public GameObject potato;
    public Text boostText;
    public Text timerText;
    List<float> seconds;
    List<int> minutes;

    // Use this for initialization
    void Start()
    {
        seconds = new List<float>();
        minutes = new List<int>();
        seconds.Add(0f);
        minutes.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        boost = kart.GetComponent<Kart>().Boost;
        boostText.text = ((int)boost).ToString();
        poweruptype = kart.GetComponent<Kart>().Powerup;
        if (potato.GetComponent<SpudScript>().IsTagged)
        {
            UpdateTimerUI(kart, 0);
        }
        
    }

    public void UpdateTimerUI(GameObject kart, int kartNumber)
    {
        string minuteText;
        string secondsText;
        seconds[kartNumber] += Time.deltaTime;

        if (seconds[kartNumber] < 10)
        {
            secondsText = "0" + seconds[kartNumber].ToString("F2");
        }
        else
        {
            secondsText = seconds[kartNumber].ToString("F2");
        }

        if (minutes[kartNumber] < 10)
        {
            minuteText = "0" + minutes[kartNumber];
        }
        else
        {
            minuteText = minutes[kartNumber].ToString();
        }
        string timeDisplay = minuteText + ":" + secondsText;

        if (kartNumber == 0)
        {
            kart.GetComponent<Kart>().TimeText = timeDisplay;
            timerText.text = timeDisplay;
        }
        else
        {
            kart.GetComponent<WaypointAI>().TimeText = timeDisplay;
        }

        if (seconds[kartNumber] >= 60)
        {
            minutes[kartNumber]++;
            seconds[kartNumber] = 0;
        }
    }
}
