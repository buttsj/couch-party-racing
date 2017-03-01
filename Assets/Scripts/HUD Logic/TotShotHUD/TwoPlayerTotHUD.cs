using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoPlayerTotHUD : MonoBehaviour {

    public GameObject kart1;
    public GameObject kart2;
    public Text boost1Text;
    private float boost1Int;
    public Text boost2Text;
    private float boost2Int;

    // Use this for initialization
    void Start()
    {
        boost1Int = 100;
        boost1Text.text = boost1Int.ToString();
        boost2Int = 100;
        boost2Text.text = boost2Int.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        boost1Int = (int)kart1.GetComponent<Kart>().Boost;
        boost1Text.text = boost1Int.ToString();
        boost2Int = (int)kart2.GetComponent<Kart>().Boost;
        boost2Text.text = boost2Int.ToString();
    }
}
