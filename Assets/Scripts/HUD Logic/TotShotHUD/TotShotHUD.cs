using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotShotHUD : MonoBehaviour
{
    private List<GameObject> aiList;
    public Text redScoreText;
    public Text blueScoreText;
    public int redScoreInt;
    public int blueScoreInt;
    public Text time;
    List<float> seconds;
    List<int> minutes;

    public int RedScore { get { return redScoreInt; } set { redScoreInt = value; } }
    public int BlueScore { get { return blueScoreInt; } set { blueScoreInt = value; } }

    // Use this for initialization
    void Start()
    {
        redScoreInt = 0;
        blueScoreInt = 0;
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
        redScoreText.text = redScoreInt.ToString();
        blueScoreText.text = blueScoreInt.ToString();
    }
}
