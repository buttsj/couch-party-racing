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
    private float acceleration = 1f;
    private int direction;
    private float speed;

    private float previousMax;
    private float boostMax;

    public float Speed { get { return speed; } }
    public float Power { get { return power; } }
    public float MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

    public float BoostMax { get { return boostMax; } set { BoostMax = value; } }

    public KartPhysics(GameObject gameObject, float minSpeed, float maxSpeed, float boostMax) {
        kart = gameObject;
        body = kart.GetComponent<Rigidbody>();
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;
        colliderFloor = gameObject.GetComponent<BoxCollider>().bounds.extents.y;
        body.centerOfMass = new Vector3(0, -1, 0);
        speed = minSpeed;

        previousMax = maxSpeed;
        this.boostMax = boostMax;
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

    public void StartBoost() {
        previousMax = maxSpeed;
        maxSpeed = boostMax;
        acceleration = 2f;
        kart.transform.Find("LeftExhaust").gameObject.SetActive(true);
        kart.transform.Find("RightExhaust").gameObject.SetActive(true);
    }

    public void EndBoost() {
        maxSpeed = previousMax;
        if (speed > previousMax)
            speed = previousMax;
        acceleration = 1f;
        kart.transform.Find("LeftExhaust").gameObject.SetActive(false);
        kart.transform.Find("RightExhaust").gameObject.SetActive(false);
    }

}
