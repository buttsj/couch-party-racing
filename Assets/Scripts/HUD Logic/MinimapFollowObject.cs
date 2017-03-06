using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollowObject : MonoBehaviour {

    public Transform followObj;

    void FixedUpdate() {
        if (followObj)
        {
            transform.position = new Vector3(followObj.position.x, transform.position.y, followObj.position.z);
        }
    }
}
