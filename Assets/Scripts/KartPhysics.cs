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
    private float decceleration;

    private float angle = 0.0f;

    void Awake () {
        body = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        speed = 150f;
        colliderFloor = GetComponent<BoxCollider>().bounds.extents.y;
        acceleration = .25f;
	}

    void Update()
    {
        power = Input.GetAxis("Vertical");
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
            //fLeftModel.transform.Rotate(Vector3.left);
            fLeftModel.transform.Rotate(Vector3.right * speed);
            fRightModel.transform.Rotate(Vector3.right * speed);
            rLeftModel.transform.Rotate(Vector3.right * speed);
            rRightModel.transform.Rotate(Vector3.right * speed);
            if (speed < 250f && power > 0)
            {
                speed += acceleration;
            }
            else if (speed >= 150 && power <= 0) {
                speed -= acceleration;
            }
            if (IsGrounded())
            {
                body.AddRelativeForce(0f, 0f, power * speed);
            }
            gameObject.transform.Rotate(Vector3.up, 2 * turnPower);
        }
        
        if (turnPower < 0) // turning left
        {
            if (angle > -15.0f)
            {
                fLParent.transform.Rotate(Vector3.back, 2.0f);
                fRParent.transform.Rotate(Vector3.back, 2.0f);
                angle = angle - 1.0f;
            }
            
            //steering_wheel.transform.Rotate(Vector3.up, 1.0f); // turn handle right
        }
        else if(turnPower > 0) // turnin right
        {
            if (angle < 15.0f)
            {
                fLParent.transform.Rotate(Vector3.forward, 2.0f);
                fRParent.transform.Rotate(Vector3.forward, 2.0f);
                angle = angle + 1.0f;
            }
            //steering_wheel.transform.Rotate(Vector3.up, 1.0f); // turn handle right
        }
        else if (turnPower == 0)
        {
            // straighten wheels
            // straighten handle
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
