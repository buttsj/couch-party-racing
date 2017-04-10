using UnityEngine;

public class RecordingCamera : MonoBehaviour {

    public GameObject camParent;
    public GameObject kart;
    public GameObject cam1;
    public GameObject cam2;
    public GameObject rotCam;

    private float timer;
    private float angle;
    private Vector3 totalangles;

	// Use this for initialization
	void Start () {
        timer = 0;
        angle = 0;
	}

	// Update is called once per frame
	void Update () {

        camParent.transform.position = kart.transform.position;

        if (gameObject.name == "RotatingCamera")
        {
            transform.RotateAround(kart.transform.position, new Vector3(0, 1, 0), 10.0f * Time.deltaTime);
            angle = transform.eulerAngles.y;
        }
        else
        {
            timer = timer + Time.deltaTime;
        }

        if (angle > 100.0f && angle < 105.0f)
        {
            cam1.SetActive(true);
            gameObject.SetActive(false);
        }

        if (angle > 200.0f && angle < 205.0f)
        {
            cam2.SetActive(true);
            gameObject.SetActive(false);
        }

        if (timer > 10.0f)
        {
            timer = 0.0f;
            rotCam.transform.RotateAround(kart.transform.position, new Vector3(0, 1, 0), 5.0f);
            rotCam.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
