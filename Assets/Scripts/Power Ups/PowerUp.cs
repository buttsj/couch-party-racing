using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    
    public enum Type { Boost, Oil, Spark }

	// Use this for initialization
	void Start () {
		
	}

    public Type DeterminePowerup()
    {
        Type ret;
        int num = Random.Range(1, 3);
        switch (num)
        {
            case 1:
                ret = Type.Boost;
                break;
            case 2:
                ret = Type.Oil;
                break;
            default:
                ret = Type.Spark;
                break;
        }
        return ret;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate ()
    {
        transform.Rotate(Vector3.up, 2.0f);
    }
}
