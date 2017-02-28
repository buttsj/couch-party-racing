﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToPreviousScene : MonoBehaviour {

    private const string TRANSITION = "Sounds/KartEffects/screen_transition";
    private AudioClip transition;
    private AudioSource source;
    private bool pressed;


    void Start ()
    {
        pressed = false;
        transition = (AudioClip)Resources.Load(TRANSITION);
        source = GameObject.Find("Sound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (SimpleInput.GetButton("Cancel") && !pressed) {
            pressed = true;
            StartCoroutine(Transition());
        }
	}

    private IEnumerator Transition()
    {
        source.clip = transition;
        source.loop = false;
        pressed = true;
        source.Play();
        yield return new WaitWhile(() => source.isPlaying);
        var prevScene = SceneManager.GetActiveScene().buildIndex - 1;
        if (prevScene == 0)
        {
            Destroy(GameObject.Find("SceneGenerator"));
        }
        SceneManager.LoadScene(prevScene);
    }
}
