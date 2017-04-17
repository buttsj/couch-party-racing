using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    
    public enum Type { Boost, Oil, Spark, Marble, Shield }
    private AudioClip powerUpGet;

    void Start () {
        powerUpGet = Resources.Load<AudioClip>("Sounds/KartEffects/Item_Get_Sound");
    }

    public Type DeterminePowerup(KartAudio audio)
    {
        Type ret;
        int num = Random.Range(1, 6);

        num = 5;

        switch (num)
        {
            case 1:
                ret = Type.Boost;
                break;
            case 2:
                ret = Type.Oil;
                break;
            case 3:
                ret = Type.Spark;
                break;
            case 4:
                ret = Type.Marble;
                break;
            case 5:
                ret = Type.Shield;
                break;
            default:
                ret = Type.Boost; // this won't happen
                break;
        }
        audio.playOneOff(powerUpGet);
        return ret;
    }

	void Update () {
		
	}

    void FixedUpdate ()
    {
        transform.Rotate(Vector3.up, 2.0f);
    }
}
