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

    private float power;
    private float turnPower;

    void Awake () {
        body = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        speed = 200f;
        turnSpeed = 1f;
	}

    void Update()
    {
        power = Input.GetAxis("Vertical");
        turnPower = Input.GetAxis("Horizontal");
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (power != 0)
        {
            fLeftModel.transform.Rotate(speed / 60 * 360 * Time.deltaTime, 0, 0);
            fRightModel.transform.Rotate(speed / 60 * 360 * Time.deltaTime, 0, 0);
            rLeftModel.transform.Rotate(speed / 60 * 360 * Time.deltaTime, 0, 0);
            rRightModel.transform.Rotate(speed / 60 * 360 * Time.deltaTime, 0, 0);

            body.AddRelativeForce(0f, 0f, power * speed);
            gameObject.transform.Rotate(Vector3.up, turnPower);
        }
        if (turnPower < 0)
        {

        }
        else if(turnPower > 0)
        {

        }
        else
        {

        }
    }
}
