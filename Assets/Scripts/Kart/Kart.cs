using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kart : MonoBehaviour
{

    public GameObject fLeftModel;
    public GameObject fRightModel;
    public GameObject rLeftModel;
    public GameObject rRightModel;
    public GameObject steering_wheel;

    public GameObject fLParent;
    public GameObject fRParent;

    public GameObject green_arrow;

    private bool damaged;
    public bool IsDamaged { get { return damaged; } set { damaged = value; } }
    private bool isBoosting;
    public bool IsBoosting { get { return isBoosting; } }
    private float selfTimer;

    private float boost;
    public float Boost { get { return boost; } set { boost = value; } }

    private KartPhysics physics;
    private float turnPower;
    private float angle = 0.0f;
    private float maxSpeed;
    private float minSpeed;
    private string powerup;
    private Vector3 originalOrientation;
    private int playerNumber;
    public int PlayerNumber { get { return playerNumber; } set { playerNumber = value; } }
    private string timeText;
    public string TimeText { get { return timeText; } set { timeText = value; } }
    public string Powerup { get { return powerup; } set { powerup = value; } }

    private IGameState gameState;
    public IGameState GameState { get { return gameState; } set { gameState = value; } }

    private IKartAbility ability;
    public IKartAbility Ability { get { return ability; } set { ability = value; } }

    

    void Start()
    {
        damaged = false;
        isBoosting = false;
        boost = 100.0f;
        selfTimer = 0;
        ability = new NullItem(gameObject);
    }

    void Awake()
    {
        maxSpeed = 250f;
        minSpeed = 150f;
        physics = new KartPhysics(this.gameObject, minSpeed, maxSpeed, 300f);
    }

    void DebugMenu()
    {
        // our debug commands
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ability = new Boost(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ability = new Oil(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ability = new Spark(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            damaged = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ability.UseItem();
        }
        if (ability.IsUsed())
        {
            ability = new NullItem(gameObject); // item is completely used
        }
        ability.Update();
        DebugMenu(); // check for Debug Commands

        if (!damaged)
        {
            NonDamagedUpdate();
        }
        else
        {
            DamagedUpdate();
        }
    }

    void FixedUpdate()
    {
        if (SimpleInput.GetButtonDown("Bump Kart", playerNumber) && IsGrounded())
        {
            physics.BumpKart();
        }

        if (physics.Power != 0)
        {
            fLeftModel.transform.Rotate(Vector3.right * physics.Speed);
            fRightModel.transform.Rotate(Vector3.right * physics.Speed);
            rLeftModel.transform.Rotate(Vector3.right * physics.Speed);
            rRightModel.transform.Rotate(Vector3.right * physics.Speed);

            if (!IsOnTrack())
            {
                physics.ApplyCarpetFriction();
            }
            else
            {
                physics.MaxSpeed = maxSpeed;
            }

            if (IsGrounded())
            {
                physics.ApplyForces();
            }

        }

        physics.RotateKart(turnPower);
        RotateTires();
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
                Debug.Log("Picked up Boost");
            }
            else if (powerup == "Oil")
            {
                ability = new Oil(gameObject);
                Debug.Log("Picked up Oil");
            }
            else if (powerup == "Spark")
            {
                ability = new Spark(gameObject);
                Debug.Log("Picked up Spark");
            }
        }
        if (other.gameObject.CompareTag("Potato"))
        {
            Debug.Log("hit potato");
            if (other.gameObject.GetComponent<SpudScript>().CanIGrab())
            {
                other.gameObject.GetComponent<SpudScript>().SpudHolder = gameObject;
                other.gameObject.GetComponent<SpudScript>().IsTagged = true;
                ((SpudRunGameState)gameState).HoldingPotato = true;
            }
            else
                Debug.Log("can't grab potato yet");
        }
        if (other.gameObject.name.Contains("FlameCircle") && damaged == false)
        {
            damaged = true;
            originalOrientation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
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

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && ability.ToString() == "Spark")
        {
            other.gameObject.GetComponent<WaypointAI>().Damage();
            // damage the other kart
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Conveyor"))
        {
            // push the kart by the conveyor belt
            if (other.gameObject.GetComponent<ConveyorScript>().direction)
                physics.SlowZone(other.gameObject);
            else
                physics.FastZone(other.gameObject);
        }
        if (other.gameObject.name.Contains("BoostPlate"))
        {
            physics.BoostPlate(other.gameObject);
        }
    }

    bool IsGrounded()
    {
        return Physics.SphereCast(new Ray(transform.position, -transform.up), 1f, 5);
    }

    bool IsOnTrack()
    {
        RaycastHit[] info = Physics.RaycastAll(transform.position - Vector3.up, -transform.up, 1f);
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


    bool IsFlipped()
    {
        return transform.eulerAngles.z > 90f;
    }

    void ResetKart()
    {
        gameState.ResetKart();
    }

    void RotateTires()
    {
        if (turnPower < 0) // turning left
        {
            if (angle > -15.0f && physics.Power > 0)
            {
                fLParent.transform.Rotate(Vector3.back, 2.0f);
                fRParent.transform.Rotate(Vector3.back, 2.0f);
                angle = angle - 1.0f;
            }
            else if (angle < 15f && physics.Power < 0)
            {
                fLParent.transform.Rotate(Vector3.forward, 2.0f);
                fRParent.transform.Rotate(Vector3.forward, 2.0f);
                angle = angle + 1.0f;
            }
        }
        else if (turnPower > 0) // turning right
        {
            if (angle < 15.0f && physics.Power > 0)
            {
                fLParent.transform.Rotate(Vector3.forward, 2.0f);
                fRParent.transform.Rotate(Vector3.forward, 2.0f);
                angle = angle + 1.0f;
            }
            else if (angle > -15f && physics.Power < 0)
            {
                fLParent.transform.Rotate(Vector3.back, 2.0f);
                fRParent.transform.Rotate(Vector3.back, 2.0f);
                angle = angle - 1.0f;
            }
        }
        else if (turnPower == 0)
        {
            if (angle < 0)
            {
                fLParent.transform.Rotate(Vector3.forward, 2.0f);
                fRParent.transform.Rotate(Vector3.forward, 2.0f);
                angle = angle + 1.0f;
            }
            else if (angle > 0)
            {
                fLParent.transform.Rotate(Vector3.back, 2.0f);
                fRParent.transform.Rotate(Vector3.back, 2.0f);
                angle = angle - 1.0f;
            }
        }
    }

    void NonDamagedUpdate()
    {
        UpdateMovement();
        UpdateBoost();
        gameState.NonDamagedUpdate();
        turnPower = SimpleInput.GetAxis("Horizontal", playerNumber);
        if (SimpleInput.GetButton("Reset Rotation", playerNumber))
        {
            ResetKart();
        }
    }

    void DamagedUpdate() {
        if (isBoosting)
        {
            isBoosting = false;
            physics.EndBoost();
        }
        gameState.DamagedUpdate();

        physics.Spin();
        selfTimer = selfTimer + Time.deltaTime;
        if (selfTimer >= 1.5f)
        {
            damaged = false;
            selfTimer = 0;
            transform.localEulerAngles = originalOrientation;
        }
    }

    void UpdateMovement() {
        if (SimpleInput.GetButton("Accelerate", playerNumber))
        {
            physics.Accelerate();
        }
        else if (SimpleInput.GetButton("Reverse", playerNumber))
        {
            physics.Reverse();
        }
        else
        {
            physics.Coast();

        }
    }

    void UpdateBoost() {
        if (SimpleInput.GetButton("Boost", playerNumber))
        {
            if (boost > 0)
            {
                isBoosting = true;
                physics.StartBoost();
                boost -= .5f;
            }
            else
            {
                isBoosting = false;
                physics.EndBoost();
            }
        }
        else
        {
            isBoosting = false;
            physics.EndBoost();
        }
    }
}
