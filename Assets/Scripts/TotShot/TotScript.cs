using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotScript : MonoBehaviour {

    public GameObject tot;
    public ParticleSystem explosion;

	// Use this for initialization
	void Start () {
        explosion.Stop();
	}
	
	// Update is called once per frame
	void Update () {
        checkForGoal();
        updateExplosionPosition();

    }

    public void checkForGoal()
    {
        if (tot.transform.position.z >= 153)
        {
            explosion.Play();
            tot.SetActive(false);
        }
        else if (tot.transform.position.z <= -153)
        {
            explosion.Play();
            tot.SetActive(false);
        }
    }

    public void updateExplosionPosition()
    {
        explosion.transform.position = new Vector3(tot.transform.position.x, tot.transform.position.y, tot.transform.position.z);
    }
}
