using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class RacingEndGameMenu : MonoBehaviour {

    private WhiteFadeUniversal fader;

    List<GameObject> playerList;
    List<GameObject> aiList;
    List<Text> playerTexts;
    public GameObject playerTimeCanvas;
    public GameObject playerPlaceCanvas;
    public Canvas raceEndMenuCanvas;
    public Text player1Text;
    public Text player2Text;
    public Text player3Text;
    public Text player4Text;
    public Text firstPlaceText;
    public Text secondPlaceText;
    public Text thirdPlaceText;
    public Text fourthPlaceText;
    public Button exit;
    public Button nextScreen;
    private bool raceOver;
    private bool addedChips;
    private bool nextGameModePressed;
    private bool placeSet;
    public bool RaceOver { get { return raceOver; } }
    List<GameObject> kartList;

    private SceneGenerator sceneGenerator;

    void Awake()
    {
        // fade in/out initializer
        GameObject fadeObject = new GameObject();
        fadeObject.name = "Fader";
        fadeObject.transform.SetParent(transform);
        fadeObject.SetActive(true);
        fader = fadeObject.AddComponent<WhiteFadeUniversal>();
        fader.BeginExitScene("Music Manager HUD");
        //
    }

    // Use this for initialization
    public void Start () {
        playerTexts = new List<Text>();
        playerList = new List<GameObject>();
        raceEndMenuCanvas.enabled = false;
        playerTimeCanvas.SetActive(false);
        playerPlaceCanvas.SetActive(false);
        exit = exit.GetComponent<Button>();
        nextScreen = nextScreen.GetComponent<Button>();
        playerList = new List<GameObject>();
        aiList = new List<GameObject>();
        kartList = new List<GameObject>();
        LoadPlayers();
        kartList.AddRange(playerList);
        kartList.AddRange(aiList);
        placeSet = false;
        playerTexts.Add(player1Text);
        playerTexts.Add(player2Text);
        playerTexts.Add(player3Text);
        playerTexts.Add(player4Text);
    }
	
	// Update is called once per frame
	public void Update () {
        if (playerList.Count == 0)
        {
            LoadPlayers();
        }
        else if (AllKartsFinishedRace())
        {
            DetermineRacePositions();
            raceOver = true;
            raceEndMenuCanvas.enabled = true;
            if (!playerPlaceCanvas.activeInHierarchy)
            {
                playerTimeCanvas.SetActive(true);
            }
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("NextScreen"));
            for (int i = 0; i < playerList.Count; i++)
            {
                playerTexts[i].text = playerList[i].GetComponent<Kart>().TimeText;
            }
            for (int i = playerList.Count; i < aiList.Count + playerList.Count; i++)
            {
                playerTexts[i].text = aiList[i - playerList.Count].GetComponent<WaypointAI>().TimeText;
            }
            if (!placeSet)
            {
                SetPlaceText();
            }
            if (!addedChips && raceEndMenuCanvas.enabled)
            {
                GameObject.Find("AccountManager").GetComponent<AccountManager>().CurrentChips += 5;
                PlayerPrefs.Save();
                addedChips = true;
                if (CouchPartyManager.IsCouchPartyMode) {
                    AddCouchPartyPoints();
                }
            }

            if (playerPlaceCanvas.activeInHierarchy) {
                if (CouchPartyManager.IsCouchPartyMode)
                {
                    EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("NextGameMode"));
                }
                else {
                    EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("ExitToMainMenu"));
                }
            }
        }
        else {
            DetermineRacePositions();
        }
	}

    void SetPlaceText() {
        for (int i = 0; i < playerList.Count; i++)
        {
            switch (((RacingGameState)playerList[i].GetComponent<Kart>().GameState).Place)
            {
                case 1:
                    firstPlaceText.text = "Player " + (i + 1).ToString();
                    break;
                case 2:
                    secondPlaceText.text = "Player " + (i + 1).ToString();
                    break;
                case 3:
                    thirdPlaceText.text = "Player " + (i + 1).ToString();
                    break;
                case 4:
                    fourthPlaceText.text = "Player " + (i + 1).ToString();
                    break;
            }
        }

        for (int i = playerList.Count; i < playerList.Count + aiList.Count; i++) {
            switch (((RacingGameState)aiList[i - playerList.Count].GetComponent<WaypointAI>().GameState).Place) {
                case 1:
                    firstPlaceText.text = "Player " + (i+1).ToString();
                    break;
                case 2:
                    secondPlaceText.text = "Player " + (i + 1).ToString();
                    break;
                case 3:
                    thirdPlaceText.text = "Player " + (i + 1).ToString();
                    break;
                case 4:
                    fourthPlaceText.text = "Player " + (i + 1).ToString();
                    break;
            }
        }

        placeSet = true;
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

        }
        return finished;
    }

    public void nextScreenPress() {
        playerTimeCanvas.SetActive(false);
        playerPlaceCanvas.SetActive(true);
    }

    public void exitPress()
    {
        playerTimeCanvas.SetActive(false);
        StartCoroutine(leaveScene());
    }

    public void nextGameModePress() {
        if (!nextGameModePressed)
        {
            nextGameModePressed = true;
            playerTimeCanvas.SetActive(false);
            StartCoroutine(LoadNextGameMode());
            
        }
    }

    public IEnumerator leaveScene()
    {
        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;
        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator LoadNextGameMode() {
        sceneGenerator = Instantiate(Resources.Load<GameObject>("Prefabs/SceneGenerator"), Vector3.zero, Quaternion.Euler(Vector3.zero)).GetComponent<SceneGenerator>();
        sceneGenerator.GamemodeName = "SpudRun";
        sceneGenerator.SceneName = "SpudRunScene";
        sceneGenerator.LevelName = null;
        sceneGenerator.name = "SceneGenerator";
        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;
        SceneManager.LoadScene("CouchPartyEndScene");
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

    void DetermineRacePositions() {
        for (int i = 0; i < kartList.Count; i++) {
            int position = 1;

            float distance;
            if (kartList[i].GetComponent<Kart>() != null)
            {
                distance = ((RacingGameState)kartList[i].GetComponent<Kart>().GameState).GetDistance();
            }
            else {
                distance = ((RacingGameState)kartList[i].GetComponent<WaypointAI>().GameState).GetDistance();
            }
            foreach (GameObject kart in kartList) {
                if (kart.GetComponent<Kart>() != null)
                {
                    if (((RacingGameState)kart.GetComponent<Kart>().GameState).GetDistance() > distance)
                    {
                        position++;
                    }
                }
                else {
                    if (((RacingGameState)kart.GetComponent<WaypointAI>().GameState).GetDistance() > distance)
                    {
                        position++;
                    }
                }
            }
            if (kartList[i].GetComponent<Kart>() != null)
                ((RacingGameState)kartList[i].GetComponent<Kart>().GameState).Place = position;
            else
                ((RacingGameState)kartList[i].GetComponent<WaypointAI>().GameState).Place = position;
        }

    }

    void AddCouchPartyPoints() {
        foreach (GameObject kart in playerList) {
            if (kart.name.Contains("1"))
            {
                CouchPartyManager.PlayerOneScore += 5 - ((RacingGameState)kart.GetComponent<Kart>().GameState).Place;
            }
            else if (kart.name.Contains("2")) {
                CouchPartyManager.PlayerTwoScore += 5 - ((RacingGameState)kart.GetComponent<Kart>().GameState).Place;
            }
            else if (kart.name.Contains("3"))
            {
                CouchPartyManager.PlayerThreeScore += 5 - ((RacingGameState)kart.GetComponent<Kart>().GameState).Place;
            }
            else if (kart.name.Contains("4"))
            {
                CouchPartyManager.PlayerFourScore += 5 - ((RacingGameState)kart.GetComponent<Kart>().GameState).Place;
            }
        }
    }

}
