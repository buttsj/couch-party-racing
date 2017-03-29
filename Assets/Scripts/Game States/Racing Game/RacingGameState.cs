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
	public void NonDamagedUpdate () {
        player.GetComponent<Kart>().PhysicsObject.TurningSpeed = 2.25f;
        HandleBump();
    }


    public void DamagedUpdate() { }

    public void ResetKart() {
        player.transform.localEulerAngles = playerCheckpointRotation;
        player.transform.position = playerCheckpointPosition + 2 * Vector3.up;
    }

    public float GetDistance() {
        return (player.transform.position - playerCheckpointPosition).magnitude + currentCheckpointNumber * 1000 + lapNumber * 100000; 
    }

    public string GetGameStateName() {
        return "RacingGameState";
    }

    public void OnCollisionEnter(GameObject other) {
        if (other.gameObject.CompareTag("Player") && player.GetComponent<Kart>().Ability.ToString() == "Spark")
        {
            if (player.GetComponent<Kart>().Ability.IsUsing() && other.transform.name.Contains("AI"))
            {
                other.GetComponent<WaypointAI>().Damage();
            }
            else if (player.GetComponent<Kart>().Ability.IsUsing() && other.transform.name.Contains("Player") && !other.GetComponent<Kart>().IsInvulnerable)
            {
                other.GetComponent<Kart>().OriginalOrientation = new Vector3(other.transform.localEulerAngles.x, other.transform.localEulerAngles.y, other.transform.localEulerAngles.z);
                other.GetComponent<Kart>().IsDamaged = true;
            }
        }
    }

    public void OnTriggerEnter(GameObject other) { }

    void HandleBump() {
        if(SimpleInput.GetButtonDown("Bump Kart", player.GetComponent<Kart>().PlayerNumber) && player.GetComponent<Kart>().IsGrounded())
        {
            player.GetComponent<Kart>().PhysicsObject.BumpKart();
        }
    }
}
