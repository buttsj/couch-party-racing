using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotScript : MonoBehaviour {

    public GameObject tot;
    public ParticleSystem explosion;
    public GameObject hud;

    private bool hudSet;

    void Start()
    {
        hud = GameObject.FindGameObjectWithTag("TotHUD");
        explosion.Stop();

        hudSet = false;
    }

    void Update () {
        if (hudSet)
        {
            checkForGoal();
            updateExplosionPosition();
        }
    }

    public void checkForGoal()
    {
        if (tot.transform.position.z >= 153)
        {
            explosion.Play();
            tot.SetActive(false);
            int rScore = hud.GetComponent<TotShotHUD>().RedScore;
            rScore += 1;
            hud.GetComponent<TotShotHUD>().RedScore = rScore;
            //reset ball, players, countdown timer
        }
        else if (tot.transform.position.z <= -153)
        {
            explosion.Play();
            tot.SetActive(false);
            int bScore = hud.GetComponent<TotShotHUD>().BlueScore;
            bScore += 1;
            hud.GetComponent<TotShotHUD>().BlueScore = bScore;
            //reset ball, players, countdown timer
        }
    }

    public void updateExplosionPosition()
    {
        explosion.transform.position = new Vector3(tot.transform.position.x, tot.transform.position.y, tot.transform.position.z);
    }

    public void setHUD(GameObject hud)
    {
        hudSet = true;
        this.hud = hud;
    }
}
