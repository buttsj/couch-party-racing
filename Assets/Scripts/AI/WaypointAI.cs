using UnityEngine;

public class WaypointAI : MonoBehaviour {

    private const float MINSPEED = 175f;
    private const float NORMALMAXSPEED = 250f;
    private const float BOOSTSPEED = 300f;

    private int currentTargetWaypoint;
    private int numberofWaypoints;
    private GameObject[] waypoints;
    private int selectedLane;
    private int laneCounter;
    private int laneCounterMax;

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

    private float resetTimer;
    private int resetWaypoint;

    private float breakTimer;

    private string powerup;
    private IKartAbility ability;

    public int CurrentTargetWaypoint { get { return currentTargetWaypoint; } set { currentTargetWaypoint = value; } }
    public float Boost { get { return boost; } set { boost = value; } }
    public IGameState GameState { get { return gameState; } set { gameState = value; } }
    public string TimeText { get { return timeText; } set { timeText = value; } }
    public bool IsDamaged { get { return damaged; } set { damaged = value; } }
    public int ResetWaypoint { get { return resetWaypoint; } set { resetWaypoint = value; } }
    public IKartAbility Ability { get { return ability; } set { ability = value; } }

    void Start() {

        resetTimer = 0.0f;
        breakTimer = 0.0f;
        selfTimer = 0.0f;
        currentTargetWaypoint = 0;
        selectedLane = 0;
        laneCounter = 0;
        laneCounterMax = Random.Range(4, 11);

        physics = new KartPhysics(gameObject, MINSPEED, NORMALMAXSPEED, BOOSTSPEED);

        boost = 100.0f;

        ability = new NullItem(gameObject);

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
            resetTimer = resetTimer + Time.deltaTime;
            handleAIMovement();
            handleReset();
            handlePowerup();
            handleWheelAnimation();
            if (!isBoosting)
            {
                gameObject.GetComponent<AudioSource>().pitch = ((physics.Speed - MINSPEED) / (NORMALMAXSPEED - MINSPEED)) + 1.5f;
            }
            else
            {
                gameObject.GetComponent<AudioSource>().pitch = ((physics.Speed - MINSPEED) / (NORMALMAXSPEED - MINSPEED)) + 2.0f;
            }
        }
        else
        {
            handleDamage();
            gameObject.GetComponent<AudioSource>().pitch = 1.5f;
        }

        if (isBoosting && boost > 0.0f)
        {
            boost -= 0.5f;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Power Up") && ability.ToString() == "Null")
        {
            powerup = other.gameObject.GetComponent<PowerUp>().DeterminePowerup().ToString();
            other.gameObject.SetActive(false);
            if (powerup == "Boost")
            {
                ability = new Boost(gameObject);
            }
        }

        /*
        if (other.gameObject.name.Contains("FlameCircle") && damaged == false)
        {
            damaged = true;
            originalOrientation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        */

        if (other.gameObject.name.Contains("Oil") && damaged == false)
        {
            if (!other.gameObject.GetComponent<OilManager>().Invulnerable)
            {
                damaged = true;
                originalOrientation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
                Destroy(other.gameObject);
            }
        }

    }

    void OnTriggerStay(Collider other)
    {
        /*
        if (other.gameObject.name.Contains("Conveyor"))
        {
            if (other.gameObject.GetComponent<ConveyorScript>().direction)
                physics.SlowZone(other.gameObject);
            else
                physics.FastZone(other.gameObject);
        }
        */

        if (other.gameObject.name.Contains("BoostPlate"))
        {
            physics.BoostPlate(other.gameObject);
        }

    }

    private void handleWheelAnimation()
    {
        fLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        fRightModel.transform.Rotate(Vector3.right * physics.Speed);
        rLeftModel.transform.Rotate(Vector3.right * physics.Speed);
        rRightModel.transform.Rotate(Vector3.right * physics.Speed);
    }

