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
}
