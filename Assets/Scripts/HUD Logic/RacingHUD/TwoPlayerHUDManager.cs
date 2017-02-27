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
    public Image spark1;
    public Image oil1;
    public Image boostImg1;
    public Image marble1;
    public Image spark2;
    public Image oil2;
    public Image boostImg2;
    public Image marble2;
    public Text boostText;
    public Text lapText;
    public Text boostText2;
    public Text lapText2;
    public Text placeText1;
    public Text placeText2;
    public Text checkpointText1;
    public Text checkpointText2;
    List<float> seconds;
    List<int> minutes;
    private List<GameObject> aiList;
    private int totalCheckpoints;
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
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.name.Contains("AI"))
            {
                aiList.Add(player);
                seconds.Add(0f);
                minutes.Add(0);
            }
        }
        totalCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint").Length - 1;
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
                UpdatePlace(kart1, 1);
                UpdateCheckpointNumber(kart1, 1);
                UpdatePowerup(kart1, 1);
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
                UpdateCheckpointNumber(kart2, 2);
                UpdatePowerup(kart2, 2);
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

    void UpdatePlace(GameObject kart, int playerNumber)
    {
        int place = ((RacingGameState)kart.GetComponent<Kart>().GameState).Place;

        switch (place)
        {
            case 1:
                if(playerNumber == 1)
                    placeText1.text = "1st";
                else
                    placeText2.text = "1st";
                break;
            case 2:
                if (playerNumber == 1)
                    placeText1.text = "2nd";
                else
                    placeText2.text = "2nd";
                break;
            case 3:
                if (playerNumber == 1)
                    placeText1.text = "3rd";
                else
                    placeText2.text = "3rd";
                break;
            case 4:
                if (playerNumber == 1)
                    placeText1.text = "4th";
                else
                    placeText2.text = "4th";
                break;
        }
    }

    void UpdateCheckpointNumber(GameObject kart, int playerNumber)
    {
        switch (playerNumber) {
            case 1:
                checkpointText1.text = ((RacingGameState)kart1.GetComponent<Kart>().GameState).CurrentCheckpoint.ToString() + " / " + totalCheckpoints.ToString();
                break;
            case 2:
                checkpointText2.text = ((RacingGameState)kart2.GetComponent<Kart>().GameState).CurrentCheckpoint.ToString() + " / " + totalCheckpoints.ToString();
                break;
        }
    }

    void UpdatePowerup(GameObject kart, int playerNumber)
    {

        switch (kart.GetComponent<Kart>().Ability.ToString())
        {
            case "Boost":
                if (playerNumber == 1)
                {
                    spark1.enabled = false;
                    oil1.enabled = false;
                    boostImg1.enabled = true;
                    marble1.enabled = false;
                }
                else {
                    spark2.enabled = false;
                    oil2.enabled = false;
                    boostImg2.enabled = true;
                    marble2.enabled = false;
                }
                break;
            case "Spark":
                if (playerNumber == 1)
                {
                    spark1.enabled = true;
                    oil1.enabled = false;
                    boostImg1.enabled = false;
                    marble1.enabled = false;
                }
                else {
                    spark2.enabled = true;
                    oil2.enabled = false;
                    boostImg2.enabled = false;
                    marble2.enabled = false;
                }
                break;
            case "Oil":
                if (playerNumber == 1)
                {
                    oil1.enabled = true;
                    spark1.enabled = false;
                    boostImg1.enabled = false;
                    marble1.enabled = false;
                }
                else {
                    oil2.enabled = true;
                    spark2.enabled = false;
                    boostImg2.enabled = false;
                    marble2.enabled = false;
                }
                break;

            case "Marble":
                if (playerNumber == 1)
                {
                    oil1.enabled = false;
                    spark1.enabled = false;
                    boostImg1.enabled = false;
                    marble1.enabled = true;
                }
                else
                {
                    oil2.enabled = false;
                    spark2.enabled = false;
                    boostImg2.enabled = false;
                    marble2.enabled = true;
                }
                break;

            case "Null":
                if (playerNumber == 1)
                {
                    oil1.enabled = false;
                    spark1.enabled = false;
                    boostImg1.enabled = false;
                    marble1.enabled = false;
                }
                else {
                    oil2.enabled = false;
                    spark2.enabled = false;
                    boostImg2.enabled = false;
                    marble2.enabled = false;
                }
                break;
        }
    }

}
