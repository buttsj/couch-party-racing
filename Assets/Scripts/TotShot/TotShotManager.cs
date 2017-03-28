using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TotShotManager : MonoBehaviour {

    public Text redScoreText;
    public Text blueScoreText;
    public Text time;
    private Text winText;
    private Text startToContinueText;

    private int redScoreInt;
    private int blueScoreInt;
    private float secondsRemain;

    private bool deadBall;

    private ParticleSystem explosion;

    private float respawnDelayTimer;
    private const float RESPAWN_DELAY = 3.0f;

    public int RedScore { get { return redScoreInt; } set { redScoreInt = value; } }
    public int BlueScore { get { return blueScoreInt; } set { blueScoreInt = value; } }
    public bool DeadBall { get { return deadBall; } set { deadBall = value;} }

    void Start()
    {
        redScoreInt = 0;
        blueScoreInt = 0;
        secondsRemain = 180;

        time.text = secondsRemain.ToString("0.0");

        deadBall = false;

        respawnDelayTimer = 0.0f;

        GameObject tot = Instantiate(Resources.Load<GameObject>("Prefabs/Tot"), new Vector3(0.0f, 5.0f, 0.0f), Quaternion.Euler(Vector3.zero));
        explosion = Instantiate(Resources.Load<ParticleSystem>("Prefabs/TotExplosionParticles"), new Vector3(0.0f, 5.0f, 0.0f), Quaternion.Euler(Vector3.zero));
        tot.GetComponent<TotScript>().manager = gameObject;
        tot.GetComponent<TotScript>().explosion = explosion;

        winText = GameObject.Find("WinText").GetComponent<Text>();
        winText.enabled = false;
        startToContinueText = GameObject.Find("PressStartText").GetComponent<Text>();
        startToContinueText.text = "Press BACK/ESC";
        startToContinueText.enabled = false;
    }

    void Update()
    {
        redScoreText.text = redScoreInt.ToString();
        blueScoreText.text = blueScoreInt.ToString();
        time.text = secondsRemain.ToString("0.0");

        if (gameIsOver())
        {
            winText.enabled = true;
            startToContinueText.enabled = true;
            if (blueScoreInt > redScoreInt)
            {
                winText.text = "Blue Team Wins!";
                winText.color = Color.blue;
            }
            else if(redScoreInt > blueScoreInt)
            {
                winText.text = "Red Team Wins!";
                winText.color = Color.red;
            }
            else
            {
                winText.text = "Draw";
            }

            if (SimpleInput.GetButtonDown("Cancel"))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        else
        {
            if (DeadBall)
            {
                resetBall();
            }

            secondsRemain -= Time.deltaTime;
                
        }

    }

    public void addToBlue()
    {
        blueScoreInt++;
    }

    public void addToRed()
    {
        redScoreInt++;
    }

    private void resetBall()
    {
        if (respawnDelayTimer >= RESPAWN_DELAY)
        {
            GameObject tot = Instantiate(Resources.Load<GameObject>("Prefabs/Tot"), new Vector3(0.0f, 5.0f, 0.0f), Quaternion.Euler(Vector3.zero));
            tot.GetComponent<TotScript>().manager = gameObject;
            tot.GetComponent<TotScript>().explosion = explosion;
            respawnDelayTimer = 0;
            deadBall = false;
        }
        else
        {
            respawnDelayTimer += Time.deltaTime;
        }
    }

    private bool gameIsOver()
    {
        bool isOver = false;
        if(secondsRemain <= 0)
        {
            secondsRemain = 0;
            isOver = true;
        }

        return isOver;
    }

}
