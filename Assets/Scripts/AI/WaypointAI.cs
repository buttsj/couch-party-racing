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

    private float boost;

    KartPhysics physics;

    void Awake()
    {
        currentTargetWaypoint = 0;

        physics = new KartPhysics(gameObject, 130, 200, 230);

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

        if (waypoints[currentTargetWaypoint].transform.parent.transform.parent.name.Contains("Turn"))
        {
            physics.MaxSpeed = 130;
        }
        else if (waypoints[currentTargetWaypoint].transform.parent.transform.parent.name.Contains("Ramp"))
        {
            physics.MaxSpeed = 200;
        }
        else
        {
            physics.MaxSpeed = 200;
        }

        if (IsGrounded())
        {
            Vector3 temp = new Vector3(waypoints[currentTargetWaypoint].transform.position.x, 0.0f, waypoints[currentTargetWaypoint].transform.position.z);
            Vector3 temp2 = new Vector3(transform.position.x, 0.0f, transform.position.z);
            Quaternion rotationTowardTarget = Quaternion.LookRotation((temp - temp2).normalized);

            transform.rotation = new Quaternion(transform.rotation.x, Quaternion.Slerp(transform.rotation, rotationTowardTarget, 0.2f).y, transform.rotation.z, transform.rotation.w);

            physics.Accelerate();
            physics.ApplyForces();
        }

        fLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        fRightModel.transform.Rotate(Vector3.right * physics.Speed);
        rLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        rRightModel.transform.Rotate(Vector3.right * physics.Speed);

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

}
