using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotScript : MonoBehaviour {

    public GameObject tot;
    public ParticleSystem explosion;
    public GameObject HUD;

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
            int rScore = HUD.GetComponent<TotShotHUD>().RedScore;
            rScore += 1;
            HUD.GetComponent<TotShotHUD>().RedScore = rScore;
            //reset ball, players, countdown timer
        }
        else if (tot.transform.position.z <= -153)
        {
            explosion.Play();
            tot.SetActive(false);
            int bScore = HUD.GetComponent<TotShotHUD>().BlueScore;
            bScore += 1;
            HUD.GetComponent<TotShotHUD>().BlueScore = bScore;
            //reset ball, players, countdown timer
        }
    }

    public void updateExplosionPosition()
    {
        explosion.transform.position = new Vector3(tot.transform.position.x, tot.transform.position.y, tot.transform.position.z);
    }
}
