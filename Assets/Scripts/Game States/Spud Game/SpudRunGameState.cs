using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpudRunGameState : IGameState {

    public bool HoldingPotato { get; set; }
    public GameObject player;
    public float SpudScore { get; set; }

    public SpudRunGameState(GameObject kart) {
        player = kart;
    }
    // Use this for initialization
    public void Start () {
		
	}

    // Update is called once per frame
    public void NonDamagedUpdate() {
        if (HoldingPotato) {
            SpudScore += Time.deltaTime;
            player.GetComponent<Kart>().PhysicsObject.MaxSpeed = 200f;
        }
    }

  

    public void DamagedUpdate() {
        if (HoldingPotato)
        {
            // drop potato
            HoldingPotato = false;
            GameObject.FindGameObjectWithTag("Potato").GetComponent<SpudScript>().SpudHolder = null;
            GameObject.FindGameObjectWithTag("Potato").GetComponent<SpudScript>().IsTagged = false;
        }
    }

    public void ResetKart()
    {
        player.transform.localEulerAngles = new Vector3(0, player.transform.localEulerAngles.y, 0);
    }

    public string GetGameStateName() {
        return "SpudRunGameState";
    }

    public void OnCollisionEnter(GameObject other) {
        if (other.CompareTag("Player"))
        {
            if (((SpudRunGameState)other.GetComponent<Kart>().GameState).HoldingPotato)
            {
                ((SpudRunGameState)other.GetComponent<Kart>().GameState).HoldingPotato = false;
                GameObject.Find("Potato").GetComponent<SpudScript>().SpudHolder = null;
                GameObject.Find("Potato").GetComponent<SpudScript>().IsTagged = false;
                if (player.GetComponent<Kart>().Ability.ToString() == "Spark" && player.GetComponent<Kart>().Ability.IsUsing())
                {
                    SpudScore += 5;
                }
                else {
                    SpudScore += 3;
                }
            }
        }

    }
}
