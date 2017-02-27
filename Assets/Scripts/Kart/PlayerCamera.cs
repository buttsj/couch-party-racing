using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public Transform player;
    public float followDistance;
    private Vector3 offset;
    int notGroundedOffset;
    float fov;
	// Use this for initialization
	void Start() {
        offset = new Vector3(0, 7, 0);
        transform.position = player.position - followDistance * player.forward + offset;
        transform.localEulerAngles = new Vector3(player.localEulerAngles.x, player.localEulerAngles.y, 0);
        fov = gameObject.GetComponent<Camera>().fieldOfView;
    }



    // Update is called once per frame
    void LateUpdate() {
        if (!player.gameObject.GetComponent<Kart>().IsDamaged)
        {
            transform.position = player.position - followDistance * player.forward + offset;
            transform.localEulerAngles = new Vector3(player.localEulerAngles.x, player.localEulerAngles.y, 0);
        }
        else {
            transform.position = player.position - followDistance * transform.forward + offset;

        }
      
        if (player.gameObject.GetComponent<Kart>().IsBoosting)
        {
            if (gameObject.GetComponent<Camera>().fieldOfView < 80)
            {
                gameObject.GetComponent<Camera>().fieldOfView++;
            }

        }
        else
        {
            if (gameObject.GetComponent<Camera>().fieldOfView > 60)
            {
                gameObject.GetComponent<Camera>().fieldOfView--;
            }
        }
    }
}
