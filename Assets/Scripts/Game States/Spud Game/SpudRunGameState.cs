﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpudRunGameState : IGameState {

    public bool HoldingPotato { get; set; }
    public GameObject player;
    public float SpudScore { get; set; }
    public float InvulnerableTimer { get; set; }

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
        InvulnerableTimer -= Time.deltaTime;
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
            if (((SpudRunGameState)other.GetComponent<Kart>().GameState).HoldingPotato && ((SpudRunGameState)other.GetComponent<Kart>().GameState).InvulnerableTimer < 0)
            {
                ((SpudRunGameState)other.GetComponent<Kart>().GameState).HoldingPotato = false;
                GameObject.Find("Potato").GetComponent<SpudScript>().SpudHolder = null;
                GameObject.Find("Potato").GetComponent<SpudScript>().IsTagged = false;
                if (player.GetComponent<Kart>().Ability.ToString() == "Spark" && player.GetComponent<Kart>().Ability.IsUsing())
                { 
                    other.gameObject.GetComponent<Kart>().IsDamaged = true;
                    SpudScore += 5;
                }
                else {
                    SpudScore += 3;
                }
            }
            else if (player.GetComponent<Kart>().Ability.ToString() == "Spark" && player.GetComponent<Kart>().Ability.IsUsing() && ((SpudRunGameState)other.GetComponent<Kart>().GameState).InvulnerableTimer < 0)
            {
                other.gameObject.GetComponent<Kart>().IsDamaged = true;
            }

        }

    }

    public void OnTriggerEnter(GameObject other) {
        if (other.gameObject.CompareTag("Potato"))
        {
            if (other.gameObject.GetComponent<SpudScript>().CanIGrab())
            {
                other.gameObject.GetComponent<SpudScript>().SpudHolder = player;
                other.gameObject.GetComponent<SpudScript>().IsTagged = true;
                HoldingPotato = true;
                InvulnerableTimer = 2f;
            }
        }

        if (other.name.Contains("Marble") && HoldingPotato)
        {
            ((SpudRunGameState)other.GetComponent<MarbleManager>().Owner.GetComponent<Kart>().GameState).SpudScore += 2;
        }
    }
}
