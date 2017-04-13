using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpudScript : MonoBehaviour {

    private float timer;
    private bool tagged;
    private GameObject holder;
    private float invulnTimer;

    private const float SPUD_MAX_TIME = 60f;

    public float TimeRemaining { get { return timer; } set { timer = value; } }
    public bool IsTagged { get { return tagged; } set { tagged = value; } }
    public GameObject SpudHolder { get { return holder; } set { holder = value; } }

    private bool gameOver;
    public bool GameOver { get { return gameOver; } set { gameOver = value; } }
    private bool newRound;
    private float newRoundTimer;
    public Text newRoundText;

    private Vector3 spawnPos;
    private Quaternion spawnRot;

    private List<Vector3> spawnLocations = new List<Vector3>() { new Vector3(0, 65, 0), new Vector3(0, 5, 0), new Vector3(147, 65, 211) , new Vector3(-210, 65, 0)
, new Vector3(-183, 65, 184), new Vector3(188, 65, 184), new Vector3(0, 65, 118), new Vector3(0, 65, -210), new Vector3(189, 65, -180), new Vector3(350, 5, -365), new Vector3(350, 5, 327), new Vector3(-323, 5, 327), new Vector3(-323, 5, -344)};
    public bool RoundOver { get; set; }
    private int roundCount;
    public int RoundCount { get { return roundCount; } }

    public bool CanIGrab()
    {
        if (invulnTimer > 2.0f)
            return true;
        else
            return false;
    }

	// Use this for initialization
	void Start () {
        spawnPos = gameObject.transform.position;
        spawnRot = gameObject.transform.rotation;
        timer = SPUD_MAX_TIME;
        tagged = false;
        gameOver = false;
        RoundOver = false;
        invulnTimer = 0.0f;
        roundCount = 1;
        newRound = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (newRound) {
            newRoundTimer += Time.deltaTime;
            if (newRoundTimer > 2f) {
                newRoundText.text = "";
                newRound = false;
                newRoundTimer = 0;
            }
        }

        if (timer <= 0) {
            RoundOver = true;
        }
        if (RoundOver)
        {

            
            if (roundCount < 3)
            {
                ResetRound();
                roundCount++;
                Debug.Log(roundCount);
            }
            else {
                gameOver = true;
                EndRound();
            }
            
        }


		if (tagged)
        {
            invulnTimer = 0.0f;
            transform.position = holder.transform.position;
            transform.rotation = holder.transform.rotation;
        }
        else
        {
            invulnTimer = invulnTimer + Time.deltaTime;
            transform.Rotate(Vector3.up, 2.0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            timer = 0;
        }
	}

    void ResetRound()
    {
        RoundOver = false;
        timer = SPUD_MAX_TIME;
        if (tagged)
        {
            tagged = false;
            ((SpudRunGameState)holder.GetComponent<Kart>().GameState).HoldingPotato = false;
            holder = null;
        }
        newRoundText.text = "Round over. Find the Spud !";
        newRound = true;
        transform.position = spawnLocations[Random.Range(0, spawnLocations.Count - 1)];
        transform.rotation = spawnRot;
    }

    void EndRound() {
        if (tagged)
        {
            tagged = false;
            ((SpudRunGameState)holder.GetComponent<Kart>().GameState).HoldingPotato = false;
            holder = null;
        }
        transform.position = spawnPos;
        transform.rotation = spawnRot;

    }
}
