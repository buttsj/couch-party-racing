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
    private bool addedChips;
    private bool deadBall;
    private bool destroyedKarts;
    private string winningTeam;
    private ParticleSystem explosion;

    private float respawnDelayTimer;
    private const float RESPAWN_DELAY = 3f;

    public int RedScore { get { return redScoreInt; } set { redScoreInt = value; } }
    public int BlueScore { get { return blueScoreInt; } set { blueScoreInt = value; } }
    public bool DeadBall { get { return deadBall; } set { deadBall = value;} }
    private List<Vector3> kartStartList = new List<Vector3>() { new Vector3(-30, 1, -140), new Vector3(30, 1, 140), new Vector3(30, 1, -140), new Vector3(-30, 1, 140) };
    private List<Quaternion> kartStartRotation = new List<Quaternion>() { Quaternion.Euler(new Vector3(0f, 0f, 0f)), Quaternion.Euler(new Vector3(0f, 180f, 0f)),
        Quaternion.Euler(new Vector3(0f, 0f, 0f)), Quaternion.Euler(new Vector3(0f, 180f, 0f)) };

    void Start()
    {
        redScoreInt = 0;
        blueScoreInt = 0;
        secondsRemain = 180;

        time.text = secondsRemain.ToString("0.0");
        destroyedKarts = false;
        deadBall = false;

        respawnDelayTimer = 0.0f;

        GameObject tot = Instantiate(Resources.Load<GameObject>("Prefabs/Tot"), new Vector3(0.0f, 5.0f, 0.0f), Quaternion.Euler(Vector3.zero));
        explosion = Instantiate(Resources.Load<ParticleSystem>("Prefabs/TotExplosionParticles"), new Vector3(0.0f, 5.0f, 0.0f), Quaternion.Euler(Vector3.zero));
        tot.GetComponent<TotScript>().manager = gameObject;
        tot.GetComponent<TotScript>().explosion = explosion;
        GetInitialLocationAndRotation();
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
                winningTeam = "blue";
            }
            else if(redScoreInt > blueScoreInt)
            {
                winText.text = "Red Team Wins!";
                winText.color = Color.red;
                winningTeam = "red";
            }
            else
            {
                winText.text = "Draw";
                winningTeam = "draw";
            }

            if (!addedChips && winText.enabled)
            {
                GameObject.Find("AccountManager").GetComponent<AccountManager>().CurrentChips += 5;
                PlayerPrefs.Save();
                addedChips = true;
                if (CouchPartyManager.IsCouchPartyMode) {
                    AddCouchPartyPoints(winningTeam);
                }
            }

            if (SimpleInput.GetButtonDown("Cancel"))
            {
                Debug.Log("Cancel Pressed");
                if (CouchPartyManager.IsCouchPartyMode)
                {
                    Debug.Log("IsCouchPartyMode");
                    SceneManager.LoadScene("CouchPartyEndScene");
                }
                else
                {
                    SceneManager.LoadScene("MainMenu");
                }
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

        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            secondsRemain -= 60;
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
            if (!destroyedKarts)
            {
                DestroyKarts();
            }
            else
            {
                if (!AllKartsDestroyed())
                {
                    GameObject tot = Instantiate(Resources.Load<GameObject>("Prefabs/Tot"), new Vector3(0.0f, 5.0f, 0.0f), Quaternion.Euler(Vector3.zero));
                    tot.GetComponent<TotScript>().manager = gameObject;
                    tot.GetComponent<TotScript>().explosion = explosion;
                    respawnDelayTimer = 0;
                    ResetKarts();
                    deadBall = false;
                    GameObject.Find("CountdownTimer(Clone)").GetComponent<CountdownTimer>().ResetTimer();

                }
            }
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

    private void ResetKarts() {
        foreach (GameObject kart in GameObject.FindGameObjectsWithTag("Player")) {
            if (kart.name.Contains("1"))
            {
                kart.transform.position = kartStartList[0];
                kart.transform.rotation = kartStartRotation[0];
            }
            else if (kart.name.Contains("2")) {
                kart.transform.position = kartStartList[1];
                kart.transform.rotation = kartStartRotation[1];
            }
            else if (kart.name.Contains("3"))
            {
                kart.transform.position = kartStartList[2];
                kart.transform.rotation = kartStartRotation[2];
            }
            else if (kart.name.Contains("4"))
            {
                kart.transform.position = kartStartList[3];
                kart.transform.rotation = kartStartRotation[3];
            }
            kart.GetComponent<Rigidbody>().isKinematic = true;
            kart.GetComponent<Rigidbody>().isKinematic = false;
         }
        destroyedKarts = false;
    }

    private void GetInitialLocationAndRotation() {
        foreach (GameObject kart in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (kart.name.Contains("1"))
            {
                kartStartList[0] = kart.transform.position;
                kartStartRotation[0] = kart.transform.rotation;
                
            }
            else if (kart.name.Contains("2"))
            {
                kartStartList[1] = kart.transform.position;
                kartStartRotation[1] = kart.transform.rotation;
            }
            else if (kart.name.Contains("3"))
            {
                kartStartList[2] = kart.transform.position;
                kartStartRotation[2] = kart.transform.rotation;
            }
            else if (kart.name.Contains("4"))
            {
                kartStartList[3] = kart.transform.position;
                kartStartRotation[3] = kart.transform.rotation;
            }
            
        }
    }

    void DestroyKarts() {
        if (!destroyedKarts)
        {
            foreach (GameObject kart in GameObject.FindGameObjectsWithTag("Player"))
            {
                kart.GetComponent<Kart>().Destroyed = true;
                kart.GetComponent<Kart>().ToggleRenderers(false);
                kart.GetComponent<Kart>().transform.Find("ExplosionEffect").gameObject.SetActive(true);
            }
        }
        destroyedKarts = true;
    }

    bool AllKartsDestroyed() {
        bool allDestroyed = true;
        foreach (GameObject kart in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!kart.GetComponent<Kart>().Destroyed) {
                allDestroyed = false;
            }
        }
        return allDestroyed;
    }

    void AddCouchPartyPoints(string winningTeam) {
        List<GameObject> blueKarts = new List<GameObject>();
        List<GameObject> redKarts = new List<GameObject>();
        foreach (GameObject kart in GameObject.FindGameObjectsWithTag("Player")) {
            if (((TotShotGameState)kart.GetComponent<Kart>().GameState).getTeam() == "blue")
            {
                blueKarts.Add(kart);
            }
            else if (((TotShotGameState)kart.GetComponent<Kart>().GameState).getTeam() == "red") {
                redKarts.Add(kart);
            }
        }

        if (winningTeam == "blue")
        {
            foreach (GameObject kart in blueKarts)
            {
                if (kart.name.Contains("1"))
                {
                    CouchPartyManager.PlayerOneScore += 2;
                }
                else if (kart.name.Contains("2"))
                {
                    CouchPartyManager.PlayerTwoScore += 2;
                }
                else if (kart.name.Contains("3"))
                {
                    CouchPartyManager.PlayerThreeScore += 2;
                }
                else if (kart.name.Contains("4"))
                {
                    CouchPartyManager.PlayerFourScore += 2;
                }
            }
        }
        else if (winningTeam == "red")
        {
            foreach (GameObject kart in redKarts)
            {
                if (kart.name.Contains("1"))
                {
                    CouchPartyManager.PlayerOneScore += 2;
                }
                else if (kart.name.Contains("2"))
                {
                    CouchPartyManager.PlayerTwoScore += 2;
                }
                else if (kart.name.Contains("3"))
                {
                    CouchPartyManager.PlayerThreeScore += 2;
                }
                else if (kart.name.Contains("4"))
                {
                    CouchPartyManager.PlayerFourScore += 2;
                }
            }
        }
        
    }

}
