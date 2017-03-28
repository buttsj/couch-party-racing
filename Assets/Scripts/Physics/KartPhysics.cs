using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartPhysics {
    GameObject kart;
    Rigidbody body;
    private float minSpeed;
    private float maxSpeed;
    private float power;
    private float acceleration = 1f;
    private int direction;
    private float speed;

    private const float flipForce = 1200;
    public float previousMax;
    private float boostMax;

    public float Speed { get { return speed; } set { speed = value; } }
    public float Power { get { return power; } }
    public float MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }
    public float PreJumpSpeed { get; set; }
    public float TurningSpeed { get; set; }
    public bool IsFlipping { get { return Mathf.Abs(body.angularVelocity.x) > 1 || Mathf.Abs(body.angularVelocity.z) > 1; } }
    public float BoostMax { get { return boostMax; } set { BoostMax = value; } }

    public KartPhysics(GameObject gameObject, float minSpeed, float maxSpeed, float boostMax) {
        kart = gameObject;
        body = kart.GetComponent<Rigidbody>();
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;
        body.centerOfMass = new Vector3(0, -1, 0);
        speed = minSpeed;

        previousMax = maxSpeed;
        this.boostMax = boostMax;
    }

    public void Accelerate() {
        power = 1;
        if (speed < maxSpeed)
            speed += acceleration;
        else
            speed = maxSpeed;
    }

    public void Reverse() {
        power = -1;
        speed = minSpeed;
    }

    public void Coast() {
        power = 0;
        if (speed > minSpeed)
            speed--;
    }

    public void RotateKart(float turnDirection) {
        kart.transform.Rotate(Vector3.up, TurningSpeed * turnDirection);
    }

    public void ApplyForces() {
        body.AddRelativeForce(0f, 0f, power * speed);
    }

    public void ApplyLandingForces() {
        body.AddRelativeForce(0f, 0f, 4*power * speed);
    }

    public void BumpKart() {
        body.AddRelativeForce(0, 1000, 0);
    }

    public void TotJump1()
    {
        body.AddRelativeForce(0, 2000, 0);
    }

    public void TotJump2()
    {
        body.AddRelativeForce(0, 3000, 0);
    }


    public void Spin()
    {
        kart.transform.Rotate(Vector3.up, 20.0f);
    }

    public void SlowZone(GameObject other) {
        body.AddForce(other.transform.forward * 150f);
    }

    public void FastZone(GameObject other) {
        body.AddForce(other.transform.forward * -150f);
    }

    public void BoostPlate(GameObject other)
    {
        body.AddForce(other.transform.forward * -450f);
    }

    public void StartBoost() {
        maxSpeed = boostMax;
        acceleration = 2f;
        kart.transform.Find("LeftExhaust").gameObject.SetActive(true);
        kart.transform.Find("RightExhaust").gameObject.SetActive(true);
    }

    public void RocketBoost() {
        body.AddRelativeForce(0f, 0f, 250);
    }

    public void EndBoost() {
        maxSpeed = previousMax;
        if (speed > previousMax)
            speed = previousMax;
        acceleration = 1f;
        kart.transform.Find("LeftExhaust").gameObject.SetActive(false);
        kart.transform.Find("RightExhaust").gameObject.SetActive(false);
    }

    public void BackFlip() {
        body.AddForceAtPosition(flipForce*Vector3.up, kart.transform.position + 2*kart.transform.forward);
        body.AddForceAtPosition(-flipForce * Vector3.up, kart.transform.position - 2 * kart.transform.forward);
    }

    public void FrontFlip()
    {
        body.AddForceAtPosition(-flipForce * Vector3.up, kart.transform.position + 2 * kart.transform.forward);
        body.AddForceAtPosition(flipForce * Vector3.up, kart.transform.position - 2 * kart.transform.forward);
    }

    public void RightRoll() {
        body.AddForceAtPosition(-flipForce * Vector3.up, kart.transform.position + 2 * kart.transform.right);
        body.AddForceAtPosition(flipForce * Vector3.up, kart.transform.position - 2 * kart.transform.right);
    }

    public void LeftRoll()
    {
        body.AddForceAtPosition(flipForce * Vector3.up, kart.transform.position + 2 * kart.transform.right);
        body.AddForceAtPosition(-flipForce * Vector3.up, kart.transform.position - 2 * kart.transform.right);
    }

}
