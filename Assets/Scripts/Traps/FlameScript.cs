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
        {
            flameTrapOne.GetComponent<ParticleSystem>().Stop();
            flameTrapOne.transform.parent.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        else
        {
            flameTrapOne.GetComponent<ParticleSystem>().Play();
            flameTrapOne.transform.parent.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        }
        if (!twoOn)
        {
            flameTrapTwo.GetComponent<ParticleSystem>().Stop();
            flameTrapTwo.transform.parent.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        else
        {
            flameTrapTwo.GetComponent<ParticleSystem>().Play();
            flameTrapTwo.transform.parent.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        }
        if (!threeOn)
        {
            flameTrapThree.GetComponent<ParticleSystem>().Stop();
            flameTrapThree.transform.parent.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        else
        {
            flameTrapThree.GetComponent<ParticleSystem>().Play();
            flameTrapThree.transform.parent.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        }
        if (!fourOn)
        {
            flameTrapFour.GetComponent<ParticleSystem>().Stop();
            flameTrapFour.transform.parent.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        else
        {
            flameTrapFour.GetComponent<ParticleSystem>().Play();
            flameTrapFour.transform.parent.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        }

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
