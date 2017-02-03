using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public Transform player;
    public float followDistance;
    private Vector3 offset;
	// Use this for initialization
	void Start() {
        offset = new Vector3(0, 7, 0)
        transform.position = player.position - followDistance * player.forward + offset;
        transform.localEulerAngles = new Vector3(player.localEulerAngles.x, player.localEulerAngles.y, 0);
    }
	

	
	// Update is called once per frame
	void LateUpdate () {
        if (!player.gameObject.GetComponent<Kart>().IsDamaged)
        {
            transform.position = player.position - followDistance * player.forward + offset;
            transform.localEulerAngles = new Vector3(player.localEulerAngles.x, player.localEulerAngles.y, 0);
        }
    }
}
