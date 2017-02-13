﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGenerator : MonoBehaviour {
    private const string TRACK_SCENE_NAME = "DemoScene";
    private const string KART_PATH = "Prefabs/Karts/KartUpdated";
    private const string HUD_PATH = "Prefabs/UI Prefabs/";
    private const int CAMERA_FOLLOW_DISTANCE = 20;
    private const int MAX_PLAYERS = 4;

    public string LevelName { get; set; }
    public string SceneName { get; set; }
    public string GamemodeName { get; set; }


    private LevelGenerator levelGenerator;
    private List<GameObject> kartList;
    private List<Vector3> kartPosList = new List<Vector3>() { new Vector3(30, 1, -10), new Vector3(30, 1, 10), new Vector3(10, 1, -10), new Vector3(10, 1, 10)};
    private List<Color> kartColorList = new List<Color> { Color.red, Color.magenta, Color.green, Color.yellow};

    void Awake () {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (IsLoaded()) {
            GenerateLevel();
            GeneratePlayers();
            GenerateCameras();
            GenerateAI();
            GenerateHUD(SimpleInput.NumberOfPlayers);

            DestroyGenerator();
        }
	}

    public void LoadScene() {
        SceneManager.LoadScene(SceneName);
    }

    private void GenerateLevel() {
        if (LevelName != null) {
            GameObject track = new GameObject(LevelName);
            levelGenerator = new LevelGenerator(track.transform);
            levelGenerator.GenerateLevel(LevelName);
        }
    }

    private void GenerateAI() {
        for (int i = kartList.Count; i < MAX_PLAYERS; i++) {
            kartList.Add(Instantiate(Resources.Load<GameObject>(KART_PATH), kartPosList[i], Quaternion.Euler(new Vector3(0, -90, 0))));
            kartList[i].GetComponent<Kart>().enabled = false;
            kartList[i].GetComponent<WaypointAI>().enabled = true;
            kartList[i].name = "AIPlayer" + (i + 1);
            kartList[i].GetComponentInChildren<Renderer>().material.color = kartColorList[i];
            kartList[i].transform.FindChild("MinimapColor").GetComponentInChildren<Renderer>().material.color = kartColorList[i];
        }
    }

    private void GeneratePlayers() {
        kartList = new List<GameObject>();

        for (int i = 0; i < SimpleInput.NumberOfPlayers; i++) {
            kartList.Add(Instantiate(Resources.Load<GameObject>(KART_PATH), kartPosList[i], Quaternion.Euler(new Vector3(0, -90, 0))));
            kartList[i].GetComponent<Kart>().enabled = true;
            kartList[i].GetComponent<WaypointAI>().enabled = false;
            kartList[i].name = "Player " + (i + 1);
            kartList[i].GetComponent<Kart>().PlayerNumber = i + 1;
            kartList[i].GetComponentInChildren<Renderer>().material.color = kartColorList[i];
            kartList[i].transform.FindChild("MinimapColor").GetComponentInChildren<Renderer>().material.color = kartColorList[i];
        }
    }

    private void GenerateCameras() {
        Rect FULL_SCREEN = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        Rect BOT_HALF = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
        Rect TOP_HALF = new Rect(0.0f, 0.5f, 1.0f, 0.5f);

        Rect BOT_LEFT = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
        Rect TOP_LEFT = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
        Rect BOT_RIGH = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        Rect TOP_RIGH = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        
        switch (SimpleInput.NumberOfPlayers) {
            case 1:
                CreateCamera("Camera (Player 1)", FULL_SCREEN, 1);
                break;
            case 2:
                CreateCamera("Camera (Player 1)", TOP_HALF, 1);
                CreateCamera("Camera (Player 2)", BOT_HALF, 2);
                break;
            case 3:
                CreateCamera("Camera (Player 1)", TOP_LEFT, 1);
                CreateCamera("Camera (Player 2)", TOP_RIGH, 2);
                CreateCamera("Camera (Player 3)", BOT_HALF, 3);
                break;
            case 4:
                CreateCamera("Camera (Player 1)", TOP_LEFT, 1);
                CreateCamera("Camera (Player 2)", TOP_RIGH, 2);
                CreateCamera("Camera (Player 3)", BOT_LEFT, 3);
                CreateCamera("Camera (Player 4)", BOT_RIGH, 4);
                break;
        }
    }

    private GameObject CreateCamera(string cameraName, Rect rect, int playerNumber) {
        GameObject camera = new GameObject(cameraName);

        camera.SetActive(false);

        camera.AddComponent<Camera>().rect = rect;
        camera.GetComponent<Camera>().farClipPlane = 5000;
        camera.AddComponent<PlayerCamera>().player = kartList[playerNumber - 1].transform;
        camera.GetComponent<PlayerCamera>().followDistance = CAMERA_FOLLOW_DISTANCE;
        camera.AddComponent<AudioListener>();

        camera.SetActive(true);

       
        return camera;
    }

    private bool IsLoaded() {
        return SceneName == SceneManager.GetActiveScene().name;
    }

    private void DestroyGenerator() {
        Destroy(gameObject);
    }

    private void GenerateHUD(int numberOfPlayers) {
        GameObject hud;
        switch (numberOfPlayers) {
            case 1:
                hud = Instantiate(Resources.Load<GameObject>(HUD_PATH + "Single Player HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                hud.GetComponent<HUDManager>().kart = kartList[0];
                break;
            case 2:
                hud = Instantiate(Resources.Load<GameObject>(HUD_PATH + "Two Player HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                hud.GetComponent<TwoPlayerHUDManager>().kart1 = kartList[0];
                hud.GetComponent<TwoPlayerHUDManager>().kart2 = kartList[1];
                break;
            case 3:
                hud = Instantiate(Resources.Load<GameObject>(HUD_PATH + "Two Player HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                hud.GetComponent<ThreePlayerHUDManager>().kart1 = kartList[0];
                hud.GetComponent<ThreePlayerHUDManager>().kart2 = kartList[1];
                hud.GetComponent<ThreePlayerHUDManager>().kart3 = kartList[2];
                break;
            case 4:
                hud = Instantiate(Resources.Load<GameObject>(HUD_PATH + "Two Player HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                hud.GetComponent<FourPlayerHUDManager>().kart1 = kartList[0];
                hud.GetComponent<FourPlayerHUDManager>().kart2 = kartList[1];
                hud.GetComponent<FourPlayerHUDManager>().kart3 = kartList[2];
                hud.GetComponent<FourPlayerHUDManager>().kart4 = kartList[3];
                break;
        }
    }
    
}
