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
    public GameObject pow_particles;

    private AudioClip boostSound;
    private bool makeBoostSound;

    private bool damaged;
    public bool IsDamaged { get { return damaged; } set { damaged = value; } }
    private bool isBoosting;
    public bool IsBoosting { get { return isBoosting; } }
    private float selfTimer;

    private float boost;
    public float Boost { get { return boost; } set { boost = value; } }

    private KartPhysics physics;
    public KartPhysics PhysicsObject { get { return physics; } }
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
    public bool IsRacingGameState { get; set; }
    public bool IsTotShotGameState { get; set; }
    private bool wasGrounded;
    private IKartAbility ability;
    public IKartAbility Ability { get { return ability; } set { ability = value; } }

    private KartAudio kartAudio;

    public void KartApplause() { kartAudio.applauseSound(); }

    private int hopLimitCounter;
    private const int HOPLIMITMAX = 2;

    void Awake()
    {
        maxSpeed = 250f;
        minSpeed = 150f;
        physics = new KartPhysics(this.gameObject, minSpeed, maxSpeed, 300f);
    }

    void Start()
    {
        boostSound = Resources.Load<AudioClip>("Sounds/KartEffects/Boosting");
        makeBoostSound = false;
        damaged = false;
        isBoosting = false;
        wasGrounded = true;
        boost = 100.0f;
        selfTimer = 0;
        ability = new NullItem(gameObject);

        kartAudio = new KartAudio(gameObject, physics, maxSpeed, minSpeed);

        hopLimitCounter = 0;
    }

    void DebugMenu()
    {
        // our debug commands
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ability = new Boost(gameObject, kartAudio);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ability = new Oil(gameObject, kartAudio);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ability = new Spark(gameObject, kartAudio);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ability = new Marble(gameObject, kartAudio);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // increase lap count
            ((RacingGameState)gameState).LapNumber++;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            damaged = true;
        }
    }

    void Update()
    {
        if (SimpleInput.GetButtonDown("Use PowerUp", playerNumber))
        {
            switch (ability.ToString())
            {
                case "Oil":
                    if (IsGrounded())
                        ability.UseItem();
                    break;
                case "Spark":
                    ability.UseItem();
                    break;
                case "Boost":
                    ability.UseItem();
                    break;
                case "Marble":
                    ability.UseItem();
                    break;
            }
        }

        handleBump();

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
        if (physics.Power != 0)
        {
            fLeftModel.transform.Rotate(Vector3.right * physics.Speed);
            fRightModel.transform.Rotate(Vector3.right * physics.Speed);
            rLeftModel.transform.Rotate(Vector3.right * physics.Speed);
            rRightModel.transform.Rotate(Vector3.right * physics.Speed);

            if (!IsOnTrack() && IsRacingGameState)
            {
                physics.MaxSpeed = 175;
            }
            else
            {
                physics.MaxSpeed = maxSpeed;
            }

            if (IsGrounded())
            {
                if (!wasGrounded) {
                    physics.Speed = physics.PreJumpSpeed;
                    wasGrounded = true;
                    physics.ApplyLandingForces();
                }
                physics.ApplyForces();
            }
            else {
                physics.PreJumpSpeed = physics.Speed;
                wasGrounded = false;
            }

        }
        physics.RotateKart(turnPower);
        RotateTires();
        
    }

    void handleBump()
    {
        if (!IsTotShotGameState && SimpleInput.GetButtonDown("Bump Kart", playerNumber) && IsGrounded())
        {
            physics.BumpKart();
        }
        else if(IsTotShotGameState && SimpleInput.GetButtonDown("Bump Kart", playerNumber) && hopLimitCounter < HOPLIMITMAX)
        {
            if (hopLimitCounter == 0)
            {
                physics.TotJump1();
                Debug.Log("Jump 1");
            }
            else
            {
                physics.TotJump2();
                Debug.Log("Jump 2");
            }
            hopLimitCounter++;
        }
        else if (IsTotShotGameState && Physics.SphereCast(new Ray(transform.position, -transform.up), 1f, 1))
        {
            hopLimitCounter = 0;
        }
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
            else if(powerup == "Marble")
            {
                ability = new Marble(gameObject, kartAudio);
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
        if (other.gameObject.name.Contains("Marble") && other.gameObject.GetComponent<MarbleManager>().validTarget(gameObject))
        {
            if (damaged == false)
            {
                damaged = true;
                originalOrientation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && ability.ToString() == "Spark")
        {
            if (ability.IsUsing() && other.transform.name.Contains("AI"))
            {
                other.gameObject.GetComponent<WaypointAI>().Damage();
            }
            else if (ability.IsUsing() && other.transform.name.Contains("Player"))
            {
                other.gameObject.GetComponent<Kart>().IsDamaged = true;
            }
        }

        gameState.OnCollisionEnter(other.gameObject);
        
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

    public bool IsGrounded()
    {
        return Physics.SphereCast(new Ray(transform.position, -transform.up), 1f, 10);
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

        kartAudio.spinOutSound();
        kartAudio.handleDamageGearingSounds();

        pow_particles.GetComponent<ParticleSystem>().Play();

        selfTimer = selfTimer + Time.deltaTime;
        if (selfTimer >= 1.5f)
        {
            pow_particles.GetComponent<ParticleSystem>().Stop();
            damaged = false;
            selfTimer = 0;
            transform.localEulerAngles = originalOrientation;
            kartAudio.SpinOutPlayed = false;
        }
    }

    void UpdateMovement() {
        if (SimpleInput.GetButton("Accelerate", playerNumber))
        {
            physics.Accelerate();
            kartAudio.handleAccelerationGearingSounds(isBoosting);
        }
        else if (SimpleInput.GetButton("Reverse", playerNumber))
        {
            physics.Reverse();
            kartAudio.handleAccelerationGearingSounds(isBoosting);
        }
        else
        {
            physics.Coast();
            kartAudio.handleCoastingGearingSounds(isBoosting);
        }
    }

    void UpdateBoost() {
        if (SimpleInput.GetButton("Boost", playerNumber))
        {
            if (boost > 0)
            {
                isBoosting = true;
                physics.StartBoost();
                BoostNoise();
                boost -= .5f;
            }
            else
            {
                makeBoostSound = false;
                isBoosting = false;
                physics.EndBoost();
            }
        }
        else
        {
            makeBoostSound = false;
            isBoosting = false;
            physics.EndBoost();
        }
    }

    void BoostNoise()
    {
        if (!makeBoostSound)
        {
            makeBoostSound = true;
            GameObject.Find("Music Manager HUD").GetComponent<AudioSource>().PlayOneShot(boostSound);
        }
    }
}
