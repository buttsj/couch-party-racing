using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameScript : MonoBehaviour {

    public GameObject flameTrapOne;
    public GameObject flameTrapTwo;
    public GameObject flameTrapThree;
    public GameObject flameTrapFour;

    public bool oneOn;
    public bool twoOn;
    public bool threeOn;
    public bool fourOn;

    private float timer;

    // Use this for initialization
    void Start () {
        oneOn = true;
        twoOn = false;
        threeOn = false;
        fourOn = true;
        timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (!oneOn)
            flameTrapOne.GetComponent<ParticleSystem>().Stop();
        else
            flameTrapOne.GetComponent<ParticleSystem>().Play();
        if (!twoOn)
            flameTrapTwo.GetComponent<ParticleSystem>().Stop();
        else
            flameTrapTwo.GetComponent<ParticleSystem>().Play();
        if (!threeOn)
            flameTrapThree.GetComponent<ParticleSystem>().Stop();
        else
            flameTrapThree.GetComponent<ParticleSystem>().Play();
        if (!fourOn)
            flameTrapFour.GetComponent<ParticleSystem>().Stop();
        else
            flameTrapFour.GetComponent<ParticleSystem>().Play();

        timer = timer + Time.deltaTime;
        if (timer >= 10.0f)
        {
            oneOn = !oneOn;
            twoOn = !twoOn;
            threeOn = !threeOn;
            fourOn = !fourOn;
            timer = 0.0f;
        }
	}
}
