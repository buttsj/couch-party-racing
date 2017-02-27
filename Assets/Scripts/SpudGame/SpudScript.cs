using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Vector3 spawnPos;
    private Quaternion spawnRot;
    public bool RoundOver { get; set; }
    private int roundCount;
    public int RoundCount { get; }

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
	}
	
	// Update is called once per frame
	void Update () {

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
        transform.position = spawnPos;
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
