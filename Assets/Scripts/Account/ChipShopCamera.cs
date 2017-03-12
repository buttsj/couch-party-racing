using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipShopCamera : MonoBehaviour {

    Vector3 zoomedIn;
    Vector3 zoomedOut;
    float camPanDuration;

	// Use this for initialization
	void Start () {
        zoomedOut = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        zoomedIn = new Vector3(-10f, 2f, 3.5f);
        camPanDuration = .3f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FocusIn()
    {
        StartCoroutine(LerpToPosition(camPanDuration, zoomedIn));
    }

    public void FocusOut()
    {
        StartCoroutine(LerpToPosition(camPanDuration, zoomedOut));
    }

    private IEnumerator LerpToPosition(float lerpSpeed, Vector3 newPosition)
    {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / lerpSpeed);
            transform.position = Vector3.Lerp(startingPos, newPosition, t);
            yield return 0;
        }
    }
}
