using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public int waypointNumber;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "AIKart")
        {
            WaypointAI ai = other.GetComponent<WaypointAI>();
            ai.modifyTargetWaypoint(waypointNumber);
        }
    }

}
