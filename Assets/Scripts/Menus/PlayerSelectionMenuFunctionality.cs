using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectionMenuFunctionality : MonoBehaviour {

    public Image img;
    public Text loading;
    private float alpha;
    private float fadeSpeed = 1f;
    private bool FadeOutBool;

    private const string TRANSITION = "Sounds/KartEffects/screen_transition";
    private AudioClip transition;
    private AudioSource source;
    private bool pressed;

    public const string READY = "Ready to Play!";
    public const string UNREADY = "Press Any Button";

    public Text player1ReadyText;
    public Text player2ReadyText;
    public Text player3ReadyText;
    public Text player4ReadyText;

    public Text startToContinueText;

    private SceneGenerator sceneGenerator;
    private string gamemodeName;

    void Start() {
        Color col = img.color;
        col.a = 0;
        img.color = col;
        alpha = 0.0f;
        FadeOutBool = false;

        pressed = false;
        transition = (AudioClip)Resources.Load(TRANSITION);
        source = GameObject.Find("Sound").GetComponent<AudioSource>();

        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();
        gamemodeName = sceneGenerator.GamemodeName;

        player1ReadyText.text = UNREADY;
        player2ReadyText.text = UNREADY;
        player3ReadyText.text = UNREADY;
        player4ReadyText.text = UNREADY;

        startToContinueText.text = "";

        SimpleInput.MapPlayersToDefaultPref();
    }

    void Update() {
        if (FadeOutBool)
        {
            FadeOut();
        }
        if (SimpleInput.GetButtonDown("Pause", 1) && (player1ReadyText.text == READY) && !pressed) {
            StartCoroutine(LoadScene());
        } else {
            checkForReadyPlayers();
        }
    }

    private IEnumerator LoadScene() {
        // Configure Controls (Player Testing Order Matters)
        pressed = true;
        FadeOutBool = true;
        pressed = true;
        PlaySound();
        yield return new WaitWhile(() => source.isPlaying);

        SimpleInput.ClearCurrentPlayerDevices();

        if (player1ReadyText.text == READY) {
            SimpleInput.MapPlayerToDevice(1);
        }

        if (player2ReadyText.text == READY) {
            SimpleInput.MapPlayerToDevice(2);
        }

        if (player3ReadyText.text == READY) {
            SimpleInput.MapPlayerToDevice(3);
        }

        if (player4ReadyText.text == READY) {
            SimpleInput.MapPlayerToDevice(4);
        }

        sceneGenerator.LoadScene();
    }

    private void checkForReadyPlayers() {

        if (SimpleInput.GetAnyButtonDown(1) && (player1ReadyText.text == UNREADY)) {
            player1ReadyText.text = READY;
            startToContinueText.text = "Press Start to Continue!";
        } else if ((SimpleInput.GetAnyButtonDown(1) && !SimpleInput.GetButtonDown("Pause", 1)) && (player1ReadyText.text == READY)) {
            player1ReadyText.text = UNREADY;
            startToContinueText.text = "";
        }

        if (SimpleInput.GetAnyButtonDown(2) && (player2ReadyText.text == UNREADY)) {
            player2ReadyText.text = READY;
        } else if (SimpleInput.GetAnyButtonDown(2) && (player2ReadyText.text == READY)) {
            player2ReadyText.text = UNREADY;
        }

        if (SimpleInput.GetAnyButtonDown(3) && (player3ReadyText.text == UNREADY)) {
            player3ReadyText.text = READY;
        } else if (SimpleInput.GetAnyButtonDown(3) && (player3ReadyText.text == READY)) {
            player3ReadyText.text = UNREADY;
        }

        if (SimpleInput.GetAnyButtonDown(4) && (player4ReadyText.text == UNREADY)) {
            player4ReadyText.text = READY;
        } else if (SimpleInput.GetAnyButtonDown(4) && (player4ReadyText.text == READY)) {
            player4ReadyText.text = UNREADY;
        }
    }

    private int NumberOfReadyPlayers() {
        int count = 0;

        if (player1ReadyText.text == READY) {
            count++;
        }

        if (player2ReadyText.text == READY) {
            count++;
        }

        if (player3ReadyText.text == READY) {
            count++;
        }

        if (player4ReadyText.text == READY) {
            count++;
        }

        return count;
    }

    private void PlaySound()
    {
        source.clip = transition;
        source.loop = false;
        source.Play();
    }


    private void FadeOut()
    {
        alpha += fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        Color col = img.color;
        col.a = alpha;
        img.color = col;
        if (alpha == 1)
        {
            loading.enabled = true;
            FadeOutBool = false;
        }
    }

}
