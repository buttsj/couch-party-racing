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


    KartPhysics physics;

    void Awake()
    {
        currentTargetWaypoint = 0;

        numberofWaypoints = 12;

        physics = new KartPhysics(gameObject, 150f, 250f, 300f);
    }

	void Start () {

        waypoints = new GameObject[numberofWaypoints];

        for(int i = 0; i < numberofWaypoints; i++)
        {
            waypoints[i] = GameObject.Find(i.ToString());
        }
        
    }
	
	void FixedUpdate () {

        Quaternion rotationTowardTarget = Quaternion.LookRotation((waypoints[currentTargetWaypoint].transform.localPosition - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTowardTarget, 1.0f);

        fLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        fRightModel.transform.Rotate(Vector3.right * physics.Speed);
        rLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        rRightModel.transform.Rotate(Vector3.right * physics.Speed);

        if (IsGrounded())
        {
            physics.Accelerate();
            physics.ApplyForces();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == currentTargetWaypoint.ToString())
        {
            currentTargetWaypoint++;
            if(currentTargetWaypoint >= numberofWaypoints)
            {
                currentTargetWaypoint = 0;
            }
        }

    }

    bool IsGrounded()
    {
        return Physics.SphereCast(new Ray(transform.position, -transform.up), 1f, 5);
    }

}
