using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerTotHUD : MonoBehaviour {

    public GameObject kart1;
    public Text boost1Text;
    private float boost1Int;

	// Use this for initialization
	void Start () {
        boost1Int = 100;
        boost1Text.text = boost1Int.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        boost1Int = (int)kart1.GetComponent<Kart>().Boost;
        boost1Text.text = boost1Int.ToString();
	}
}
