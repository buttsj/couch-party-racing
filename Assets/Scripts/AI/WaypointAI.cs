using UnityEngine;

public class WaypointAI : MonoBehaviour {

    private const float MAXBREAKTIMER = 0.14f;

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

    public GameObject shield_particle;

    private string timeText;
    private IGameState gameState;

    private bool damaged;
    private float selfTimer;
    private Vector3 originalOrientation;

    private bool isBoosting;

    private float resetTimer;
    private int resetWaypoint;

    private float breakTimer;
    private float breakTimerMax;

    private string powerup;
    private IKartAbility ability;
    private float powerUpTimer;
    private float powerUpTimeLimit;
    private float destroyedAnimationTimer;

    private KartAudio kartAudio;

    private bool canSkip;

    private bool justLaidPowerup;

    private bool isInvulnerable;

    public bool Destroyed { get; set; }
    public int CurrentTargetWaypoint { get { return currentTargetWaypoint; } set { currentTargetWaypoint = value; } }
    public float Boost { get { return boost; } set { boost = value; } }
    public IGameState GameState { get { return gameState; } set { gameState = value; } }
    public string TimeText { get { return timeText; } set { timeText = value; } }
    public bool IsDamaged { get { return damaged; } set { damaged = value; } }
    public int ResetWaypoint { get { return resetWaypoint; } set { resetWaypoint = value; } }
    public IKartAbility Ability { get { return ability; } set { ability = value; } }
    public float MAXBREAKTIME { set { breakTimerMax = value; } }
    public KartPhysics PhysicsObject { get { return physics; } set { physics = value; } }
    public bool IsBoosting { get { return isBoosting; } set { isBoosting = value; } }
    public bool IsInvulnerable { get { return isInvulnerable; } set { isInvulnerable = value; } }

    void Start() {

        isInvulnerable = false;

        resetTimer = 0.0f;
        breakTimer = 0.0f;
        breakTimerMax = MAXBREAKTIMER;
        selfTimer = 0.0f;
        currentTargetWaypoint = 0;

        laneCounter = 0;
        laneCounterMax = Random.Range(4, 11);

        physics = new KartPhysics(gameObject, MINSPEED, NORMALMAXSPEED, BOOSTSPEED);

        boost = 100.0f;

        ability = new NullItem(gameObject);

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        numberofWaypoints = waypoints.Length;

        kartAudio = new KartAudio(gameObject, physics, NORMALMAXSPEED, MINSPEED);

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
        canSkip = true;
        isBoosting = false;

        powerUpTimer = 0.0f;
        powerUpTimeLimit = Random.Range(3, 13);

        float closestLaneDistance = Vector3.Distance(transform.position, waypoints[currentTargetWaypoint].transform.GetChild(0).transform.position);
        selectedLane = 0;
        for(int i = 1; i < 3; i++)
        {
            if(Vector3.Distance(transform.position, waypoints[currentTargetWaypoint].transform.GetChild(i).transform.position) < closestLaneDistance)
            {
                closestLaneDistance = Vector3.Distance(transform.position, waypoints[currentTargetWaypoint].transform.GetChild(i).transform.position);
                selectedLane = i;
            }
        }

        justLaidPowerup = false;
    }

    void Update()
    {
        if (Destroyed)
        {
            destroyedAnimationTimer += Time.deltaTime;
            if (destroyedAnimationTimer >= 1.5f)
            {
                Destroyed = false;
                damaged = false;
                transform.Find("ExplosionEffect").gameObject.SetActive(false);
                destroyedAnimationTimer = 0;
                immediateReset();
                ToggleRenderers(true);
            }
        }
    }
    void FixedUpdate() {
        if (Time.timeScale > 0)
        {
            gameObject.transform.Translate(0, 0.015f, 0);
        }
        if (!isOnTrack() && canSkip && IsGrounded())
        {
            if(waypoints[currentTargetWaypoint].GetComponent<Checkpoint>() != null)
            {
                waypoints[currentTargetWaypoint].GetComponent<Checkpoint>().checkpointSkip(gameObject);
            }
            currentTargetWaypoint++;
            if(currentTargetWaypoint >= numberofWaypoints)
            {
                currentTargetWaypoint = 0;
            }
            canSkip = false;
        }

        if (!damaged)
        {
            resetTimer = resetTimer + Time.deltaTime;
            handleAIMovement();
            handleReset();
            handlePowerup();
            handleWheelAnimation();
        }
        else
        {
            handleDamage();
        }

        if (isBoosting && boost > 0.0f)
        {
            boost -= 0.5f;
        }

    }

