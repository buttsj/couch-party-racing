using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingCamera : MonoBehaviour {

    public GameObject leftwheel;
    public GameObject rightwheel;
    public GameObject backlwheel;
    public GameObject backrwheel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        leftwheel.transform.Rotate(Vector3.right, 100.0f);
        rightwheel.transform.Rotate(Vector3.right, 100.0f);
        backlwheel.transform.Rotate(Vector3.right, 100.0f);
        backrwheel.transform.Rotate(Vector3.right, 100.0f);
        //transform.RotateAround(transform.parent.position, new Vector3(0, 1, 0), 10.0f * Time.deltaTime);
    }

    void FixedUpdate ()
    {
    }
}
