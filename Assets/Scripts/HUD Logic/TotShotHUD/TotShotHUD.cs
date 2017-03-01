using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotShotHUD : MonoBehaviour
{
    public Text redScoreText;
    public Text blueScoreText;
    public int redScoreInt;
    public int blueScoreInt;
    public Text time;
    List<float> seconds;
    List<int> minutes;

    public int RedScore { get { return redScoreInt; } set { redScoreInt = value; } }
    public int BlueScore { get { return blueScoreInt; } set { blueScoreInt = value; } }

    void Start()
    {
        redScoreInt = 0;
        blueScoreInt = 0;
        seconds = new List<float>();
        minutes = new List<int>();
        seconds.Add(0f);
        minutes.Add(0);
    }

    void Update()
    {
        redScoreText.text = redScoreInt.ToString();
        blueScoreText.text = blueScoreInt.ToString();
    }
}
