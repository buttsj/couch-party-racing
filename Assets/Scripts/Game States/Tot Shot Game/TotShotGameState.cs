using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotShotGameState : IGameState {

    public GameObject player;
    private string teamColor;

    public TotShotGameState(GameObject kart, string team)
    {
        player = kart;
        teamColor = team;
    }

    public void Start()
    {
        
    }

    public void NonDamagedUpdate()
    {
        player.GetComponent<Kart>().PhysicsObject.TurningSpeed = 3.5f;
        if (SimpleInput.GetButtonDown("Back Flip", player.GetComponent<Kart>().PlayerNumber))
        {
            player.GetComponent<Kart>().PhysicsObject.BackFlip();
        }
        else if (SimpleInput.GetButtonDown("Front Flip", player.GetComponent<Kart>().PlayerNumber))
        {
            player.GetComponent<Kart>().PhysicsObject.FrontFlip();
        }
        else if (SimpleInput.GetButtonDown("Right Roll", player.GetComponent<Kart>().PlayerNumber)) {
            player.GetComponent<Kart>().PhysicsObject.RightRoll();
        }
        else if (SimpleInput.GetButtonDown("Left Roll", player.GetComponent<Kart>().PlayerNumber))
        {
            player.GetComponent<Kart>().PhysicsObject.LeftRoll();
        }
    }

    public void DamagedUpdate()
    {

    }

    public void ResetKart()
    {
        player.transform.localEulerAngles = new Vector3(0, player.transform.localEulerAngles.y, 0);
    }

    public string GetGameStateName()
    {
        return "TotShotGameState";
    }

    public void OnCollisionEnter(GameObject other)
    {

    }

    public void OnTriggerEnter(GameObject other) { }

    public string getTeam()
    {
        return teamColor;
    }
}
