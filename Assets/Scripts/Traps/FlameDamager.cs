using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDamager : MonoBehaviour {

    ParticleSystem flameEmitter;

	// Use this for initialization
	void Start () {
		flameEmitter = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Kart") || flameEmitter.isEmitting)
        {
            //need to somehow access kart to damage
        }
    }
}
