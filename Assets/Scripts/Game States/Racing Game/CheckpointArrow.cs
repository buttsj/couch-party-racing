using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointArrow : MonoBehaviour {
    private List<GameObject> checkpoints;
    private GameObject[] waypoints;
    public GameObject kart;
    private Vector3 offset = new Vector3(0, 15, 0);
    

    // Use this for initialization
    void Start () {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        checkpoints = new List<GameObject>();
        foreach (GameObject waypoint in waypoints) {
            if (waypoint.GetComponent<Checkpoint>()) {
                checkpoints.Add(waypoint);
            }
        }

        for (int i = 1; i < checkpoints.Count ; i++)
        {
            int j = i;
            while (j > 0 && (checkpoints[j - 1].GetComponent<Checkpoint>().checkpointNumber > checkpoints[j].GetComponent<Checkpoint>().checkpointNumber))
            {
                GameObject temp = checkpoints[j - 1];
                checkpoints[j - 1] = checkpoints[j];
                checkpoints[j] = temp;
                j--;
            }
        }

        //transform.Rotate(90, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 targetCheckpointXZPosition = new Vector3(checkpoints[((RacingGameState)kart.GetComponent<Kart>().GameState).CurrentCheckpoint].transform.position.x, 0.0f, checkpoints[((RacingGameState)kart.GetComponent<Kart>().GameState).CurrentCheckpoint].transform.position.z);
        Vector3 arrowXZPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Quaternion targetQuaternion = Quaternion.LookRotation((targetCheckpointXZPosition - arrowXZPosition).normalized);
        Quaternion slerpQuaternion = Quaternion.Slerp(transform.rotation, targetQuaternion, 0.2f);
        transform.rotation = new Quaternion(transform.rotation.x, slerpQuaternion.y, transform.rotation.z, slerpQuaternion.w);
        if (Mathf.Abs(Vector3.Angle(gameObject.transform.forward, kart.transform.forward)) > 120f)
        {
            SetColor(Color.red);
        }
        else {
            SetColor(Color.green);
        }

        transform.position = kart.transform.position + offset;
    }

    private void SetColor(Color color) {
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = color;
        gameObject.transform.GetChild(1).GetComponent<Renderer>().material.color = color;
        gameObject.transform.GetChild(2).GetComponent<Renderer>().material.color = color;
    }
}
