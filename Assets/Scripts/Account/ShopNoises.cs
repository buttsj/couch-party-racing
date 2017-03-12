using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNoises : MonoBehaviour {

    private const string NOISEONE = "Sounds/KartEffects/airwrench";
    private const string NOISETWO = "Sounds/KartEffects/drill";
    private const string NOISETHREE = "Sounds/KartEffects/longdrill";
    private AudioClip noiseOne;
    private AudioClip noiseTwo;
    private AudioClip noiseThree;
    private AudioSource source;
    private float timer;

    // Use this for initialization
    void Start () {
		noiseOne = (AudioClip)Resources.Load(NOISEONE);
        noiseTwo = (AudioClip)Resources.Load(NOISETWO);
        noiseThree = (AudioClip)Resources.Load(NOISETHREE);
        source = GameObject.Find("Sound").GetComponent<AudioSource>();
        timer = 5.0f;
    }
	
	// Update is called once per frame
	void Update () {
        timer = timer + Time.deltaTime;
        if (timer >= 10.0f)
        {
            timer = 0.0f;
            int rand = Random.Range(1, 4);
            switch (rand)
            {
                case 1:
                    source.PlayOneShot(noiseOne);
                    break;
                case 2:
                    source.PlayOneShot(noiseTwo);
                    break;
                case 3:
                    source.PlayOneShot(noiseThree);
                    break;
            }
        }
	}
}
