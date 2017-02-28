using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerTotShotHUD : MonoBehaviour
{

    private float boost;
    public GameObject kart;
    private List<GameObject> aiList;
    public Text boostText;
    public Text redScore;
    public Text blueScore;
    public Text time;
    List<float> seconds;
    List<int> minutes;

    // Use this for initialization
    void Start()
    {
        aiList = new List<GameObject>();
        seconds = new List<float>();
        minutes = new List<int>();
        seconds.Add(0f);
        minutes.Add(0);
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.name.Contains("AI"))
            {
                aiList.Add(player);
                seconds.Add(0f);
                minutes.Add(0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        boost = kart.GetComponent<Kart>().Boost;
        boostText.text = ((int)boost).ToString();
    }
}
