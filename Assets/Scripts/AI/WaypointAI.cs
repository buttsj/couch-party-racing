﻿using System.Collections;
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
    private int lapNumber;
    private string timeText;
    public int LapNumber { get { return lapNumber; } }
    public string TimeText { get { return timeText; } set { timeText = value; } }

    private float boost;

    KartPhysics physics;

    void Awake()
    {
        currentTargetWaypoint = 0;

        physics = new KartPhysics(gameObject, 100, 200, 230);

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

        AIPhysics();

        fLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        fRightModel.transform.Rotate(Vector3.right * physics.Speed);
        rLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        rRightModel.transform.Rotate(Vector3.right * physics.Speed);

    }

    private void AIPhysics()
    {
        if (waypoints[currentTargetWaypoint].transform.parent.transform.parent.name.Contains("Turn"))
        {
            physics.MaxSpeed = 130;
        }
        else if (waypoints[currentTargetWaypoint].transform.parent.transform.parent.name.Contains("Curvy"))
        {
            physics.MaxSpeed = 130;
        }
        else if (waypoints[currentTargetWaypoint].transform.parent.transform.parent.name.Contains("Corner"))
        {
            physics.MaxSpeed = 100;
        }
        else
        {
            physics.MaxSpeed = 200;
        }

        if (IsGrounded())
        {

            Vector3 targetWaypointXZPosition = new Vector3(waypoints[currentTargetWaypoint].transform.position.x, 0.0f, waypoints[currentTargetWaypoint].transform.position.z);
            Vector3 aiXZPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);

            Quaternion targetQuaternion = Quaternion.LookRotation((targetWaypointXZPosition - aiXZPosition).normalized);
            Quaternion slerpQuaternion = Quaternion.Slerp(transform.rotation, targetQuaternion, 0.2f);

            transform.rotation = new Quaternion(transform.rotation.x, slerpQuaternion.y, transform.rotation.z, slerpQuaternion.w);

            physics.Accelerate();
            physics.ApplyForces();
        }
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

    bool IsGrounded()
    {
        return Physics.SphereCast(new Ray(transform.position, -transform.up), 1f, 5);
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.name.Contains("Finish"))
        {
            lapNumber++;
        }
    }

}
