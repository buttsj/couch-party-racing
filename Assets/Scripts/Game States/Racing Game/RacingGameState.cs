using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingGameState : IGameState {
    private int lapNumber;
    public int LapNumber { get { return lapNumber; } set { lapNumber = value; } }
    private Vector3 playerCheckpointPosition;
    private Vector3 playerCheckpointRotation;
    private int currentCheckpointNumber;
    private int numberOfCheckpoints;
    public Vector3 PlayerCheckpointPosition { get { return playerCheckpointPosition; } set { playerCheckpointPosition = value; } }
    public Vector3 PlayerCheckpointRotation { get { return playerCheckpointRotation; } set { playerCheckpointRotation = value; } }
    public int CurrentCheckpoint { get { return currentCheckpointNumber; } set { currentCheckpointNumber = value; } }
    public int NumberOfCheckpoints { get { return numberOfCheckpoints; } }
    public int CurrentRacePosition { get; set; }
    public Vector2 LapAndCheckpoint { get { return new Vector2(lapNumber, currentCheckpointNumber); } }
    public int Place { get; set; }
    private GameObject player;

    public RacingGameState(GameObject kart) {
        player = kart;
        lapNumber = 1;
        playerCheckpointPosition = player.transform.position;
        playerCheckpointRotation = player.transform.localEulerAngles;
        currentCheckpointNumber = 0;
        numberOfCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint").Length;
    }

    public void Start () {
        
    }
	
	// Update is called once per frame
	public void Update () {
		
	}

    public void ResetKart() {
        player.transform.localEulerAngles = playerCheckpointRotation;
        player.transform.position = playerCheckpointPosition + 2 * Vector3.up;
    }

    public float GetDistance() {
        return (player.transform.position - playerCheckpointPosition).magnitude + currentCheckpointNumber * 100 + lapNumber * 1000; 
    }
}
