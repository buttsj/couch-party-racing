using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowKart : MonoBehaviour {

    public GameObject kart;

    void FixedUpdate ()
    {
        transform.position = new Vector3(kart.transform.position.x, transform.position.y, kart.transform.position.z);
    }
}
