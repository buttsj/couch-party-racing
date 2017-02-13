using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kart : MonoBehaviour {

    public GameObject fLeftModel;
    public GameObject fRightModel;
    public GameObject rLeftModel;
    public GameObject rRightModel;
    public GameObject steering_wheel;

    public GameObject fLParent;
    public GameObject fRParent;

    private bool damaged;
    public bool IsDamaged { get { return damaged; } set { damaged = value; } }
    private float selfTimer;

    private KartPhysics physics;
    private float turnPower;
    private float angle = 0.0f;
    private float boost;
    private string powerup;
    private int lapNumber;
    private int playerNumber;
    private string timeText;
    private bool holdingPotato;
    public int PlayerNumber { get { return playerNumber; } set { playerNumber = value; }  }
    public float Boost { get { return boost; } set { boost = value; } }
    public int LapNumber { get { return lapNumber; } }
    public string Powerup { get { return powerup; } set { powerup = value; } }

    public string TimeText { get { return timeText; } set { timeText = value; } }

    void Start() {
        physics = new KartPhysics(this.gameObject, 150f, 250f, 300f);
        damaged = false;
        boost = 100.0f;
        lapNumber = 0;
        selfTimer = 0;
        holdingPotato = false;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            damaged = true;
        }
        if (!damaged) {
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

            if (SimpleInput.GetButton("Boost", playerNumber))
            {
                if (boost > 0)
                {
                    physics.StartBoost();
                    boost -= .5f;
                }
                else
                {
                    physics.EndBoost();
                }
            }
                else
                {
                physics.EndBoost();
            }
            turnPower = SimpleInput.GetAxis("Horizontal", playerNumber);
            if (SimpleInput.GetButton("Reset Rotation", playerNumber))
            {
                ResetRotation();
            }
        }
        else
        {
            if (holdingPotato)
            {
                // drop potato
                holdingPotato = false;
                GameObject.FindGameObjectWithTag("Potato").GetComponent<SpudScript>().SpudHolder = null;
                GameObject.FindGameObjectWithTag("Potato").GetComponent<SpudScript>().IsTagged = false;
            }

            physics.Spin();
            selfTimer = selfTimer + Time.deltaTime;
            if (selfTimer >= 4.0f)
            {
                damaged = false;
                selfTimer = 0;
            }
        }
    }

    void FixedUpdate () {
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

            if (IsGrounded())
            {
                physics.ApplyForces();
            }       
               
        }

        physics.RotateKart(turnPower);
        
        if (turnPower < 0) // turning left
        {
            if (angle > -15.0f && physics.Power > 0)
            {
                fLParent.transform.Rotate(Vector3.back, 2.0f);
                fRParent.transform.Rotate(Vector3.back, 2.0f);
                angle = angle - 1.0f;
            }
            else if (angle < 15f && physics.Power < 0) {
                fLParent.transform.Rotate(Vector3.forward, 2.0f);
                fRParent.transform.Rotate(Vector3.forward, 2.0f);
                angle = angle + 1.0f;
            }
            
            //steering_wheel.transform.Rotate(Vector3.up, 1.0f); // turn handle right
        }
        else if(turnPower > 0) // turning right
        {
            if (angle < 15.0f && physics.Power > 0 )
            {
                fLParent.transform.Rotate(Vector3.forward, 2.0f);
                fRParent.transform.Rotate(Vector3.forward, 2.0f);
                angle = angle + 1.0f;
            }
            else if (angle > - 15f && physics.Power < 0) {
                fLParent.transform.Rotate(Vector3.back, 2.0f);
                fRParent.transform.Rotate(Vector3.back, 2.0f);
                angle = angle - 1.0f;
            }
            //steering_wheel.transform.Rotate(Vector3.up, 1.0f); // turn handle right
        }
        else if (turnPower == 0)
        {
            // straighten wheels
            // straighten handle

            if (angle < 0 )
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Power Up"))
        {
            powerup = other.gameObject.GetComponent<PowerUp>().DeterminePowerup().ToString();
            Debug.Log("PICKED UP: " + powerup);
            other.gameObject.SetActive(false);
            if (powerup == "Boost") {
                boost = 100;
            }
        }
        if (other.gameObject.name.Contains("Finish")) {
            lapNumber++;
        }
        if (other.gameObject.CompareTag("Potato"))
        {
            
            Debug.Log("hit potato");
            if (other.gameObject.GetComponent<SpudScript>().CanIGrab())
            {
                other.gameObject.GetComponent<SpudScript>().SpudHolder = gameObject;
                other.gameObject.GetComponent<SpudScript>().IsTagged = true;
                holdingPotato = true;
            }
            else
                Debug.Log("can't grab potato yet");
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
    }

    bool IsGrounded() {
        return Physics.SphereCast(new Ray(transform.position, -transform.up), 1f, 5);
    }

    bool IsFlipped() {
        return transform.eulerAngles.z > 90f;
}

    void ResetRotation() {
        transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
    }
}
