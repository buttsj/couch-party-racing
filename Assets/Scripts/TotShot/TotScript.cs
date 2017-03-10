using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotScript : MonoBehaviour {

    public ParticleSystem explosion;
    public GameObject manager;

    void Start()
    {
        explosion.Stop();
    }

    void Update () {
        updateExplosionPosition();
        checkForGoal();
    }

    public void checkForGoal()
    {
        if (transform.position.z >= 303)
        {
            explosion.Play();
            manager.GetComponent<TotShotManager>().addToRed();
            manager.GetComponent<TotShotManager>().DeadBall = true;
            Destroy(gameObject);
        }
        else if (transform.position.z <= -303)
        {
            explosion.Play();
            manager.GetComponent<TotShotManager>().addToBlue();
            manager.GetComponent<TotShotManager>().DeadBall = true;
            Destroy(gameObject);
        }
    }

    public void updateExplosionPosition()
    {
        explosion.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

}
