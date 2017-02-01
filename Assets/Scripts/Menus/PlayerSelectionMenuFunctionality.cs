﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectionMenuFunctionality : MonoBehaviour {

    public const string READY = "Ready!";
    public const string UNREADY = "Unready";

    public Text player1ReadyText;
    public Text player2ReadyText;
    public Text player3ReadyText;
    public Text player4ReadyText;

	void Start () {

        player1ReadyText.text = UNREADY;
        player2ReadyText.text = UNREADY;
        player3ReadyText.text = UNREADY;
        player4ReadyText.text = UNREADY;
    }
	
	void Update () {
		
        if(SimpleInput.GetButtonDown("Pause", 1) && (player1ReadyText.text == READY))
        {
            SceneManager.LoadScene("Temp_Scene");
        }

        checkForReadyPlayers();
    }

    private void checkForReadyPlayers()
    {

        if (SimpleInput.GetButtonDown("Accelerate", 1))
        {
            player1ReadyText.text = READY;
        }
        else if (SimpleInput.GetButtonDown("Use PowerUp", 1) && (player1ReadyText.text == READY))
        {
            player1ReadyText.text = UNREADY;
        }

        if (SimpleInput.GetButtonDown("Accelerate", 2))
        {
            player2ReadyText.text = READY;
        }
        else if (SimpleInput.GetButtonDown("Use PowerUp", 2) && (player2ReadyText.text == READY))
        {
            player2ReadyText.text = UNREADY;
        }

        if (SimpleInput.GetButtonDown("Accelerate", 3))
        {
            player3ReadyText.text = READY;
        }
        else if (SimpleInput.GetButtonDown("Use PowerUp", 3) && (player3ReadyText.text == READY))
        {
            player3ReadyText.text = UNREADY;
        }
    }

}
