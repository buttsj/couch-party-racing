using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpudGameManager : MonoBehaviour {
    List<GameObject> playerList;
    public bool raceOver;
    public GameObject kart; // overriding the player to be Player 1


    // Use this for initialization
    void Start () {
        playerList = new List<GameObject>();
        raceOver = false;
        kart.GetComponent<Kart>().PlayerNumber = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (playerList.Count == 0)
        {
            LoadPlayers();
        }
    }

    void LoadPlayers()
    {
        /*foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            playerList.Add(player);
        }*/
    }
}