    private void handleAIMovement()
    {
        Vector3 targetWaypointXZPosition = new Vector3(waypoints[currentTargetWaypoint].transform.GetChild(selectedLane).position.x, 0.0f, waypoints[currentTargetWaypoint].transform.GetChild(selectedLane).position.z);
        Vector3 aiXZPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);

        Quaternion targetQuaternion = Quaternion.LookRotation((targetWaypointXZPosition - aiXZPosition).normalized);
        Quaternion slerpQuaternion = Quaternion.Slerp(transform.rotation, targetQuaternion, 0.2f);

        transform.rotation = new Quaternion(transform.rotation.x, slerpQuaternion.y, transform.rotation.z, slerpQuaternion.w);

        if (IsGrounded())
        {
            handleAIPhysics();
        }
        else
        {
            physics.EndBoost();
            isBoosting = false;
        }
    }

    private void handleAIPhysics()
    {
        int onTargetIndex = currentTargetWaypoint - 1;
        if (onTargetIndex < 0)
        {
            onTargetIndex = numberofWaypoints - 1;
        }
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

        string onTarget = waypoints[onTargetIndex].transform.parent.transform.parent.name;
        string firstTarget = waypoints[currentTargetWaypoint].transform.parent.transform.parent.name;
        string secondTarget = waypoints[secondTargetIndex].transform.parent.transform.parent.name;
        string thirdTarget = waypoints[thirdTargetIndex].transform.parent.transform.parent.name;

        if (IsStraightTrackType(firstTarget) && IsStraightTrackType(secondTarget) && IsStraightTrackType(thirdTarget) && IsStraightTrackType(onTarget) && boost > 0.0f)
        {
            physics.StartBoost();
            isBoosting = true;
        }
        else
        {
            physics.EndBoost();
            isBoosting = false;
        }

        if (breakTimer > 0.0f)
        {
            physics.Coast();
            breakTimer = breakTimer - Time.deltaTime;
        }
        else
        {
            physics.Accelerate();
        }
        physics.ApplyForces();
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

    private void handleReset()
    {
        if (resetTimer >= 6.0f)
        {
            gameState.ResetKart();
            currentTargetWaypoint = resetWaypoint;
            resetTimer = 0.0f;
        }
    }

    private void handlePowerup()
    {
        if (ability.ToString() == "Boost" && boost <= 0.0f)
        {
            ability.UseItem();
        }
        if (ability.IsUsed())
        {
            ability = new NullItem(gameObject);
        }
        ability.Update();
    }

    public void modifyTargetWaypoint(int waypointNumber)
    {
        if (currentTargetWaypoint == waypointNumber)
        {
            resetTimer = 0.0f;
            laneCounter++;
            if (laneCounter >= laneCounterMax)
            {
                selectedLane = Random.Range(0, 3);
                laneCounter = 0;
            }

            currentTargetWaypoint++;
            if (currentTargetWaypoint >= waypoints.Length)
            {
                currentTargetWaypoint = 0;
            }

            breakForTurn();
        }
    }

    private void breakForTurn()
    {
        int onTargetIndex = currentTargetWaypoint - 1;
        int previousTargetIndex = currentTargetWaypoint - 2;
        if (onTargetIndex < 0)
        {
            onTargetIndex = numberofWaypoints - 1;
            previousTargetIndex = numberofWaypoints - 2;
        }
        else if (previousTargetIndex < 0)
        {
            previousTargetIndex = numberofWaypoints - 1;
        }
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
        string onTarget = waypoints[onTargetIndex].transform.parent.transform.parent.name;
        string previousTarget = waypoints[previousTargetIndex].transform.parent.transform.parent.name;
        string firstTarget = waypoints[currentTargetWaypoint].transform.parent.transform.parent.name;
        string secondTarget = waypoints[secondTargetIndex].transform.parent.transform.parent.name;
        string thirdTarget = waypoints[thirdTargetIndex].transform.parent.transform.parent.name;
        if (previousTarget.Contains("Turn") || onTarget.Contains("Turn") || firstTarget.Contains("Turn") || secondTarget.Contains("Turn") || thirdTarget.Contains("Turn"))
        {
            breakTimer = 0.2f;
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

}
