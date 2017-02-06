using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    private float boost;
    private string poweruptype;
    public GameObject kart;
    private List<GameObject> aiList;
    public Text boostText;
    public Text lapText;
    public Text timerText;
    List<float> seconds;
    List<int> minutes;

    // Use this for initialization
    void Start () {
        aiList = new List<GameObject>();
        seconds = new List<float>();
        minutes = new List<int>();
        seconds.Add(0f);
        minutes.Add(0);
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("AI"))
        {
            aiList.Add(player);
            seconds.Add(0f);
            minutes.Add(0);
        }
    }

    // Update is called once per frame
    void Update() {
        boost = kart.GetComponent<Kart>().Boost;
        boostText.text = ((int)boost).ToString();
        poweruptype = kart.GetComponent<Kart>().Powerup;
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
            UpdateTimerUI(kart, 0);

        for(int i = 0; i < aiList.Count; i++) {
            if (aiList[i].GetComponent<WaypointAI>().LapNumber < 4)
                UpdateTimerUI(aiList[i], i + 1);
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
        else {
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

        timerText.text = minuteText + ":" + secondsText;
        if (kart.GetComponent<Kart>() != null)
            kart.GetComponent<Kart>().TimeText = timerText.text;
        else if (kart.GetComponent<WaypointAI>() != null)
            kart.GetComponent<WaypointAI>().TimeText = timerText.text;

        if (seconds[kartNumber] >= 60)
        {
            minutes[kartNumber]++;
            seconds[kartNumber] = 0;
        }
    }
}
