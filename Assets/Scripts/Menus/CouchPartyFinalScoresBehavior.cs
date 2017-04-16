using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CouchPartyFinalScoresBehavior : MonoBehaviour {

    public Text player1ScoreText;
    public Text player2ScoreText;
    public Text player3ScoreText;
    public Text player4ScoreText;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        player1ScoreText.text = CouchPartyManager.PlayerOneScore.ToString();
        player2ScoreText.text = CouchPartyManager.PlayerTwoScore.ToString();
        player3ScoreText.text = CouchPartyManager.PlayerThreeScore.ToString();
        player4ScoreText.text = CouchPartyManager.PlayerFourScore.ToString();

        if (SimpleInput.GetButtonDown("Pause")) {
            Debug.Log("Hop pressed");
            if (CouchPartyManager.IsLastRound)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else {
                SceneManager.LoadScene("SelectionMenu");
            }
        }
	}
}
