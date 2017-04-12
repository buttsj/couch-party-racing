﻿using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCamera : MonoBehaviour {

    public Transform player;
    public float followDistance;
    private Vector3 originalOrientation;
    private Vector3 offset;
    float fov;
    public bool Spectate = false;
    private bool isHuman;

	void Start() {
        offset = new Vector3(0, 10, 0);
        transform.position = player.position - followDistance * player.forward + offset;
        transform.localEulerAngles = new Vector3(player.localEulerAngles.x + 10, player.localEulerAngles.y, 0);
        fov = gameObject.GetComponent<Camera>().fieldOfView;

        if(player.gameObject.GetComponent<Kart>() != null)
        {
            isHuman = true;
        }

        if (Spectate)
        {
            Text text = FindObjectsOfType<Canvas>()[0].gameObject.AddComponent<Text>();
            text.text = "Press Space/Bump Kart to quit";
            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.font = ArialFont;
            text.material = ArialFont.material;
        }
    }

    void LateUpdate() {
        if (isHuman)
        {
            if (!player.gameObject.GetComponent<Kart>().IsDamaged && !player.gameObject.GetComponent<Kart>().PhysicsObject.IsFlipping)
            {
                Vector3 tempPosition = new Vector3(followDistance * player.forward.x, 0, followDistance * player.forward.z);
                transform.position = player.position - followDistance * tempPosition.normalized + offset;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, player.localEulerAngles.y, 0);
                originalOrientation = tempPosition;
            }
            else
            {
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
        else
        {
            if (!player.gameObject.GetComponent<WaypointAI>().IsDamaged && !player.gameObject.GetComponent<WaypointAI>().PhysicsObject.IsFlipping)
            {
                Vector3 tempPosition = new Vector3(followDistance * player.forward.x, 0, followDistance * player.forward.z);
                transform.position = player.position - followDistance * tempPosition.normalized + offset;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, player.localEulerAngles.y, 0);
                originalOrientation = tempPosition;
            }
            else
            {
                transform.position = player.position - followDistance * originalOrientation.normalized + offset;
            }

            if (player.gameObject.GetComponent<WaypointAI>().IsBoosting)
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
            if (Spectate)
            {
                if (SimpleInput.GetButtonDown("Bump Kart"))
                {
                    SceneManager.LoadScene(0);
                }
            }
        }

    }
}
