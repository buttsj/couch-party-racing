using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartPhysics {
    GameObject kart;
    Rigidbody body;
    private float minSpeed;
    private float maxSpeed;
    private float colliderFloor;
    private float power;
    private const float acceleration = 1f;
    private int direction;
    private float speed;
    public float Speed { get { return speed; } }
    public float Power { get { return power; } }
    public KartPhysics(GameObject gameObject, float min, float max) {
        kart = gameObject;
        body = kart.GetComponent<Rigidbody>();
        minSpeed = min;
        maxSpeed = max;
        colliderFloor = gameObject.GetComponent<BoxCollider>().bounds.extents.y;
        body.centerOfMass = new Vector3(0, -1, 0);
        speed = minSpeed;
    }

    public void Accelerate() {
        power = 1;
        if (speed < maxSpeed)
            speed += acceleration;
    }

    public void Reverse() {
        power = -1;
        speed = minSpeed;
    }

    public void Coast() {
        power = 0;
        speed = minSpeed;
    }

    public void RotateKart(float turnDirection) {
        kart.transform.Rotate(Vector3.up, 2 * turnDirection);
    }

    public void ApplyForces() {
        body.AddRelativeForce(0f, 0f, power * speed);
    }

    public void BumpKart() {
        body.AddRelativeForce(0, 1000, 0);
    }
}
