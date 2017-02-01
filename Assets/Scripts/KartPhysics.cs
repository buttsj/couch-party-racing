using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartPhysics : MonoBehaviour {

    public GameObject fLeftModel;
    public GameObject fRightModel;
    public GameObject rLeftModel;
    public GameObject rRightModel;
    public GameObject steering_wheel;

    public GameObject fLParent;
    public GameObject fRParent;
    
    private Rigidbody body;

    private float speed;
    private float colliderFloor;
    private float power;
    private float turnPower;
    private float acceleration;
    private float angle = 0.0f;

    void Awake () {
        body = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start() {
        speed = 150f;
        colliderFloor = GetComponent<BoxCollider>().bounds.extents.y;
        acceleration = 1f;
        body.centerOfMass = new Vector3(0, -1, 0);
	}

    void Update() { 
        if (SimpleInput.GetButton("Accelerate", 1))
    {
                power = 1;
                if(speed< 250f)
                    speed += acceleration;
            }
            else if (SimpleInput.GetButton("Reverse", 1))
            {
                power = -1;
                speed = 150f;
            }
            else
            {
                power = 0;
            speed = 150f;

            }
    
        turnPower = SimpleInput.GetAxis("Horizontal", 1);
        if (SimpleInput.GetButton("Reset Rotation", 1)) {
            ResetRotation();
    }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            body.AddRelativeForce(0, 1000, 0); // bump player kart up
        }
        if (power != 0)
        {
            //fLeftModel.transform.Rotate(Vector3.left);
            fLeftModel.transform.Rotate(Vector3.right * speed);
            fRightModel.transform.Rotate(Vector3.right * speed);
            rLeftModel.transform.Rotate(Vector3.right * speed);
            rRightModel.transform.Rotate(Vector3.right * speed);

            if (IsGrounded())
            {
                body.AddRelativeForce(0f, 0f, power * speed);

            }
            
        }

        if (body.velocity.sqrMagnitude > 0) {
            gameObject.transform.Rotate(Vector3.up, 2 * turnPower);
        }
        
        if (turnPower < 0) // turning left
        {
            if (angle > -15.0f && power > 0)
            {
                fLParent.transform.Rotate(Vector3.back, 2.0f);
                fRParent.transform.Rotate(Vector3.back, 2.0f);
                angle = angle - 1.0f;
            }
            else if (angle < 15f && power < 0) {
                fLParent.transform.Rotate(Vector3.forward, 2.0f);
                fRParent.transform.Rotate(Vector3.forward, 2.0f);
                angle = angle + 1.0f;
            }
            
            //steering_wheel.transform.Rotate(Vector3.up, 1.0f); // turn handle right
        }
        else if(turnPower > 0) // turning right
        {
            if (angle < 15.0f && power > 0 )
            {
                fLParent.transform.Rotate(Vector3.forward, 2.0f);
                fRParent.transform.Rotate(Vector3.forward, 2.0f);
                angle = angle + 1.0f;
            }
            else if (angle > - 15f && power < 0) {
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

    bool IsGrounded() {
        return Physics.SphereCast(new Ray(transform.position, -transform.up), 1f, 5);
    }



    bool IsFlipped() {
        return transform.eulerAngles.z > 90f;
}

    void ResetRotation() {
        transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Power Up"))
        {
            string powerup = other.gameObject.GetComponent<PowerUp>().DeterminePowerup().ToString();
            Debug.Log("PICKED UP: " + powerup);
            other.gameObject.SetActive(false);
        }
    }

   


}
