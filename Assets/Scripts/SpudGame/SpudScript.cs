using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpudScript : MonoBehaviour {

    private float timer;
    private bool tagged;
    public float TimeRemaining { get { return timer; } set { timer = value; } }
    public bool IsTagged { get { return tagged; } set { tagged = value; } }

	// Use this for initialization
	void Start () {
        timer = 60.0f;
        tagged = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (tagged)
        {
            timer = timer - Time.deltaTime;
        }
        if (timer < 0.0f)
        {
            // game over
        }
	}
}
