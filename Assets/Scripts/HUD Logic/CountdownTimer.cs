using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountdownTimer : MonoBehaviour {


    private WhiteFadeUniversal fader;
    public Text timerText;
    private float timer;

    void Awake()
    {
        // fade in/out initializer
        GameObject fadeObject = new GameObject();
        fadeObject.name = "Fader";
        fadeObject.transform.SetParent(transform);
        fadeObject.SetActive(true);
        fader = fadeObject.AddComponent<WhiteFadeUniversal>();
        fader.BeginNewScene("Music Manager HUD");
        //
    }

    void Start () {
        timer = 3f;
        timerText.text = timer.ToString("F2");
	}
	
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
            timerText.text = "Go !";
        }
        else if (timer < -2){
            gameObject.GetComponent<Canvas>().enabled = false;
            Destroy(gameObject);
        }
		
	}

    public void ResetTimer()
    {
        timer = 3f;
        timerText.text = timer.ToString("F2");
        gameObject.GetComponent<Canvas>().enabled = true;
    }
}
