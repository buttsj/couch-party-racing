using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTestingScript : MonoBehaviour {

    public int playerNumber;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        var horiValue = SimpleInput.GetAxis("HorizontalTest", playerNumber);
        var vertValue = SimpleInput.GetAxis("VerticalTest", playerNumber);
        var colorValue = SimpleInput.GetButton("ColorTest", playerNumber);

        //Debug.Log(horiValue);
        //Debug.Log(Input.GetJoystickNames()[0]);
        //Debug.Log(SimpleInput.GetAnyButton());



        if (horiValue != 0) {
            gameObject.transform.position = gameObject.transform.position + new Vector3(horiValue, 0, 0);
        }

        if (vertValue != 0) {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, vertValue, 0);
        }

        if (colorValue) {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        } else {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
