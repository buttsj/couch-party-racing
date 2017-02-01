using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public Transform player;
    public float followDistance;
    private Vector3 offset;
	// Use this for initialization
	void Start() {
        offset = new Vector3(0, 7, -followDistance);
    }
	

	
	// Update is called once per frame
	void Update () {
        transform.position = player.TransformPoint(offset);
        transform.localEulerAngles = new Vector3(player.localEulerAngles.x, player.localEulerAngles.y, 0);
        
    }
}
