﻿using System.Collections;
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

    private KartPhysics physics;
    private float turnPower;
    private float angle = 0.0f;
    private int playerNumber;
    public int PlayerNumber { get { return playerNumber; } set { playerNumber = value; }  }


    // Use this for initialization
    void Start() {
        physics = new KartPhysics(gameObject, 150f, 250f);
        playerNumber = 1;
	}

    void Update() { 
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
    
        turnPower = SimpleInput.GetAxis("Horizontal", playerNumber);
        if (SimpleInput.GetButton("Reset Rotation", playerNumber)) {
            ResetRotation();
    }
    }

    // Update is called once per frame
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
            string powerup = other.gameObject.GetComponent<PowerUp>().DeterminePowerup().ToString();
            Debug.Log("PICKED UP: " + powerup);
            other.gameObject.SetActive(false);
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
