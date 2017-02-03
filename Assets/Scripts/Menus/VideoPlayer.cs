using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayer : MonoBehaviour {

    public MovieTexture movie;

	// Use this for initialization
	void Start () {
        movie.loop = true;
        movie.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
