using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    private float boost;
    public GameObject kart;
    public Text boostText;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        boost = kart.GetComponent<Kart>().Boost;
        boostText.text = ((int)boost).ToString();
    }
}
