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
    public Text boostText2;
    public Text lapText2;
    List<float> seconds;
    List<int> minutes;
    private List<GameObject> aiList;
    // Use this for initialization
    void Start()
    {
        aiList = new List<GameObject>();
        seconds = new List<float>();
        minutes = new List<int>();
        seconds.Add(0f);
        seconds.Add(0f);
        minutes.Add(0);
        minutes.Add(0);
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("AI"))
        {
            aiList.Add(player);
            seconds.Add(0f);
            minutes.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateKartHUD(0);
        UpdateKartHUD(1);
        UpdateKartHUD(2);
    }

    void UpdateKartHUD(int kartNumber) {
        switch (kartNumber) {
            case 0: 
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
                    UpdateTimerUI(0);
                break;
            case 1:
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
                    UpdateTimerUI(1);
                break;

            case 2:
                UpdateTimerUI(2);
                break;

        }

    }

    public void UpdateTimerUI(int playerNumber)
    {
        string minuteText;
        string secondsText;
        seconds[playerNumber] += Time.deltaTime;

        if (seconds[playerNumber] < 10)
        {
            secondsText = "0" + seconds[playerNumber].ToString("F2");
        }
        else
        {
            secondsText = seconds[playerNumber].ToString("F2");
        }

        if (minutes[playerNumber] < 10)
        {
            minuteText = "0" + minutes[playerNumber];
        }
        else
        {
            minuteText = minutes[playerNumber].ToString();
        }
        switch (playerNumber) {
            case 0:
                string time = minuteText + ":" + secondsText;
                kart1.GetComponent<Kart>().TimeText = time;
                if (seconds[playerNumber] >= 60)
                {
                    minutes[playerNumber]++;
                    seconds[playerNumber] = 0;
                }
                break;

            case 1:
                time = minuteText + ":" + secondsText;
                kart2.GetComponent<Kart>().TimeText = time;
                if (seconds[playerNumber] >= 60)
                {
                    minutes[playerNumber]++;
                    seconds[playerNumber] = 0;
                }
                break;

            case 2:
                int i = 2;
                foreach (GameObject player in aiList)
                {
                    time = minuteText + ":" + secondsText;
                    player.GetComponent<WaypointAI>().TimeText = time;
                    if (seconds[i] >= 60)
                    {
                        minutes[i]++;
                        seconds[i] = 0;
                    }
                    i++;
                }
                break;
        }
    }

}
