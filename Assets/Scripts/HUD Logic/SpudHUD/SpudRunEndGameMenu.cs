using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class SpudRunEndGameMenu : MonoBehaviour
{
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
                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("ExitToMainMenu"));
                for (int i = 0; i < playerList.Count; i++)
                {
                    playerTexts[i].text = ((SpudRunGameState)playerList[i].GetComponent<Kart>().GameState).SpudScore.ToString("F2");
                }
            if (!addedChips && canvas.enabled) {
                GameObject.Find("AccountManager").GetComponent<AccountManager>().CurrentChips += 5;
                PlayerPrefs.Save();
                addedChips = true;
            }
          
        }

    }

    public void exitPress()
    {
        canvas.enabled = false;
        SceneManager.LoadScene("MainMenu");
    }

    void LoadPlayers()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.name.Contains("Player"))
                playerList.Add(player);
        }
    }

   

}
