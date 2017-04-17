using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class SpudRunEndGameMenu : MonoBehaviour
{
    private WhiteFadeUniversal fader;
    List<GameObject> playerList;
    List<Text> playerTexts;
    public Canvas canvas;
    public Text player1Text;
    public Text player2Text;
    public Text player3Text;
    public Text player4Text;
    public Button exit;
    private bool raceOver;
    public bool RaceOver { get { return raceOver; } }
    GameObject potato;
    private bool addedChips;
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
    public void Start()
    {
        playerTexts = new List<Text>();
        playerList = new List<GameObject>();
        canvas = canvas.GetComponent<Canvas>();
        exit = exit.GetComponent<Button>();
        canvas.enabled = false;
        playerList = new List<GameObject>();
        LoadPlayers();
        potato = GameObject.Find("Potato");
        playerTexts.Add(player1Text);
        playerTexts.Add(player2Text);
        playerTexts.Add(player3Text);
        playerTexts.Add(player4Text);
    }

    // Update is called once per frame
    public void Update()
    {
        if (playerList.Count == 0)
        {
            LoadPlayers();
        }
        else if (potato.GetComponent<SpudScript>().GameOver)
        {
                canvas.enabled = true;
            if (CouchPartyManager.IsCouchPartyMode)
            {
                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("NextGameMode"));
            }
            else {
                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("ExitToMainMenu"));
            }

                for (int i = 0; i < playerList.Count; i++)
                {
                    playerTexts[i].text = ((SpudRunGameState)playerList[i].GetComponent<Kart>().GameState).SpudScore.ToString("F2");
                }

            DetermineKartRank();
            if (!addedChips && canvas.enabled) {
                GameObject.Find("AccountManager").GetComponent<AccountManager>().CurrentChips += 5;
                PlayerPrefs.Save();
                addedChips = true;
                if (CouchPartyManager.IsCouchPartyMode) {
                    AddCouchPartyPoints();
                }
            }
          
        }

    }

    public void exitPress()
    {
        canvas.enabled = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void nextGameModePress()
    {
        StartCoroutine(LoadNextGameMode());
    }

    public IEnumerator leaveScene()
    {
        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;
        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator LoadNextGameMode()
    {
        sceneGenerator = Instantiate(Resources.Load<GameObject>("Prefabs/SceneGenerator"), Vector3.zero, Quaternion.Euler(Vector3.zero)).GetComponent<SceneGenerator>();
        sceneGenerator.GamemodeName = "TotShot";
        sceneGenerator.SceneName = "TotShotScene";
        sceneGenerator.LevelName = null;
        sceneGenerator.name = "SceneGenerator";
        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;
        SceneManager.LoadScene("CouchPartyEndScene");
    }

    void LoadPlayers()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.name.Contains("Player"))
                playerList.Add(player);
        }
    }

    void DetermineKartRank()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            int position = 1;

            float score = 0;
            if (playerList[i].GetComponent<Kart>() != null)
            {
                score = ((SpudRunGameState)playerList[i].GetComponent<Kart>().GameState).SpudScore;
            }
            
            foreach (GameObject kart in playerList)
            {
                if (kart.GetComponent<Kart>() != null)
                {
                    if (((SpudRunGameState)kart.GetComponent<Kart>().GameState).SpudScore > score)
                    {
                        position++;
                    }
                }
                
            }
            if (playerList[i].GetComponent<Kart>() != null)
                ((SpudRunGameState)playerList[i].GetComponent<Kart>().GameState).Place = position;
            
        }

    }

    void AddCouchPartyPoints()
    {
        foreach (GameObject kart in playerList)
        {
            if (kart.name.Contains("1"))
            {
                CouchPartyManager.PlayerOneScore += 5 - ((SpudRunGameState)kart.GetComponent<Kart>().GameState).Place;
            }
            else if (kart.name.Contains("2"))
            {
                CouchPartyManager.PlayerTwoScore += 5 - ((SpudRunGameState)kart.GetComponent<Kart>().GameState).Place;
            }
            else if (kart.name.Contains("3"))
            {
                CouchPartyManager.PlayerThreeScore += 5 - ((SpudRunGameState)kart.GetComponent<Kart>().GameState).Place;
            }
            else if (kart.name.Contains("4"))
            {
                CouchPartyManager.PlayerFourScore += 5 - ((SpudRunGameState)kart.GetComponent<Kart>().GameState).Place;
            }
        }
    }



}
