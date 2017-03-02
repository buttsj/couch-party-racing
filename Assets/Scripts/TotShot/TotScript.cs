using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotScript : MonoBehaviour {

    public GameObject tot;
    public ParticleSystem explosion;
    public GameObject hud;
    public GameObject resetTimer;
    public bool isAfterGoalSequence;
    private List<GameObject> karts;
    private List<Vector3> startingLocations = new List<Vector3>() { new Vector3(-30, 1, -140), new Vector3(30, 1, -140), new Vector3(-30, 1, 140), new Vector3(30, 1, 140) };
    private List<Quaternion> startingRotations = new List<Quaternion>() { Quaternion.Euler(new Vector3(0f, 0f, 0f)), Quaternion.Euler(new Vector3(0f, 0f, 0f)),
        Quaternion.Euler(new Vector3(0f, 180f, 0f)), Quaternion.Euler(new Vector3(0f, 180f, 0f)) };
    private float timer;

    private bool hudSet;

    void Start()
    {
        explosion.Stop();
        hudSet = false;
        isAfterGoalSequence = false;
        timer = 0;
    }

    void Update () {
        if (hudSet)
        {
            checkForGoal();
            updateExplosionPosition();
        }
        while (isAfterGoalSequence)
        {
            ResetEnvironment();
            isAfterGoalSequence = false;
            hudSet = true;
            timer = 0;
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
            isAfterGoalSequence = true;
            hudSet = false;
        }
        else if (tot.transform.position.z <= -153)
        {
            explosion.Play();
            tot.SetActive(false);
            int bScore = hud.GetComponent<TotShotHUD>().BlueScore;
            bScore += 1;
            hud.GetComponent<TotShotHUD>().BlueScore = bScore;
            isAfterGoalSequence = true;
            hudSet = false;
        }
    }

    public void updateExplosionPosition()
    {
        explosion.transform.position = new Vector3(tot.transform.position.x, tot.transform.position.y, tot.transform.position.z);
    }

    public void setHUD(GameObject hud, GameObject resetTimer)
    {
        hudSet = true;
        this.hud = hud;
        this.resetTimer = resetTimer;
    }

    public void ResetEnvironment()
    {
        for (int i = 0; i < 4;i++)
        {
            karts[i].transform.position = startingLocations[i];
            karts[i].transform.rotation = startingRotations[i];
        }
        resetTimer.GetComponent<CountdownTimer>().ResetTimer();
        tot.transform.position = new Vector3(0, 5, 0);
        tot.SetActive(true);
    }

    public void setKarts(List<GameObject> karts)
    {
        this.karts = karts;
    }
}
