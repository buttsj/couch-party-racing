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
        speed = 100f;
        turnSpeed = .9f;
	}

    void Update()
    {
        power = Input.GetAxis("Vertical") * speed;
        turnPower = Input.GetAxis("Horizontal") * turnSpeed;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (power != 0)
        {
            body.AddRelativeForce(0f, 0f, power);
            gameObject.transform.Rotate(Vector3.up, turnPower);
        }
    }
}
