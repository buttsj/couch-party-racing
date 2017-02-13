using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpudScript : MonoBehaviour {

    private string timer;
    private bool tagged;
    private GameObject holder;
    private float invulnTimer;

    public string TimeRemaining { get { return timer; } set { timer = value; } }
    public bool IsTagged { get { return tagged; } set { tagged = value; } }
    public GameObject SpudHolder { get { return holder; } set { holder = value; } }

    public bool CanIGrab()
    {
        if (invulnTimer > 2.0f)
            return true;
        else
            return false;
    }

	// Use this for initialization
	void Start () {
        tagged = false;
        invulnTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (tagged)
        {
            invulnTimer = 0.0f;
            transform.position = holder.transform.position;
            transform.rotation = holder.transform.rotation;
        }
        else
        {
            invulnTimer = invulnTimer + Time.deltaTime;
            transform.Rotate(Vector3.up, 2.0f);
        }
	}
}
