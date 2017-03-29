using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorRotator : MonoBehaviour {

    public Transform rotateAroundObject;

    private bool canAcceptInput = false;
    //private List<float> rotationAngleList = new List<Vector3>() { new Vector3(90, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 90, 0), new Vector3(0, 270, 0)};
    private int rotationAngleIndex = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (canAcceptInput && SimpleInput.GetAxis("Flip", 1) < 0) {
            float angle = 90f;
            Debug.Log(rotationAngleIndex);
            switch (rotationAngleIndex) {
                case 0:
                    transform.RotateAround(rotateAroundObject.position, new Vector3(1, 0, 0), -angle);
                    break;
                case 4:
                    transform.RotateAround(rotateAroundObject.position, new Vector3(0, 1, 0), angle);
                    transform.RotateAround(rotateAroundObject.position, new Vector3(1, 0, 0), angle);
                    break;
                default:
                    transform.RotateAround(rotateAroundObject.position, new Vector3(0, 1, 0), angle);
                    break;
            }

            rotationAngleIndex = (++rotationAngleIndex) % 5;
        }

        canAcceptInput = IsInputReset();
    }

    private bool IsInputReset() {
        return SimpleInput.GetAxis("Flip") == 0 && SimpleInput.GetAxis("Roll") == 0;
    }

    private bool CanRotateXAxis() {
        var rot = transform.rotation.eulerAngles;
        Debug.Log(rot.z);
        return (rot.z == 0 || (int)Mathf.Abs(rot.z) == 180) && (rot.y == 0 || (int)Mathf.Abs(rot.y) == 180);
    }
}
