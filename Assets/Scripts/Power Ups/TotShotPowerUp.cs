using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotShotPowerUp : MonoBehaviour
{

    public enum Type { Boost }
    private AudioClip powerUpGet;

    void Start()
    {
        powerUpGet = Resources.Load<AudioClip>("Sounds/KartEffects/Item_Get_Sound");
    }

    public Type DeterminePowerup(KartAudio audio)
    {
        Type ret = Type.Boost;

        audio.playOneOff(powerUpGet);

        return ret;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, 2.0f);
    }
}
