using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RacingGameManager : MonoBehaviour {
    List<GameObject> playerList;
    List<GameObject> aiList;
    List<Text> playerTexts;
    public Canvas canvas;
    public Text player1Text;
    public Text player2Text;
    public Text player3Text;
    public Text player4Text;
    public Button exit;
    public bool raceOver;

    // Use this for initialization
    void Start () {
        playerTexts = new List<Text>();
        playerList = new List<GameObject>();
        canvas = canvas.GetComponent<Canvas>();
        exit = exit.GetComponent<Button>();
        canvas.enabled = false;
        playerList = new List<GameObject>();
        aiList = new List<GameObject>();
        LoadPlayers();
        playerTexts.Add(player1Text);
        playerTexts.Add(player2Text);
        playerTexts.Add(player3Text);
        playerTexts.Add(player4Text);
    }
	
	// Update is called once per frame
	void Update () {
        if (playerList.Count == 0) {
            LoadPlayers();
        }
        else if (AllKartsFinishedRace()) {
            raceOver = true;
            canvas.enabled = true;
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("ExitToMainMenu"));
            for (int i = 0; i < playerList.Count; i++) {
                    playerTexts[i].text = playerList[i].GetComponent<Kart>().TimeText;        
                }
            for (int i = playerList.Count; i < aiList.Count + playerList.Count; i++)
            {
                playerTexts[i].text = aiList[i - playerList.Count].GetComponent<WaypointAI>().TimeText;
            }
       }
	}

    bool AllKartsFinishedRace() {
        bool finished = true;

        foreach (GameObject player in playerList) {
            if (player.GetComponent<Kart>() != null)
            {
                if (player.GetComponent<Kart>().LapNumber < 4)
                {
                    finished = false;
                }
            }
            else if (player.GetComponent<WaypointAI>() != null) {
                if (player.GetComponent<WaypointAI>().LapNumber < 4) {
                    finished = false;
                }
            }

        }
        return finished;
    }

    public void exitPress()
    {
        canvas.enabled = false;
        SceneManager.LoadScene("MainMenu");
    }

    void LoadPlayers() {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            playerList.Add(player);
        }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("AI"))
        {
            aiList.Add(player);
        }
    }
}
