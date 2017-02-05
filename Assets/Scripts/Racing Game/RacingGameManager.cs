using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RacingGameManager : MonoBehaviour {
    List<GameObject> playerList;
    List<Text> playerTexts;
    public Canvas canvas;
    public Text player1Text;
    public Text player2Text;
    public Text player3Text;
    public Text player4Text;
    public Button exit;


	// Use this for initialization
	void Start () {
        playerTexts = new List<Text>();
        playerList = new List<GameObject>();
        canvas = canvas.GetComponent<Canvas>();
        exit = exit.GetComponent<Button>();
        canvas.enabled = false;
        playerList = new List<GameObject>();
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
            canvas.enabled = true;
            for (int i = 0; i < playerList.Count; i++) {
                playerTexts[i].text = playerList[i].GetComponent<Kart>().TimeText;

            }
        }
	}

    bool AllKartsFinishedRace() {
        bool finished = true;

        foreach (GameObject player in playerList) {
            if (player.GetComponent<Kart>().LapNumber < 4) {
                finished = false;
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
    }
}
