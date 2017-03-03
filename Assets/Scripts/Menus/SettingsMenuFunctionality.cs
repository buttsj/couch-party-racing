using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuFunctionality : MonoBehaviour {

    public Toggle fullscreen;
    public Dropdown resolution;
    public Dropdown texQuality;
    public Dropdown aa;
    public Dropdown vsync;
    public Slider volume;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ApplySettings()
    {
        if (fullscreen.isOn)
            Screen.fullScreen = true;
        else
            Screen.fullScreen = false;

        AudioListener.volume = volume.value;
    }
}
