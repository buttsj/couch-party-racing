using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public Transform player;
    public float followDistance;
    private Vector3 originalOrientation;
    private Vector3 offset;
    float fov;
	// Use this for initialization
	void Start() {
        offset = new Vector3(0, 10, 0);
        transform.position = player.position - followDistance * player.forward + offset;
        transform.localEulerAngles = new Vector3(player.localEulerAngles.x + 10, player.localEulerAngles.y, 0);
        fov = gameObject.GetComponent<Camera>().fieldOfView;
    }



    // Update is called once per frame
    void LateUpdate() {
        if (!player.gameObject.GetComponent<Kart>().IsDamaged && !player.gameObject.GetComponent<Kart>().PhysicsObject.IsFlipping)
        {
            Vector3 tempPosition = new Vector3(followDistance * player.forward.x, 0, followDistance * player.forward.z);
            transform.position = player.position - followDistance * tempPosition.normalized + offset;
            transform.localEulerAngles =  new Vector3(transform.localEulerAngles.x, player.localEulerAngles.y, 0);
            originalOrientation = tempPosition;
        }
        else {
            transform.position = player.position - followDistance * originalOrientation.normalized + offset;

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
