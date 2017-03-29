using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotShotGameState : IGameState {

    public GameObject player;
    private string teamColor;
    private int hopLimitCounter;
    private const int HOPLIMITMAX = 2;
    private bool dpadHeldDown;

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
        HandleBump();
        player.GetComponent<Kart>().PhysicsObject.TurningSpeed = 3.5f;
        if (dpadHeldDown)
        {
            if (SimpleInput.GetAxis("Flip", player.GetComponent<Kart>().PlayerNumber) < 0)
            {
                player.GetComponent<Kart>().PhysicsObject.BackFlip();
            }
            else if (SimpleInput.GetAxis("Flip", player.GetComponent<Kart>().PlayerNumber) > 0)
            {
                player.GetComponent<Kart>().PhysicsObject.FrontFlip();
            }
            else if (SimpleInput.GetAxis("Roll", player.GetComponent<Kart>().PlayerNumber) > 0)
            {
                player.GetComponent<Kart>().PhysicsObject.RightRoll();
            }
            else if (SimpleInput.GetAxis("Roll", player.GetComponent<Kart>().PlayerNumber) < 0)
            {
                player.GetComponent<Kart>().PhysicsObject.LeftRoll();
            }
        }
        dpadHeldDown = IsInputReset();
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

    void HandleBump() {

        if (SimpleInput.GetButtonDown("Bump Kart", player.GetComponent<Kart>().PlayerNumber) && hopLimitCounter < HOPLIMITMAX)
        {
            if (hopLimitCounter == 0)
            {
                player.GetComponent<Kart>().PhysicsObject.TotJump1();
            }
            else
            {
                player.GetComponent<Kart>().PhysicsObject.TotJump2();
            }
            hopLimitCounter++;
        }
        else if (Physics.SphereCast(new Ray(player.transform.position, -player.transform.up), 1f, 1))
        {
            hopLimitCounter = 0;
        }
    }

    bool IsInputReset() {

    return SimpleInput.GetAxis("Flip") == 0 && SimpleInput.GetAxis("Roll") == 0;
    }
}
