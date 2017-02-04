using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointAI : MonoBehaviour {

    public GameObject fLeftModel;
    public GameObject fRightModel;
    public GameObject rLeftModel;
    public GameObject rRightModel;
    public GameObject steering_wheel;

    public GameObject fLParent;
    public GameObject fRParent;

    private int currentTargetWaypoint;
    private int numberofWaypoints;
    private GameObject[] waypoints;

    private float boost;

    KartPhysics physics;

    void Awake()
    {
        currentTargetWaypoint = 0;

        physics = new KartPhysics(gameObject, 150, 200, 200);

        boost = 100.0f;
    }

	void Start () {

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        numberofWaypoints = waypoints.Length;

        for(int i = 1; i < numberofWaypoints; i++)
        {
            int j = i;
            while (j > 0 && (waypoints[j-1].GetComponent<Waypoint>().waypointNumber > waypoints[j].GetComponent<Waypoint>().waypointNumber))
            {
                GameObject temp = waypoints[j - 1];
                waypoints[j - 1] = waypoints[j];
                waypoints[j] = temp;
                j--;
            }
        }

    }
	
	void FixedUpdate () {

        Quaternion rotationTowardTarget = Quaternion.LookRotation((waypoints[currentTargetWaypoint].transform.position - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTowardTarget, .6f);

        fLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        fRightModel.transform.Rotate(Vector3.right * physics.Speed);
        rLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        rRightModel.transform.Rotate(Vector3.right * physics.Speed);

        /*
        if (boost > 0)
        {
            physics.StartBoost();
            boost -= .5f;
        }
        else
        {
            physics.EndBoost();
        }
        */

        Debug.Log(currentTargetWaypoint);

        physics.Accelerate();
        physics.ApplyForces();

    }

    public int NumberOfWaypoints
    {
        get { return numberofWaypoints; }
        set { numberofWaypoints = value; }
    }

    public int CurrentTargetWaypoint
    {
        get { return currentTargetWaypoint; }
        set { currentTargetWaypoint = value; }
    }

}
