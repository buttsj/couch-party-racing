using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public int waypointNumber;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "AIKart")
        {

            WaypointAI waypointAI = other.GetComponent<WaypointAI>();

            if (waypointAI.CurrentTargetWaypoint == waypointNumber)
            {
                waypointAI.CurrentTargetWaypoint++;
                if (waypointAI.CurrentTargetWaypoint >= waypointAI.NumberOfWaypoints)
                {
                    waypointAI.CurrentTargetWaypoint = 0;
                }
            }

        }
    }

}
