using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {

    public GameObject PowerUpOne;
    public GameObject PowerUpTwo;
    public GameObject PowerUpThree;
    private float respawnTimerOne = 0.0f;
    private float respawnTimerTwo = 0.0f;
    private float respawnTimerThree = 0.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate ()
    {
        if (!PowerUpOne.activeSelf)
            respawnTimerOne = respawnTimerOne + Time.deltaTime;

        if (!PowerUpTwo.activeSelf)
            respawnTimerTwo = respawnTimerTwo + Time.deltaTime;

        if (!PowerUpThree.activeSelf)
            respawnTimerThree = respawnTimerThree + Time.deltaTime;

        if (respawnTimerOne >= 15.0f)
        {
            PowerUpOne.SetActive(true);
            respawnTimerOne = 0.0f;
        }

        if (respawnTimerTwo >= 15.0f)
        {
            PowerUpTwo.SetActive(true);
            respawnTimerTwo = 0.0f;
        }

        if (respawnTimerThree >= 15.0f)
        {
            PowerUpThree.SetActive(true);
            respawnTimerThree = 0.0f;
        }
    }
}
