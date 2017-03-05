using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMovement : MonoBehaviour {

    private const int TRACK_SCALE = 60;
    private bool canAcceptInput = false;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (canAcceptInput) {
            // X-Axis
            if (SimpleInput.GetAxis("Horizontal", 1) > 0) {
                transform.position += new Vector3(TRACK_SCALE, 0, 0);
            }
            if (SimpleInput.GetAxis("Horizontal", 1) < 0) {
                transform.position -= new Vector3(TRACK_SCALE, 0, 0);
            }

            // Y-Axis
            if (SimpleInput.GetAxis("Vertical", 1) > 0) {
                transform.position += new Vector3(0, TRACK_SCALE, 0);
            }
            if (SimpleInput.GetAxis("Vertical", 1) < 0) {
                transform.position -= new Vector3(0, TRACK_SCALE, 0);
            }
        }

        // Z-Axis
        if (SimpleInput.GetButtonDown("Accelerate", 1)) {
            transform.position += new Vector3(0, 0, TRACK_SCALE);
        }
        if (SimpleInput.GetButtonDown("Reverse", 1)) {
            transform.position -= new Vector3(0, 0, TRACK_SCALE);
        }

        canAcceptInput = IsInputReset();
    }

    private bool IsInputReset() {
        return SimpleInput.GetAxis("Horizontal") == 0 && SimpleInput.GetAxis("Vertical") == 0;
    }
}
