using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartPhysics : MonoBehaviour {

    public GameObject fLeftModel;
    public GameObject fRightModel;
    public GameObject rLeftModel;
    public GameObject rRightModel;
    public GameObject steering_wheel;

    private Rigidbody body;

    private float speed;
    private float turnSpeed;
    private float colliderFloor;
    private float power;
    private float turnPower;
    private float acceleration;
    private float decceleration;

    void Awake() {
        body = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start() {
        speed = 125f;
        turnSpeed = 1f;
        colliderFloor = GetComponent<BoxCollider>().bounds.extents.y;
        acceleration = .25f;
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
            speed = 125f;

            }
    
        turnPower = SimpleInput.GetAxis("Horizontal", 1);
        if (SimpleInput.GetButton("Reset Rotation", 1)) {
            ResetZRotation();
    }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            body.AddRelativeForce(0, 900, 0); // bump player kart up
        }
        if (power != 0)
        {
            fLeftModel.transform.Rotate(Vector3.right*speed);
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
            //fLeftModel.transform.Rotate(Vector3.back * 2);
            //fRightModel.transform.Rotate(Vector3.back * 2);
            //steering_wheel.transform.Rotate(Vector3.up, 1.0f); // turn handle right
        }
        else if(turnPower > 0) // turnin right
        {
            //fLeftModel.transform.Rotate(Vector3.forward * 2);
            //fRightModel.transform.Rotate(Vector3.forward * 2);
            //steering_wheel.transform.Rotate(Vector3.up, 1.0f); // turn handle right
        }
        else
        {
            // straighten wheels
            // straighten handle
        }
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -transform.up, colliderFloor + 0.25f);
    }

    bool IsFlipped() {
        return transform.eulerAngles.z > 90f;
}

    void ResetZRotation() {
        transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
    }
}
