using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public int checkpointNumber;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            
            RacingGameState racingGameState = (RacingGameState)other.GetComponent<Kart>().GameState;

            if (checkpointNumber == racingGameState.CurrentCheckpoint)
            {
                racingGameState.PlayerCheckpointPosition = new Vector3(transform.position.x, transform.position.y - 240.0f, transform.position.z);
                racingGameState.PlayerCheckpointRotation = new Vector3(other.GetComponent<Kart>().transform.localEulerAngles.x, other.GetComponent<Kart>().transform.localEulerAngles.y, other.GetComponent<Kart>().transform.localEulerAngles.z);
                racingGameState.CurrentCheckpoint++;
                if(racingGameState.CurrentCheckpoint >= racingGameState.NumberOfCheckpoints)
                {
                    racingGameState.LapNumber++;
                    racingGameState.CurrentCheckpoint = 0;
                }
            }

        }else if (other.gameObject.name.Contains("AI"))
        {

            RacingGameState racingGameState = (RacingGameState)other.GetComponent<WaypointAI>().GameState;
            if (checkpointNumber == racingGameState.CurrentCheckpoint)
            {
                other.GetComponent<WaypointAI>().ResetWaypoint = gameObject.GetComponent<Waypoint>().waypointNumber;
                racingGameState.PlayerCheckpointPosition = new Vector3(transform.position.x, transform.position.y - 240.0f, transform.position.z);
                racingGameState.PlayerCheckpointRotation = new Vector3(other.GetComponent<WaypointAI>().transform.localEulerAngles.x, other.GetComponent<WaypointAI>().transform.localEulerAngles.y, other.GetComponent<WaypointAI>().transform.localEulerAngles.z);
                racingGameState.CurrentCheckpoint++;
                if (racingGameState.CurrentCheckpoint >= racingGameState.NumberOfCheckpoints)
                {
                    racingGameState.LapNumber++;
                    racingGameState.CurrentCheckpoint = 0;
                }
            }

        }

    }

    public void checkpointSkip(GameObject other)
    {
        RacingGameState racingGameState = (RacingGameState)other.GetComponent<WaypointAI>().GameState;
        other.GetComponent<WaypointAI>().ResetWaypoint = gameObject.GetComponent<Waypoint>().waypointNumber;
        racingGameState.PlayerCheckpointPosition = new Vector3(transform.position.x, transform.position.y - 240.0f, transform.position.z);
        racingGameState.PlayerCheckpointRotation = new Vector3(other.GetComponent<WaypointAI>().transform.localEulerAngles.x, other.GetComponent<WaypointAI>().transform.localEulerAngles.y, other.GetComponent<WaypointAI>().transform.localEulerAngles.z);
        racingGameState.CurrentCheckpoint++;
        if (racingGameState.CurrentCheckpoint >= racingGameState.NumberOfCheckpoints)
        {
            racingGameState.LapNumber++;
            racingGameState.CurrentCheckpoint = 0;
        }
    }

}
