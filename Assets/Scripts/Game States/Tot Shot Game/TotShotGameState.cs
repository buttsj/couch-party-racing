using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotShotGameState : IGameState {

    public GameObject player;

    public TotShotGameState(GameObject kart)
    {
        player = kart;
    }

    public void Start()
    {

    }

    public void NonDamagedUpdate()
    {

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

}
