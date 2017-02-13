using UnityEngine;

public class WaypointFollowerAI {

    private GameObject kart;
    private KartPhysics physics;
    private bool active;
    private int currentTargetWaypoint;
    private int subTarget;

    private GameObject[] waypoints;

    public WaypointFollowerAI(GameObject kart, KartPhysics physics, bool active)
    {
        this.kart = kart;
        this.physics = physics;
        this.active = active;
        currentTargetWaypoint = 0;
        subTarget = 0;

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        for (int i = 1; i < waypoints.Length; i++)
        {
            int j = i;
            while (j > 0 && (waypoints[j - 1].GetComponent<Waypoint>().waypointNumber > waypoints[j].GetComponent<Waypoint>().waypointNumber))
            {
                GameObject temp = waypoints[j - 1];
                waypoints[j - 1] = waypoints[j];
                waypoints[j] = temp;
                j--;
            }
        }
    }

    public void AIUpdate()
    {
        Vector3 targetWaypointXZPosition = new Vector3(waypoints[currentTargetWaypoint].transform.position.x, 0.0f, waypoints[currentTargetWaypoint].transform.position.z);
        Vector3 aiXZPosition = new Vector3(kart.transform.position.x, 0.0f, kart.transform.position.z);

        Quaternion targetQuaternion = Quaternion.LookRotation((targetWaypointXZPosition - aiXZPosition).normalized);
        Quaternion slerpQuaternion = Quaternion.Slerp(kart.transform.rotation, targetQuaternion, 0.2f);

        kart.transform.rotation = new Quaternion(kart.transform.rotation.x, slerpQuaternion.y, kart.transform.rotation.z, slerpQuaternion.w);

        physics.Accelerate();
        physics.ApplyForces();
    }

    public void modifyTargetWaypoint(int waypointNumber)
    {
        if (currentTargetWaypoint == waypointNumber)
        {
            currentTargetWaypoint++;
            if (currentTargetWaypoint >= waypoints.Length)
            {
                currentTargetWaypoint = 0;
            }
        }
    }

    public int CurrentTargetWaypoint
    {
        get { return currentTargetWaypoint; }
        set { currentTargetWaypoint = value; }
    }

    public bool Active
    {
        get { return active; }
        set { active = value; }
    }

}
