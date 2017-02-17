using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilManager : MonoBehaviour {

    private float timer;

	// Use this for initialization
	void Start () {
        timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        timer = timer + Time.deltaTime;
        if (timer > 5.0f)
        {
            Destroy(gameObject);
        }
	}
}
