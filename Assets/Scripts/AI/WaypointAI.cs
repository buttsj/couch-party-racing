using UnityEngine;

public class WaypointAI : MonoBehaviour {

    private int currentTargetWaypoint;
    private int numberofWaypoints;
    private GameObject[] waypoints;
    private int selectedTargetChild;

    private KartPhysics physics;
    private float boost;

    public GameObject fLeftModel;
    public GameObject fRightModel;
    public GameObject rLeftModel;
    public GameObject rRightModel;
    public GameObject steering_wheel;
    public GameObject fLParent;
    public GameObject fRParent;

    private string timeText;
    private IGameState gameState;

    private bool damaged;

    private float selfTimer;
    private Vector3 originalOrientation;

    private bool isBoosting;

    void Start() {

        selfTimer = 0.0f;
        currentTargetWaypoint = 0;
        selectedTargetChild = 0;

        physics = new KartPhysics(gameObject, 100, 200, 250);

        boost = 100.0f;

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        numberofWaypoints = waypoints.Length;

        for (int i = 1; i < numberofWaypoints; i++)
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

        damaged = false;

        isBoosting = false;
    }

    void FixedUpdate() {

        if (!damaged)
        {
            handleAIPhysics();
        }
        else
        {
            handleDamage();
        }

        handleWheelAnimation();
    }

    private void handleWheelAnimation()
    {
        fLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        fRightModel.transform.Rotate(Vector3.right * physics.Speed);
        rLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        rRightModel.transform.Rotate(Vector3.right * physics.Speed);
    }

    private void handleAIPhysics()
    {
        if (IsGrounded())
        {
            handleMovementModifications();

            Vector3 targetWaypointXZPosition = new Vector3(waypoints[currentTargetWaypoint].transform.GetChild(selectedTargetChild).position.x, 0.0f, waypoints[currentTargetWaypoint].transform.GetChild(selectedTargetChild).position.z);
            Vector3 aiXZPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);

            Quaternion targetQuaternion = Quaternion.LookRotation((targetWaypointXZPosition - aiXZPosition).normalized);
            Quaternion slerpQuaternion = Quaternion.Slerp(transform.rotation, targetQuaternion, 0.2f);

            transform.rotation = new Quaternion(transform.rotation.x, slerpQuaternion.y, transform.rotation.z, slerpQuaternion.w);

            physics.Accelerate();
            physics.ApplyForces();
        }
    }

    private void handleMovementModifications()
    {
        int secondTargetIndex = currentTargetWaypoint + 1;
        int thirdTargetIndex = currentTargetWaypoint + 2;
        if (secondTargetIndex >= numberofWaypoints)
        {
            secondTargetIndex = 0;
            thirdTargetIndex = secondTargetIndex + 1;
        }
        else if (thirdTargetIndex >= numberofWaypoints)
        {
            thirdTargetIndex = 0;
        }

        string firstTarget = waypoints[currentTargetWaypoint].transform.parent.transform.parent.name;
        string secondTarget = waypoints[secondTargetIndex].transform.parent.transform.parent.name;
        string thirdTarget = waypoints[thirdTargetIndex].transform.parent.transform.parent.name;

        if (IsStraightTrackType(firstTarget) && IsStraightTrackType(secondTarget) && IsStraightTrackType(thirdTarget) && boost > 0.0f)
        {
            physics.StartBoost();
            isBoosting = true;
        }
        else
        {
            physics.EndBoost();
            isBoosting = false;
        }

        if (isBoosting && boost > 0.0f)
        {
            boost -= 0.5f;
        }
    }

    private void handleDamage()
    {
        physics.Spin();
        selfTimer = selfTimer + Time.deltaTime;
        if (selfTimer >= 1.5f)
        {
            damaged = false;
            selfTimer = 0;
            transform.localEulerAngles = originalOrientation;
        }
    }

    public void modifyTargetWaypoint(int waypointNumber)
    {
        if (currentTargetWaypoint == waypointNumber)
        {
            selectedTargetChild = Random.Range(0, 3);
            currentTargetWaypoint++;
            if (currentTargetWaypoint >= waypoints.Length)
            {
                currentTargetWaypoint = 0;
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics.SphereCast(new Ray(transform.position, -transform.up), 1f, 5);
    }

    private bool IsStraightTrackType(string name)
    {
        bool isStraight = false;

        if (name.Contains("Straight") || name.Contains("Ramp")){
            isStraight = true;
        }

        return isStraight;
    }

    public void Damage()
    {
        originalOrientation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        damaged = true;
    }

    public int CurrentTargetWaypoint
    {
        get { return currentTargetWaypoint; }
        set { currentTargetWaypoint = value; }
    }

    public IGameState GameState {
        get { return gameState; }
        set { gameState = value; }
    }

    public string TimeText {
        get { return timeText; }
        set { timeText = value; }
    }

    public bool IsDamaged {
        get { return damaged; }
        set { damaged = value; }
    }

}
