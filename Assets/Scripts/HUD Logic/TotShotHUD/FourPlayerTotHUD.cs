using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FourPlayerTotHUD : MonoBehaviour {

    public GameObject kart1;
    public GameObject kart2;
    public GameObject kart3;
    public GameObject kart4;
    public Text boost1Text;
    private float boost1Int;
    public Text boost2Text;
    private float boost2Int;
    public Text boost3Text;
    private float boost3Int;
    public Text boost4Text;
    private float boost4Int;

    // Use this for initialization
    void Start()
    {
        boost1Int = 100;
        boost1Text.text = boost1Int.ToString();
        boost2Int = 100;
        boost2Text.text = boost2Int.ToString();
        boost3Int = 100;
        boost3Text.text = boost3Int.ToString();
        boost4Int = 100;
        boost4Text.text = boost4Int.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        boost1Int = (int)kart1.GetComponent<Kart>().Boost;
        boost1Text.text = boost1Int.ToString();
        boost2Int = (int)kart2.GetComponent<Kart>().Boost;
        boost2Text.text = boost2Int.ToString();
        boost3Int = (int)kart3.GetComponent<Kart>().Boost;
        boost3Text.text = boost3Int.ToString();
        boost4Int = (int)kart4.GetComponent<Kart>().Boost;
        boost4Text.text = boost4Int.ToString();
    }
}
