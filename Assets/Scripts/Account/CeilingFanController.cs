using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingFanController : MonoBehaviour {

	void Update () {
        transform.Rotate(Vector3.up, 5.0f);
    }

}
