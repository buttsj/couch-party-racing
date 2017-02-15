using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public int checkpointNumber;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            
            Kart player = other.GetComponent<Kart>();

            if (checkpointNumber == player.CurrentCheckpoint)
            {
                player.PlayerCheckpointPosition = transform.position;
                player.PlayerCheckpointRotation = new Vector3(player.transform.localEulerAngles.x, player.transform.localEulerAngles.y, player.transform.localEulerAngles.z);
                player.CurrentCheckpoint++;
                if(player.CurrentCheckpoint >= player.NumberOfCheckpoints)
                {
                    player.LapNumber++;
                    player.CurrentCheckpoint = 0;
                }
            }

        }

    }

}