    private void LateUpdate()
    {
        justLaidPowerup = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Power Up") && ability.ToString() == "Null")
        {
            powerup = other.gameObject.GetComponent<PowerUp>().DeterminePowerup(kartAudio).ToString();
            other.gameObject.SetActive(false);
            if (powerup == "Boost")
            {
                ability = new Boost(gameObject, kartAudio);
            }
            else if (powerup == "Oil")
            {
                ability = new Oil(gameObject, kartAudio);
            }
            else if (powerup == "Spark")
            {
                ability = new Spark(gameObject, kartAudio);
            }
            else if (powerup == "Marble")
            {
                ability = new Marble(gameObject, kartAudio);
            }
            else if (powerup == "Shield")
            {
                ability = new Shield(gameObject, kartAudio);
            }
        }

        if (other.gameObject.name.Contains("Oil") && damaged == false && !justLaidPowerup && !isInvulnerable)
        {
            if (!other.gameObject.GetComponent<OilManager>().Invulnerable)
            {
                damaged = true;
                originalOrientation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.name.Contains("Marble") && other.gameObject.GetComponent<MarbleManager>().validTarget(gameObject))
        {
            if (!isInvulnerable)
            {
                if (damaged == false)
                {
                    damaged = true;
                    originalOrientation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
                }
            }
            Destroy(other.gameObject);
        }
        if (other.tag == "DeathObject")
        {
            ToggleRenderers(false);
            transform.Find("ExplosionEffect").gameObject.SetActive(true);
            Destroyed = true;
            damaged = true;
        }

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && ability.ToString() == "Spark")
        {
            if (ability.IsUsing() && other.transform.name.Contains("AI") && !other.gameObject.GetComponent<WaypointAI>().IsInvulnerable)
            {
                other.gameObject.GetComponent<WaypointAI>().Damage();
            }
            else if (ability.IsUsing() && other.transform.name.Contains("Player") && !other.gameObject.GetComponent<Kart>().IsInvulnerable)
            {
                other.gameObject.GetComponent<Kart>().IsDamaged = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
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
            kartAudio.handleCoastingGearingSounds(isBoosting);
            breakTimer = breakTimer - Time.deltaTime;
        }
        else
        {
            physics.Accelerate();
            kartAudio.handleAccelerationGearingSounds(isBoosting);
        }
        physics.ApplyForces();
    }

    private void handleDamage()
    {
        physics.Spin();

        kartAudio.spinOutSound();
        kartAudio.handleDamageGearingSounds();

        selfTimer = selfTimer + Time.deltaTime;
        if (selfTimer >= 1.5f)
        {
            damaged = false;
            selfTimer = 0;
            transform.localEulerAngles = originalOrientation;
            kartAudio.SpinOutPlayed = false;
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

    private void immediateReset()
    {
        gameState.ResetKart();
        currentTargetWaypoint = resetWaypoint;
    }

    private void handlePowerup()
    {
        if (ability.ToString() == "Boost" && boost <= 49.9f)
        {
            ability.UseItem();
        }
        else if(ability.ToString() == "Oil")
        {
            if(powerUpTimer >= powerUpTimeLimit && IsGrounded())
            {
                ability.UseItem();
                justLaidPowerup = true;
                powerUpTimeLimit = Random.Range(3, 13);
            }
            else
            {
                powerUpTimer = powerUpTimer + Time.deltaTime;
            }
        }
        else if (ability.ToString() == "Spark")
        {
            if (powerUpTimer >= powerUpTimeLimit)
            {
                ability.UseItem();
                powerUpTimeLimit = Random.Range(3, 13);
            }
            else
            {
                powerUpTimer = powerUpTimer + Time.deltaTime;
            }
        }
        else if (ability.ToString() == "Marble")
        {
            if (powerUpTimer >= powerUpTimeLimit)
            {
                ability.UseItem();
                powerUpTimeLimit = Random.Range(3, 13);
            }
            else
            {
                powerUpTimer = powerUpTimer + Time.deltaTime;
            }
        }
        else if (ability.ToString() == "Shield")
        {
            if (powerUpTimer >= powerUpTimeLimit)
            {
                ability.UseItem();
                powerUpTimeLimit = Random.Range(3, 13);
            }
            else
            {
                powerUpTimer = powerUpTimer + Time.deltaTime;
            }
        }

        if (ability.IsUsed())
        {
            ability = new NullItem(gameObject);
            powerUpTimer = 0.0f;
        }

        ability.Update();
    }

    public void modifyTargetWaypoint(int waypointNumber)
    {
        if (currentTargetWaypoint == waypointNumber)
        {
            canSkip = true;
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
            breakTimer = breakTimerMax;
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

    private bool isOnTrack()
    {
        RaycastHit[] info = Physics.RaycastAll(transform.position - Vector3.up, -transform.up, 250f);
        bool onTrack = false;
        foreach (RaycastHit hit in info)
        {
            if (hit.collider.gameObject.CompareTag("Track"))
            {
                onTrack = true;
            }
        }

        return onTrack;
    }

    public void ToggleRenderers(bool toggle)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.transform.GetComponent<ParticleSystem>() == null)
            {
                r.enabled = toggle;
            }
        }
    }
}
