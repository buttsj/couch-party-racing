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

        numberofWaypoints = 33;

        physics = new KartPhysics(gameObject, 150, 250, 300);

        boost = 100.0f;
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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTowardTarget, 0.18f);

        fLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        fRightModel.transform.Rotate(Vector3.right * physics.Speed);
        rLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        rRightModel.transform.Rotate(Vector3.right * physics.Speed);

        if (boost > 0)
        {
            physics.StartBoost();
            boost -= .5f;
        }
        else
        {
            physics.EndBoost();
        }

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
