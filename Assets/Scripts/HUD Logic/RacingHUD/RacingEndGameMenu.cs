using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RacingEndGameMenu : MonoBehaviour {
    List<GameObject> playerList;
    List<GameObject> aiList;
    List<Text> playerTexts;
    public Canvas canvas;
    public Text player1Text;
    public Text player2Text;
    public Text player3Text;
    public Text player4Text;
    public Button exit;
    private bool raceOver;
    public bool RaceOver { get { return raceOver; } }

    // Use this for initialization
    public void Start () {
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
	public void Update () {
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
                if (((RacingGameState)player.GetComponent<Kart>().GameState).LapNumber < 4)
                {
                    finished = false;
                }
            }
            else if (player.GetComponent<WaypointAI>() != null) {
                if (((RacingGameState)player.GetComponent<WaypointAI>().GameState).LapNumber < 4) {
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
            if (player.name.Contains("Player"))
                playerList.Add(player);
            else if (player.name.Contains("AI"))
                aiList.Add(player);
        }
    }

}
