using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TotShotManager : MonoBehaviour {

    public Text redScoreText;
    public Text blueScoreText;
    private int redScoreInt;
    private int blueScoreInt;
    public Text time;
    private float secondsRemain;
    private float minutesRemain;
    private string seconds;
    private string minutes;
    private float timer;
    private bool isCountingDown;
    private bool deadBall;

    private Text winText;
    private Text startToContinueText;

    private ParticleSystem explosion;

    private float respawnDelayTimer;
    private const float RESPAWN_DELAY = 3.0f;

    private List<GameObject> karts;
    private List<Vector3> startingLocations = new List<Vector3>() { new Vector3(-30, 1, -140), new Vector3(30, 1, -140), new Vector3(-30, 1, 140), new Vector3(30, 1, 140) };
    private List<Quaternion> startingRotations = new List<Quaternion>() { Quaternion.Euler(new Vector3(0f, 0f, 0f)), Quaternion.Euler(new Vector3(0f, 0f, 0f)),
        Quaternion.Euler(new Vector3(0f, 180f, 0f)), Quaternion.Euler(new Vector3(0f, 180f, 0f)) };

    public int RedScore { get { return redScoreInt; } set { redScoreInt = value; } }
    public int BlueScore { get { return blueScoreInt; } set { blueScoreInt = value; } }
    public bool DeadBall { get { return deadBall; } set { deadBall = value;} }

    void Start()
    {
        redScoreInt = 0;
        blueScoreInt = 0;
        secondsRemain = 0;
        minutesRemain = 1;
        seconds = "00";
        minutes = "1";
        time.text = minutes + ":" + seconds;
        deadBall = false;
        isCountingDown = true;
        timer = 3f;

        respawnDelayTimer = 0.0f;

        GameObject tot = Instantiate(Resources.Load<GameObject>("Prefabs/Tot"), new Vector3(0.0f, 5.0f, 0.0f), Quaternion.Euler(Vector3.zero));
        explosion = Instantiate(Resources.Load<ParticleSystem>("Prefabs/TotExplosionParticles"), new Vector3(0.0f, 5.0f, 0.0f), Quaternion.Euler(Vector3.zero));
        tot.GetComponent<TotScript>().manager = gameObject;
        tot.GetComponent<TotScript>().explosion = explosion;

        winText = GameObject.Find("WinText").GetComponent<Text>();
        winText.enabled = false;
        startToContinueText = GameObject.Find("PressStartText").GetComponent<Text>();
        startToContinueText.enabled = false;
    }

    void Update()
    {
        redScoreText.text = redScoreInt.ToString();
        blueScoreText.text = blueScoreInt.ToString();

        if (gameIsOver())
        {
            time.text = "0:00";
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

            if (SimpleInput.GetButtonDown("Pause"))
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

            if (!isCountingDown)
            {
                UpdateTimerUI();
            }
            else
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 1)
            {
                isCountingDown = false;
            }
        }

    }

    private void UpdateTimerUI()
    {
        if (!gameIsOver())
        {
            secondsRemain -= Time.deltaTime;
            seconds = ((int)secondsRemain).ToString();
            if (secondsRemain < 10)
            {
                seconds = "0" + ((int)secondsRemain).ToString();
            }
            if (secondsRemain <= 0 && minutesRemain >= 0)
            {
                minutesRemain--;
                secondsRemain = 59.9f;
                seconds = "59";
            }
            minutes = minutesRemain.ToString();

            time.text = minutes + ":" + seconds;
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
        if(minutesRemain <= 0 && secondsRemain <= 0)
        {
            isOver = true;
        }
        else if(blueScoreInt >= 3)
        {
            isOver = true;
        }
        else if(redScoreInt >= 3)
        {
            isOver = true;
        }

        return isOver;
    }

}
