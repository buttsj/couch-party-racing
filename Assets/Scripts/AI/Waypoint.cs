using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public int waypointNumber;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "AIKart")
        {
            WaypointFollowerAI ai = other.GetComponent<WaypointFollowerAI>();
            ai.modifyTargetWaypoint(waypointNumber);
        }
    }

}
