using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGenerator : MonoBehaviour {
    private const string AI_KART_PATH = "Prefabs/Karts/AIKart";
    private const string KART_PATH = "Prefabs/Karts/KartUpdated";
    private const string UI_PREFAB_PATH = "Prefabs/UI Prefabs/";
    private const string RACING_HUD_PATH = "Prefabs/UI Prefabs/Racing UI/";
    private const string SPUD_HUD_PATH = "Prefabs/UI Prefabs/SpudRun UI/";
    private const string TOT_HUD_PATH = "Prefabs/UI Prefabs/TotShot UI/";
    private const int CAMERA_FOLLOW_DISTANCE = 20;
    private const int MAX_PLAYERS = 4;

    public string LevelName { get; set; }
    public string SceneName { get; set; }
    public string GamemodeName { get; set; }


    private LevelGenerator levelGenerator;
    private GameObject startObj;

    private List<GameObject> kartList;
    private List<Vector3> kartStartAdjList = new List<Vector3>() { new Vector3(-55, 1, 15), new Vector3(-55, 1, 45), new Vector3(-25, 1, 15), new Vector3(-25, 1, 45) };
    //private List<Color> kartColorList = new List<Color> { Color.red, Color.magenta, Color.green, Color.yellow };
    private List<Color> kartColorList = new List<Color>();
    public Color KartColorizer { set { kartColorList.Add(value); } }

    void Awake() {
        DontDestroyOnLoad(this);
    }
    
    void Update() {
        if (IsLoaded()) {
            GenerateLevel();
            GeneratePlayers();
            GenerateCameras();
            GenerateAI();
            GenerateHUD();
            InitializeMinimap();

            DestroyGenerator();
        }
    }

    private void GenerateHUD() {
        if (GamemodeName == "RaceMode") {
            GenerateRacingHUD(SimpleInput.NumberOfPlayers);
        } else if (GamemodeName == "SpudRun") {
            GenerateSpudRunHUD(SimpleInput.NumberOfPlayers);
        } else if (GamemodeName == "TotShot")
        {
            GenerateTotShotHUD(SimpleInput.NumberOfPlayers);
        }
    }

    public void LoadScene() {
        SceneManager.LoadScene(SceneName);
    }

    private void GenerateLevel() {
        if (LevelName != null) {
            GameObject track = new GameObject(LevelName);
            levelGenerator = new LevelGenerator(track.transform);
            startObj = levelGenerator.GenerateLevel(LevelName);
        } else {
            startObj = new GameObject();
        }
    }

    private void InitializeMinimap() {
        GameObject minimap = GameObject.Find("Minimap");

        if (minimap) {
            for (int i = 0; i < kartList.Count; i++) {
                var icon = minimap.transform.GetChild(i).gameObject;

                icon.SetActive(true);
                icon.GetComponent<MinimapFollowObject>().followObj = kartList[i].transform;
                icon.GetComponent<Renderer>().material.color = kartColorList[i];
            }

            switch (SimpleInput.NumberOfPlayers) {
                case 3:
                    minimap.GetComponent<Camera>().rect = new Rect(0.785f, 0.03f, .2f, .26f);
                    break;
                case 4:
                    minimap.GetComponent<Camera>().rect = new Rect(.4f, 0.4f, .2f, .26f);
                    break;
            }

        }
    }

    private void GenerateKart(int kartNumber, string destination) {
        Vector3 startPos = startObj.transform.position + kartStartAdjList[kartNumber];
        Quaternion startAngel = Quaternion.Euler(startObj.transform.rotation.eulerAngles + new Vector3(0f, 90f, 0f));

        kartList.Add(Instantiate(Resources.Load<GameObject>(destination), startPos, startAngel));
        kartList[kartNumber].GetComponentInChildren<Renderer>().material.color = kartColorList[kartNumber];
    }

    /*private Vector3 KartStartPos(int kartNumber) {
        const float AWAY_FROM_FLAG = 25;
        const float AWAY_FROM_SIDE = 15;
        const float AWAY_FROM_OTHERS = 30;
        const float HEIGHT = 1; 

        Vector3 trackPos = startObj.transform.position;
        var trackAngle = Mathf.RoundToInt(startObj.transform.rotation.eulerAngles.y);

        Vector3 kartPos = new Vector3();

        switch (trackAngle) {
            case 90:
                break;
            case 180:
                break;
            case 270:
                break;
            default:
                kartPos = trackPos + new Vector3(AWAY_FROM_FLAG, HEIGHT, AWAY_FROM_SIDE);
                break;
        }
             
        return kartPos;
    }*/

    private void GenerateAI() {
        if (GamemodeName == "RaceMode") {
            for (int i = kartList.Count; i < MAX_PLAYERS; i++) {
                GenerateKart(i, AI_KART_PATH);
                kartList[i].name = "AI" + (i + 1);
                if (GamemodeName == "RaceMode") {
                    kartList[i].GetComponent<WaypointAI>().GameState = new RacingGameState(kartList[i]);
                }
            }

            WaypointSetter.SetWaypoints();
        }
    }

    private void GeneratePlayers() {
        kartList = new List<GameObject>();

        for (int i = 0; i < SimpleInput.NumberOfPlayers; i++) {
            GenerateKart(i, KART_PATH);
            kartList[i].name = "Player " + (i + 1);
            kartList[i].GetComponent<Kart>().PlayerNumber = i + 1;
        }

        switch (GamemodeName) {
            case "RaceMode":
                for (int i = 0; i < SimpleInput.NumberOfPlayers; i++) {
                    kartList[i].GetComponent<Kart>().GameState = new RacingGameState(kartList[i]);
                    kartList[i].GetComponent<Kart>().IsRacingGameState = true;
                    kartList[i].GetComponent<Kart>().IsTotShotGameState = false;
                }
                break;
            case "SpudRun":
                for (int i = 0; i < SimpleInput.NumberOfPlayers; i++) {
                    kartList[i].GetComponent<Kart>().GameState = new SpudRunGameState(kartList[i]);
                    kartList[i].GetComponent<Kart>().IsRacingGameState = false;
                    kartList[i].GetComponent<Kart>().IsTotShotGameState = false;
                }
                break;
            case "TotShot":
                for (int i = 0; i < SimpleInput.NumberOfPlayers; i++)
                {
                    kartList[i].GetComponent<Kart>().GameState = new TotShotGameState(kartList[i]);
                    kartList[i].GetComponent<Kart>().IsRacingGameState = false;
                    kartList[i].GetComponent<Kart>().IsTotShotGameState = true;
                }
                break;
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

    private void GenerateRacingHUD(int numberOfPlayers) {
        GameObject hud;
        switch (numberOfPlayers) {
            case 1:
                hud = Instantiate(Resources.Load<GameObject>(RACING_HUD_PATH + "Single Player HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                hud.GetComponent<HUDManager>().kart = kartList[0];
                break;
            case 2:
                hud = Instantiate(Resources.Load<GameObject>(RACING_HUD_PATH + "Two Player HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                hud.GetComponent<TwoPlayerHUDManager>().kart1 = kartList[0];
                hud.GetComponent<TwoPlayerHUDManager>().kart2 = kartList[1];
                break;
            case 3:
                hud = Instantiate(Resources.Load<GameObject>(RACING_HUD_PATH + "Three Player HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                hud.GetComponent<ThreePlayerHUDManager>().kart1 = kartList[0];
                hud.GetComponent<ThreePlayerHUDManager>().kart2 = kartList[1];
                hud.GetComponent<ThreePlayerHUDManager>().kart3 = kartList[2];
                break;
            case 4:
                hud = Instantiate(Resources.Load<GameObject>(RACING_HUD_PATH + "Four Player HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                hud.GetComponent<FourPlayerHUDManager>().kart1 = kartList[0];
                hud.GetComponent<FourPlayerHUDManager>().kart2 = kartList[1];
                hud.GetComponent<FourPlayerHUDManager>().kart3 = kartList[2];
                hud.GetComponent<FourPlayerHUDManager>().kart4 = kartList[3];
                break;
        }
        Instantiate(Resources.Load<GameObject>(RACING_HUD_PATH + "RacingEndMenu"), Vector3.zero, Quaternion.Euler(Vector3.zero));
        Instantiate(Resources.Load<GameObject>(UI_PREFAB_PATH + "PauseMenu"), Vector3.zero, Quaternion.Euler(Vector3.zero));
    }

    private void GenerateSpudRunHUD(int numberOfPlayers) {
        GameObject hud;
        switch (numberOfPlayers) {
            case 2:
                hud = Instantiate(Resources.Load<GameObject>(SPUD_HUD_PATH + "Two Player Spud HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                hud.GetComponent<TwoPlayerSpudHUD>().kart1 = kartList[0];
                hud.GetComponent<TwoPlayerSpudHUD>().kart2 = kartList[1];
                hud.GetComponent<TwoPlayerSpudHUD>().potato = GameObject.Find("Potato");
                break;
            case 3:
                hud = Instantiate(Resources.Load<GameObject>(SPUD_HUD_PATH + "Three Player Spud HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                hud.GetComponent<ThreePlayerSpudHUD>().kart1 = kartList[0];
                hud.GetComponent<ThreePlayerSpudHUD>().kart2 = kartList[1];
                hud.GetComponent<ThreePlayerSpudHUD>().kart3 = kartList[2];
                hud.GetComponent<ThreePlayerSpudHUD>().potato = GameObject.Find("Potato");
                break;
            case 4:
                hud = Instantiate(Resources.Load<GameObject>(SPUD_HUD_PATH + "Four Player Spud HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                hud.GetComponent<FourPlayerSpudHUD>().kart1 = kartList[0];
                hud.GetComponent<FourPlayerSpudHUD>().kart2 = kartList[1];
                hud.GetComponent<FourPlayerSpudHUD>().kart3 = kartList[0];
                hud.GetComponent<FourPlayerSpudHUD>().kart4 = kartList[1];
                hud.GetComponent<FourPlayerSpudHUD>().potato = GameObject.Find("Potato");
                break;
        }
        Instantiate(Resources.Load<GameObject>(SPUD_HUD_PATH + "SpudRunEndMenu"), Vector3.zero, Quaternion.Euler(Vector3.zero));
        Instantiate(Resources.Load<GameObject>(UI_PREFAB_PATH + "PauseMenu"), Vector3.zero, Quaternion.Euler(Vector3.zero));

    }

    private void GenerateTotShotHUD(int numberOfPlayers)
    {
        GameObject hud;
        hud = Instantiate(Resources.Load<GameObject>(TOT_HUD_PATH + "TotShotHUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
        Instantiate(Resources.Load<GameObject>(UI_PREFAB_PATH + "PauseMenu"), Vector3.zero, Quaternion.Euler(Vector3.zero));

    }
}
