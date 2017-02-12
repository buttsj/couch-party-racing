using System.Collections;
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

    public Text startToContinueText;

    private SceneGenerator sceneGenerator;

    public string MapName = "";


    void Awake () {
        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();
    }

	void Start () {

        player1ReadyText.text = UNREADY;
        player2ReadyText.text = UNREADY;
        player3ReadyText.text = UNREADY;
        player4ReadyText.text = UNREADY;

        startToContinueText.text = "";

        SimpleInput.MapPlayersToDefaultPref();
    }

    void Update() {
        // just to quick load to Spud Run
        if (MapName == "SpudRunScene")
        {
            SceneManager.LoadScene(MapName);
        }
        if (SimpleInput.GetButtonDown("Pause", 1) && (player1ReadyText.text == READY)) {
            LoadScene();
        } else {
            checkForReadyPlayers();
        }
    }

    private void LoadScene() {
        // Configure Controls (Player Testing Order Matters)
        SimpleInput.ClearCurrentPlayerDevices();

        if (player1ReadyText.text == READY) {
            SimpleInput.MapPlayerToDevice(1);
        }

        if (player2ReadyText.text == READY) {
            SimpleInput.MapPlayerToDevice(2);
        }

        if (player3ReadyText.text == READY) {
            SimpleInput.MapPlayerToDevice(3);
        }

        if (player4ReadyText.text == READY) {
            SimpleInput.MapPlayerToDevice(4);
        }
        sceneGenerator.LoadLevel("tiles.xml");
        //sceneGenerator.LoadLevel("tiles.xml", MapName);
    }

    private void checkForReadyPlayers()
    {

        if (SimpleInput.GetAnyButtonDown(1) && (player1ReadyText.text == UNREADY))
        {
            player1ReadyText.text = READY;
            startToContinueText.text = "Press Start to Continue!";
        }
        else if ((SimpleInput.GetAnyButtonDown(1) && !SimpleInput.GetButtonDown("Pause", 1)) && (player1ReadyText.text == READY))
        {
            player1ReadyText.text = UNREADY;
            startToContinueText.text = "";
        }

        if (SimpleInput.GetAnyButtonDown(2) && (player2ReadyText.text == UNREADY))
        {
            player2ReadyText.text = READY;
        }
        else if (SimpleInput.GetAnyButtonDown(2) && (player2ReadyText.text == READY))
        {
            player2ReadyText.text = UNREADY;
        }

        if (SimpleInput.GetAnyButtonDown(3) && (player3ReadyText.text == UNREADY))
        {
            player3ReadyText.text = READY;
        }
        else if (SimpleInput.GetAnyButtonDown(3) && (player3ReadyText.text == READY))
        {
            player3ReadyText.text = UNREADY;
        }

        if (SimpleInput.GetAnyButtonDown(4) && (player4ReadyText.text == UNREADY))
        {
            player4ReadyText.text = READY;
        }
        else if (SimpleInput.GetAnyButtonDown(4) && (player4ReadyText.text == READY))
        {
            player4ReadyText.text = UNREADY;
        }
    }

    private int NumberOfReadyPlayers() {
        int count = 0;

        if (player1ReadyText.text == READY) {
            count++;
        }

        if (player2ReadyText.text == READY) {
            count++;
        }

        if (player3ReadyText.text == READY) {
            count++;
        }

        if (player4ReadyText.text == READY) {
            count++;
        }

        return count;
    }

}
