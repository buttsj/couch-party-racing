using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountdownTimer : MonoBehaviour {

    public Text timerText;
    private float timer;
	// Use this for initialization
	void Start () {
        timer = 3f;
        timerText.text = timer.ToString("F2");
	}
	
	// Update is called once per frame
	void Update () {
        timer -= .02f;
        if (timer > 0 && timer < 2.98f)
        {
            Time.timeScale = 0;
            timerText.text = timer.ToString("F2");
        }
        else if (timer < 0 && timer > -2)
        {
            Time.timeScale = 1;
            timerText.text = "Go!";
        }
        else if (timer < -2){
            gameObject.GetComponent<Canvas>().enabled = false;
        }
		
	}

    public void ResetTimer()
    {
        timer = 3f;
        timerText.text = timer.ToString("F2");
        gameObject.GetComponent<Canvas>().enabled = true;
    }
}
