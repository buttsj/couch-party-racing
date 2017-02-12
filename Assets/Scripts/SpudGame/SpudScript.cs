using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpudScript : MonoBehaviour {

    private float timer;
    private bool tagged;
    private GameObject holder;

    public float TimeRemaining { get { return timer; } set { timer = value; } }
    public bool IsTagged { get { return tagged; } set { tagged = value; } }
    public GameObject SpudHolder { get { return holder; } set { holder = value; } }

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
            transform.position = holder.transform.position;
            transform.rotation = holder.transform.rotation;
        }
        else
        {
            transform.Rotate(Vector3.up, 2.0f);
        }
        if (timer < 0.0f)
        {
            // game over
        }
	}
}
