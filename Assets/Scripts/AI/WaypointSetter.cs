using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public static class WaypointSetter {

    private static GameObject[] waypoints;
    private static List<GameObject> closedList;
    private static Queue<GameObject> openList;

    public static void SetWaypoints()
    {
        int currentWaypointNumber = 0;

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        closedList = new List<GameObject>();
        openList = new Queue<GameObject>();

        GameObject endWaypoint = GameObject.Find("EndWaypoint");
        endWaypoint.GetComponent<Waypoint>().waypointNumber = waypoints.Length - 1;
        closedList.Add(endWaypoint);

        openList.Enqueue(GameObject.Find("StartWaypoint"));
        while(openList.ToArray().Length > 0)
        {

            GameObject front = openList.Dequeue();
            front.GetComponent<Waypoint>().waypointNumber = currentWaypointNumber;
            currentWaypointNumber++;

            float minDist = float.MaxValue;
            int indexOfMin = -1;

            for(int i = 0; i < waypoints.Length; i++)
            {
                if (!inClosedList(waypoints[i]))
                {
                    float dist = Vector3.Distance(front.transform.position, waypoints[i].transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        indexOfMin = i;
                    }
                }
            }

            if(indexOfMin >= 0)
            {
                openList.Enqueue(waypoints[indexOfMin]);
            }

        }
    }

    private static bool inClosedList(GameObject obj)
    {
        bool inList = false;

        return inList;
    }

}
