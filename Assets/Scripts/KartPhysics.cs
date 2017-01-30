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

    void Awake () {
        body = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        speed = 150f;
        turnSpeed = 1f;
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
            fLeftModel.transform.Rotate(speed / 60 * 360 * Time.deltaTime, 0, 0);
            fRightModel.transform.Rotate(speed / 60 * 360 * Time.deltaTime, 0, 0);
            rLeftModel.transform.Rotate(speed / 60 * 360 * Time.deltaTime, 0, 0);
            rRightModel.transform.Rotate(speed / 60 * 360 * Time.deltaTime, 0, 0);
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
        
        if (turnPower < 0)
        {
            // turn wheels left
            //steering_wheel.transform.Rotate(Vector3.up, -1.0f); // turn handle left
        }
        else if(turnPower > 0)
        {
            // turn wheels right
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
