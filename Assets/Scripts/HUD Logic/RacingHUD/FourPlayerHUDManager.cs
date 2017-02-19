using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FourPlayerHUDManager : MonoBehaviour {
    private float boost1;
    private float boost2;
    private float boost3;
    private float boost4;
    private string poweruptype;
    public GameObject kart1;
    public GameObject kart2;
    public GameObject kart3;
    public GameObject kart4;
    public Text boostText;
    public Text lapText;
    public Text boostText2;
    public Text lapText2;
    public Text boostText3;
    public Text lapText3;
    public Text boostText4;
    public Text lapText4;
    public Text placeText1;
    public Text placeText2;
    public Text placeText3;
    public Text placeText4;
    public Text checkpointText1;
    public Text checkpointText2;
    public Text checkpointText3;
    public Text checkpointText4;
    private int totalCheckpoints;
    List<float> seconds;
    List<int> minutes;
    // Use this for initialization
    void Start()
    {
        seconds = new List<float>();
        minutes = new List<int>();
        seconds.Add(0f);
        seconds.Add(0f);
        seconds.Add(0f);
        seconds.Add(0f);
        minutes.Add(0);
        minutes.Add(0);
        minutes.Add(0);
        minutes.Add(0);
        totalCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint").Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateKartHUD(0);
        UpdateKartHUD(1);
        UpdateKartHUD(2);
        UpdateKartHUD(3);
    }

    void UpdateKartHUD(int kartNumber)
    {
        switch (kartNumber)
        {
            case 0:
                boost1 = kart1.GetComponent<Kart>().Boost;
                boostText.text = ((int)boost1).ToString();
                poweruptype = kart1.GetComponent<Kart>().Powerup;
                UpdatePlace(kart1, 1);
                UpdateCheckpointNumber(kart1, 1);
                if (((RacingGameState)kart1.GetComponent<Kart>().GameState).LapNumber == 0)
                {
                    lapText.text = "1 / 3";
                }
                else if (((RacingGameState)kart1.GetComponent<Kart>().GameState).LapNumber > 3)
                {
                    lapText.text = "3 / 3";
                }
                else
                {
                    lapText.text = ((RacingGameState)kart1.GetComponent<Kart>().GameState).LapNumber.ToString() + " / 3";
                }

                if (((RacingGameState)kart1.GetComponent<Kart>().GameState).LapNumber < 4)
                    UpdateTimerUI(0);
                break;
            case 1:
                boost2 = kart2.GetComponent<Kart>().Boost;
                boostText2.text = ((int)boost2).ToString();
                poweruptype = kart2.GetComponent<Kart>().Powerup;
                UpdatePlace(kart2, 2);
                UpdateCheckpointNumber(kart2,2);
                if (((RacingGameState)kart2.GetComponent<Kart>().GameState).LapNumber == 0)
                {
                    lapText2.text = "1 / 3";
                }
                else if (((RacingGameState)kart2.GetComponent<Kart>().GameState).LapNumber > 3)
                {
                    lapText2.text = "3 / 3";
                }
                else
                {
                    lapText2.text = ((RacingGameState)kart2.GetComponent<Kart>().GameState).LapNumber.ToString() + " / 3";
                }

                if (((RacingGameState)kart2.GetComponent<Kart>().GameState).LapNumber < 4)
                    UpdateTimerUI(1);
                break;
            case 2:
                boost3 = kart3.GetComponent<Kart>().Boost;
                boostText3.text = ((int)boost3).ToString();
                poweruptype = kart3.GetComponent<Kart>().Powerup;
                UpdatePlace(kart3, 3);
                UpdateCheckpointNumber(kart3, 3);
                if (((RacingGameState)kart3.GetComponent<Kart>().GameState).LapNumber == 0)
                {
                    lapText3.text = "1 / 3";
                }
                else if (((RacingGameState)kart3.GetComponent<Kart>().GameState).LapNumber > 3)
                {
                    lapText3.text = "3 / 3";
                }
                else
                {
                    lapText3.text = ((RacingGameState)kart3.GetComponent<Kart>().GameState).LapNumber.ToString() + " / 3";
                }

                if (((RacingGameState)kart3.GetComponent<Kart>().GameState).LapNumber < 4)
                    UpdateTimerUI(2);
                break;

            case 3:
                boost4 = kart4.GetComponent<Kart>().Boost;
                boostText4.text = ((int)boost4).ToString();
                poweruptype = kart4.GetComponent<Kart>().Powerup;
                UpdatePlace(kart4, 4);
                UpdateCheckpointNumber(kart4, 4);
                if (((RacingGameState)kart4.GetComponent<Kart>().GameState).LapNumber == 0)
                {
                    lapText4.text = "1 / 3";
                }
                else if (((RacingGameState)kart4.GetComponent<Kart>().GameState).LapNumber > 3)
                {
                    lapText4.text = "3 / 3";
                }
                else
                {
                    lapText4.text = ((RacingGameState)kart4.GetComponent<Kart>().GameState).LapNumber.ToString() + " / 3";
                }

                if (((RacingGameState)kart4.GetComponent<Kart>().GameState).LapNumber < 4)
                    UpdateTimerUI(3);
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
        switch (playerNumber)
        {
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
                time = minuteText + ":" + secondsText;
                kart3.GetComponent<Kart>().TimeText = time;
                if (seconds[playerNumber] >= 60)
                {
                    minutes[playerNumber]++;
                    seconds[playerNumber] = 0;
                }
                break;

            case 3:
                time = minuteText + ":" + secondsText;
                kart4.GetComponent<Kart>().TimeText = time;
                if (seconds[playerNumber] >= 60)
                {
                    minutes[playerNumber]++;
                    seconds[playerNumber] = 0;
                }
                break;
        }
    }

    void UpdatePlace(GameObject kart, int playerNumber)
    {
        int place = ((RacingGameState)kart.GetComponent<Kart>().GameState).Place;

        switch (place)
        {
            case 1:
                if (playerNumber == 1)
                    placeText1.text = "1st";
                else if (playerNumber == 2)
                    placeText2.text = "1st";
                else if (playerNumber == 3)
                    placeText3.text = "1st";
                else
                    placeText4.text = "1st";
                break;
            case 2:
                if (playerNumber == 1)
                    placeText1.text = "2nd";
                else if (playerNumber == 2)
                    placeText2.text = "2nd";
                else if (playerNumber == 3)
                    placeText3.text = "2nd";
                else
                    placeText4.text = "2nd";
                break;
            case 3:
                if (playerNumber == 1)
                    placeText1.text = "3rd";
                else if (playerNumber == 2)
                    placeText2.text = "3rd";
                else if (playerNumber == 3)
                    placeText3.text = "3rd";
                else
                    placeText4.text = "3rd";
                break;
            case 4:
                if (playerNumber == 1)
                    placeText1.text = "4th";
                else if (playerNumber == 2)
                    placeText2.text = "4th";
                else if (playerNumber == 3)
                    placeText3.text = "4th";
                else
                    placeText4.text = "4th";
                break;
        }
    }
    void UpdateCheckpointNumber(GameObject kart, int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                checkpointText1.text = ((RacingGameState)kart1.GetComponent<Kart>().GameState).CurrentCheckpoint.ToString() + " / " + totalCheckpoints.ToString();
                break;
            case 2:
                checkpointText2.text = ((RacingGameState)kart2.GetComponent<Kart>().GameState).CurrentCheckpoint.ToString() + " / " + totalCheckpoints.ToString();
                break;
            case 3:
                checkpointText3.text = ((RacingGameState)kart3.GetComponent<Kart>().GameState).CurrentCheckpoint.ToString() + " / " + totalCheckpoints.ToString();
                break;
            case 4:
                checkpointText4.text = ((RacingGameState)kart4.GetComponent<Kart>().GameState).CurrentCheckpoint.ToString() + " / " + totalCheckpoints.ToString();
                break;
        }
    }
}
