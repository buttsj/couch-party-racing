﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGenerator : MonoBehaviour
{

    private const string AI_KART_PATH = "Prefabs/Karts/AIKart";
    private const string AI_KART_TOT_PATH = "Prefabs/Karts/TotAIKart";
    private const string KART_PATH = "Prefabs/Karts/KartUpdated";
    private const string CITY_CAR_PATH = "Prefabs/Karts/CityCar";
    private const string HEARSE_PATH = "Prefabs/Karts/HearseUpdated";
    private const string TAXI_CAR_PATH = "Prefabs/Karts/TaxiUpdated";
    private const string MUSCLE_CAR_PATH = "Prefabs/Karts/RareUpdated";

    private const string UI_PREFAB_PATH = "Prefabs/UI Prefabs/";
    private const string RACING_HUD_PATH = "Prefabs/UI Prefabs/Racing UI/";
    private const string SPUD_HUD_PATH = "Prefabs/UI Prefabs/SpudRun UI/";
    private const string TOT_HUD_PATH = "Prefabs/UI Prefabs/TotShot UI/";

    private const string SPAWN_POINT_NAME = "SpawnPoint";
    private const string USER_TRACK_NAME = "UserTrack";
    private const int CAMERA_FOLLOW_DISTANCE = 25;
    private const int MAX_PLAYERS = 4;

    public string LevelName { get; set; }
    public string SceneName { get; set; }
    public string GamemodeName { get; set; }
    public List<int> ReadyPlayerIndexes { get; set; }

    public bool Spectator { get; set; }

    private LevelGenerator levelGenerator;
    private GameObject startObj;

    private List<GameObject> kartList;
    private List<Vector3> kartStartListRaceMode = new List<Vector3>() { new Vector3(-55, 122, -57), new Vector3(42, 122, 40), new Vector3(42, 122, -50), new Vector3(-52, 122, 42) };
    private List<Vector3> kartStartRotationSpudRun = new List<Vector3>() { new Vector3(0, 45, 0), new Vector3(0, 225, 0), new Vector3(0, -45, 0), new Vector3(0, -225, 0) };
    private GameObject singlePlayerCamera;

    private List<Color> kartColorList = new List<Color>();
    public Color KartColorizer { set { kartColorList.Add(value); } }

    private List<string> playerKartList = new List<string>();
    public string KartDesigner { set { playerKartList.Add(value); } }

    public void ClearColors() { kartColorList.Clear(); }
    public void ClearKarts() { playerKartList.Clear(); }

    void Awake()
    {
        DontDestroyOnLoad(this);
        ReadyPlayerIndexes = new List<int>();
    }

    void Update()
    {
        if (IsLoaded() && !Spectator) {
            GenerateLevel();

            if (GamemodeName != "TrackBuilder") {  
                GeneratePlayers();
                GenerateCameras();
                GenerateAI();
                GenerateHUD();
                GenerateMinimap();
            }

            DestroyGenerator();
        }
        else if (IsLoaded() && Spectator)
        {
            GenerateLevel();
            GenerateSpectator();
            //GeneratePlayers();
            GenerateCameras();
            GenerateAI();
            //GenerateHUD();
            GenerateMinimap();
            DestroyGenerator();
        }
    }

    private void GenerateHUD()
    {
        if (GamemodeName == "RaceMode")
        {
            GenerateRacingHUD(SimpleInput.NumberOfPlayers);
            GenerateRacingArrows();
        }
        else if (GamemodeName == "SpudRun")
        {
            GenerateSpudRunHUD(SimpleInput.NumberOfPlayers);
        }
        else if (GamemodeName == "TotShot")
        {
            GenerateTotShotHUD(SimpleInput.NumberOfPlayers);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName);
    }

    private void GenerateLevel()
    {
        if (LevelName != null)
        {
            GameObject trackParent = GameObject.Find(USER_TRACK_NAME);
            if (GamemodeName == "TrackBuilder" && trackParent != null) {
                trackParent.name = LevelName;
            } else {
                trackParent = new GameObject(LevelName);
            }

            levelGenerator = new LevelGenerator(trackParent.transform);
            startObj = levelGenerator.GenerateLevel(LevelName);

            if (GamemodeName == "TrackBuilder") {
                TrackToGridSpawner trackToGrid = new TrackToGridSpawner(trackParent);
                trackToGrid.DisableColliders(trackParent);

                foreach (Transform child in trackParent.transform) {
                    trackToGrid.AddColliders(child.gameObject, child.gameObject);
                }
            }

            kartStartListRaceMode.Clear();
            RetrieveStartPositions();
        }
        else
        {
            startObj = new GameObject();
        }
    }

    private void RetrieveStartPositions()
    {
        foreach (Transform child in startObj.transform)
        {
            if (child.name == SPAWN_POINT_NAME)
            {
                kartStartListRaceMode.Add(child.position);
            }
        }
    }

    private void GenerateMinimap()
    {
        if (GamemodeName == "RaceMode")
        {
            InitializeRaceModeMinimap();
        }
        else if (GamemodeName == "SpudRun")
        {
            InitializeSpudRunMinimap();
        }
    }

    private void InitializeRaceModeMinimap()
    {
        GameObject minimap = GameObject.Find("RaceModeMinimap");

        if (minimap)
        {
            for (int i = 0; i < kartList.Count; i++)
            {
                var icon = minimap.transform.GetChild(i).gameObject;
                icon.GetComponent<MinimapFollowObject>().followObj = kartList[i].transform;
                icon.SetActive(true);
                icon.GetComponent<Renderer>().material.color = kartColorList[i];
            }

            switch (SimpleInput.NumberOfPlayers)
            {
                case 3:
                    minimap.GetComponent<Camera>().rect = new Rect(0.785f, 0.03f, .2f, .26f);
                    break;
                case 4:
                    minimap.GetComponent<Camera>().rect = new Rect(.4f, 0.4f, .2f, .26f);
                    break;
            }

        }
    }

    private void InitializeSpudRunMinimap()
    {
        GameObject minimap = GameObject.Find("SpudRunMinimap");
        if (minimap)
        {
            for (int i = 0; i < kartList.Count; i++)
            {
                var icon = minimap.transform.GetChild(i).gameObject;
                icon.GetComponent<MinimapFollowObject>().followObj = kartList[i].transform;
                icon.SetActive(true);
                icon.GetComponent<Renderer>().material.color = kartColorList[i];
            }

            var spudIcon = minimap.transform.FindChild("SpudIcon").gameObject;
            spudIcon.GetComponent<MinimapFollowObject>().followObj = GameObject.Find("Potato").transform;
            spudIcon.SetActive(true);
            switch (SimpleInput.NumberOfPlayers)
            {
                case 3:
                    minimap.GetComponent<Camera>().rect = new Rect(0.785f, 0.03f, .2f, .26f);
                    break;
                case 4:
                    minimap.GetComponent<Camera>().rect = new Rect(.4f, 0.4f, .2f, .26f);
                    break;
            }

        }

    }

    private void GenerateKart(int kartNumber, string destination)
    {
        Vector3 startPos;
        Quaternion startAngle;
        if (GamemodeName == "TotShot")
        {
            Color teamColor = kartColorList[ReadyPlayerIndexes[kartNumber]];
            Quaternion teamOrientation = Quaternion.identity;
            Vector3 teamStartPosition = Vector3.zero;

            List<Vector3> redSpawnList = new List<Vector3>() { new Vector3(-45, 1, -140), new Vector3(45, 1, -140), new Vector3(0, 1, -140) };
            List<Vector3> blueSpawnList = new List<Vector3>() { new Vector3(-45, 1, 140), new Vector3(45, 1, 140), new Vector3(0, 1, 140) };



            int numberOfRedKarts = 0;
            for (int i = kartNumber; i >= 0; i--)
            {
                if (kartColorList[ReadyPlayerIndexes[i]] == Color.red)
                {
                    numberOfRedKarts++;
                }
            }

            if (teamColor == Color.red)
            {
                teamStartPosition = redSpawnList[numberOfRedKarts - 1];
                teamOrientation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }
            else if (teamColor == Color.blue)
            {
                teamStartPosition = blueSpawnList[kartNumber - numberOfRedKarts];
                teamOrientation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            }

            startPos = startObj.transform.position + teamStartPosition;
            startAngle = teamOrientation;
            kartList.Add(Instantiate(Resources.Load<GameObject>(destination), startPos, startAngle));
            kartList[kartNumber].GetComponentInChildren<Renderer>().material.color = teamColor;

        }
        else
        {
            int readyPlayerAdustedKartNumber = kartNumber;
            if (kartNumber < SimpleInput.NumberOfPlayers)
            {
                readyPlayerAdustedKartNumber = ReadyPlayerIndexes[kartNumber];
            }

            startPos = kartStartListRaceMode[kartNumber];
            if (GamemodeName == "RaceMode")
            {
                startAngle = Quaternion.Euler(startObj.transform.rotation.eulerAngles + new Vector3(0f, 90f, 0f));
            }
            else {
                startAngle = Quaternion.Euler(kartStartRotationSpudRun[kartNumber]);
            }
            kartList.Add(Instantiate(Resources.Load<GameObject>(destination), startPos, startAngle));
            kartList[kartNumber].GetComponentInChildren<Renderer>().material.color = kartColorList[readyPlayerAdustedKartNumber];
        }

    }

    private bool IsUniqueColor(Color color)
    {
        bool isUnique = false;

        foreach (var kart in kartList)
        {
            isUnique = kart.GetComponentInChildren<Renderer>().material.color != color;
            if (!isUnique)
            {
                break;
            }

            /*if (kart.GetComponentInChildren<Renderer>().material.color != color) {
                count
            }*/
        }

        return isUnique;
    }

    private void GenerateAI()
    {
        if (GamemodeName == "RaceMode")
        {
            for (int i = kartList.Count; i < MAX_PLAYERS; i++)
            {
                GenerateKart(i, AI_KART_PATH);
                kartList[i].name = "AI" + (i + 1);
                kartList[i].GetComponent<WaypointAI>().GameState = new RacingGameState(kartList[i]);
            }
            WaypointSetter.SetWaypoints();
            if (Spectator)
            {
                singlePlayerCamera.GetComponent<PlayerCamera>().players = kartList.ToArray();
            }
        }

    }

    private void GenerateSpectator()
    {
        kartList = new List<GameObject>();
        if (GamemodeName != "TrackBuilder" && GamemodeName != "ChipShop")
        {
            GenerateKart(0, AI_KART_PATH);
            kartList[0].name = "AI" + (0 + 1);
            kartList[0].GetComponent<WaypointAI>().GameState = new RacingGameState(kartList[0]);
        }
    }

    private void GeneratePlayers()
    {
        kartList = new List<GameObject>();

        if (GamemodeName != "TrackBuilder" && GamemodeName != "ChipShop")
        {
            for (int i = 0; i < SimpleInput.NumberOfPlayers; i++)
            {
                string kart = playerKartList[ReadyPlayerIndexes[i]];
                switch (kart)
                {
                    case "Default":
                        GenerateKart(i, KART_PATH);
                        break;
                    case "Taxi":
                        GenerateKart(i, TAXI_CAR_PATH);
                        break;
                    case "Hearse":
                        GenerateKart(i, HEARSE_PATH);
                        break;
                    case "CityCar":
                        GenerateKart(i, CITY_CAR_PATH);
                        break;
                    case "Muscle":
                        GenerateKart(i, MUSCLE_CAR_PATH);
                        break;
                }
                kartList[i].name = "Player " + (i + 1);
                kartList[i].GetComponent<Kart>().PlayerNumber = i + 1;
            }
        }

        switch (GamemodeName)
        {
            case "RaceMode":
                for (int i = 0; i < SimpleInput.NumberOfPlayers; i++)
                {
                    kartList[i].GetComponent<Kart>().GameState = new RacingGameState(kartList[i]);
                    kartList[i].GetComponent<Kart>().IsRacingGameState = true;
                    kartList[i].GetComponent<Kart>().IsTotShotGameState = false;
                }
                break;
            case "SpudRun":
                for (int i = 0; i < SimpleInput.NumberOfPlayers; i++)
                {
                    kartList[i].GetComponent<Kart>().GameState = new SpudRunGameState(kartList[i]);
                    kartList[i].GetComponent<Kart>().IsRacingGameState = false;
                    kartList[i].GetComponent<Kart>().IsTotShotGameState = false;
                }
                break;
            case "TotShot":
                for (int i = 0; i < SimpleInput.NumberOfPlayers; i++)
                {
                    string teamColorString = kartList[i].GetComponentInChildren<Renderer>().material.color == Color.red ? "red" : "blue";

                    kartList[i].GetComponent<Kart>().GameState = new TotShotGameState(kartList[i], teamColorString);
                    kartList[i].GetComponent<Kart>().IsRacingGameState = false;
                    kartList[i].GetComponent<Kart>().IsTotShotGameState = true;
                }
                break;
        }
    }

    private void GenerateCameras()
    {
        Rect FULL_SCREEN = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        Rect BOT_HALF = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
        Rect TOP_HALF = new Rect(0.0f, 0.5f, 1.0f, 0.5f);

        Rect BOT_LEFT = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
        Rect TOP_LEFT = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
        Rect BOT_RIGH = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        Rect TOP_RIGH = new Rect(0.5f, 0.5f, 0.5f, 0.5f);

        switch (SimpleInput.NumberOfPlayers)
        {
            case 1:
                singlePlayerCamera = CreateCamera("Camera (Player 1)", FULL_SCREEN, 1);
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

    private GameObject CreateCamera(string cameraName, Rect rect, int playerNumber)
    {
        GameObject camera = new GameObject(cameraName);

        camera.SetActive(false);

        camera.AddComponent<Camera>().rect = rect;
        camera.GetComponent<Camera>().farClipPlane = 5000;

        if (GamemodeName != "TrackBuilder" && GamemodeName != "ChipShop")
        {
            camera.AddComponent<PlayerCamera>().player = kartList[playerNumber - 1].transform;
            camera.GetComponent<PlayerCamera>().followDistance = CAMERA_FOLLOW_DISTANCE;
            camera.AddComponent<AudioListener>();
            if (Spectator)
                camera.GetComponent<PlayerCamera>().Spectate = true;
        }

        camera.SetActive(true);

        switch (playerNumber)
        {
            case 1:
                camera.GetComponent<Camera>().cullingMask ^= (1 << 10);
                camera.GetComponent<Camera>().cullingMask ^= (1 << 11);
                camera.GetComponent<Camera>().cullingMask ^= (1 << 12);
                break;
            case 2:
                camera.GetComponent<Camera>().cullingMask ^= (1 << 9);
                camera.GetComponent<Camera>().cullingMask ^= (1 << 11);
                camera.GetComponent<Camera>().cullingMask ^= (1 << 12);
                break;
            case 3:
                camera.GetComponent<Camera>().cullingMask ^= (1 << 10);
                camera.GetComponent<Camera>().cullingMask ^= (1 << 9);
                camera.GetComponent<Camera>().cullingMask ^= (1 << 12);
                break;
            case 4:
                camera.GetComponent<Camera>().cullingMask ^= (1 << 10);
                camera.GetComponent<Camera>().cullingMask ^= (1 << 11);
                camera.GetComponent<Camera>().cullingMask ^= (1 << 9);
                break;
        }
        return camera;
    }

    private bool IsLoaded()
    {
        return SceneName == SceneManager.GetActiveScene().name;
    }

    private void DestroyGenerator()
    {
        Destroy(gameObject);
    }

    private void GenerateRacingHUD(int numberOfPlayers)
    {
        GameObject hud;
        switch (numberOfPlayers)
        {
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
        if (CouchPartyManager.IsCouchPartyMode)
        {
            Instantiate(Resources.Load<GameObject>(RACING_HUD_PATH + "CouchPartyRacingEndMenu"), Vector3.zero, Quaternion.Euler(Vector3.zero));
        }
        else {
            Instantiate(Resources.Load<GameObject>(RACING_HUD_PATH + "RacingEndMenu"), Vector3.zero, Quaternion.Euler(Vector3.zero));
        }
        Instantiate(Resources.Load<GameObject>(UI_PREFAB_PATH + "CountdownTimer"), Vector3.zero, Quaternion.Euler(Vector3.zero));
        Instantiate(Resources.Load<GameObject>(UI_PREFAB_PATH + "PauseMenu"), Vector3.zero, Quaternion.Euler(Vector3.zero));
    }

    private void GenerateSpudRunHUD(int numberOfPlayers)
    {
        GameObject hud;
        switch (numberOfPlayers)
        {
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
                hud.GetComponent<FourPlayerSpudHUD>().kart3 = kartList[2];
                hud.GetComponent<FourPlayerSpudHUD>().kart4 = kartList[3];
                hud.GetComponent<FourPlayerSpudHUD>().potato = GameObject.Find("Potato");
                break;
        }
        if (CouchPartyManager.IsCouchPartyMode)
        {
            Instantiate(Resources.Load<GameObject>(SPUD_HUD_PATH + "CouchPartySpudRunEndMenu"), Vector3.zero, Quaternion.Euler(Vector3.zero));
        }else
        {
            Instantiate(Resources.Load<GameObject>(SPUD_HUD_PATH + "SpudRunEndMenu"), Vector3.zero, Quaternion.Euler(Vector3.zero));
        }

        Instantiate(Resources.Load<GameObject>(UI_PREFAB_PATH + "PauseMenu"), Vector3.zero, Quaternion.Euler(Vector3.zero));

    }

    private void GenerateTotShotHUD(int numberOfPlayers)
    {
        GameObject timeScoreHUD;
        GameObject boostHUD;
        GameObject countdownTimer;
        Instantiate(Resources.Load<GameObject>(UI_PREFAB_PATH + "PauseMenu"), Vector3.zero, Quaternion.Euler(Vector3.zero));
        timeScoreHUD = Instantiate(Resources.Load<GameObject>(TOT_HUD_PATH + "TotShotHUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
        countdownTimer = Instantiate(Resources.Load<GameObject>(UI_PREFAB_PATH + "CountdownTimer"), Vector3.zero, Quaternion.Euler(Vector3.zero));
        switch (numberOfPlayers)
        {
            case 1:
                boostHUD = Instantiate(Resources.Load<GameObject>(TOT_HUD_PATH + "Single Player Tot HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                boostHUD.GetComponent<SinglePlayerTotHUD>().kart1 = kartList[0];
                break;
            case 2:
                boostHUD = Instantiate(Resources.Load<GameObject>(TOT_HUD_PATH + "Two Player Tot HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                boostHUD.GetComponent<TwoPlayerTotHUD>().kart1 = kartList[0];
                boostHUD.GetComponent<TwoPlayerTotHUD>().kart2 = kartList[1];
                break;
            case 3:
                boostHUD = Instantiate(Resources.Load<GameObject>(TOT_HUD_PATH + "Three Player Tot HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                boostHUD.GetComponent<ThreePlayerTotHUD>().kart1 = kartList[0];
                boostHUD.GetComponent<ThreePlayerTotHUD>().kart2 = kartList[1];
                boostHUD.GetComponent<ThreePlayerTotHUD>().kart3 = kartList[2];
                break;
            case 4:
                boostHUD = Instantiate(Resources.Load<GameObject>(TOT_HUD_PATH + "Four Player Tot HUD"), Vector3.zero, Quaternion.Euler(Vector3.zero));
                boostHUD.GetComponent<FourPlayerTotHUD>().kart1 = kartList[0];
                boostHUD.GetComponent<FourPlayerTotHUD>().kart2 = kartList[1];
                boostHUD.GetComponent<FourPlayerTotHUD>().kart3 = kartList[2];
                boostHUD.GetComponent<FourPlayerTotHUD>().kart4 = kartList[3];
                break;
        }
    }

    private void CreateRacingArrow(int playerNumber, string arrowName)
    {
        GameObject arrow = Instantiate(Resources.Load<GameObject>("Prefabs/Checkpoint_Arrow"), Vector3.zero, Quaternion.Euler(Vector3.zero));
        arrow.name = arrowName;
        arrow.GetComponent<CheckpointArrow>().kart = kartList[playerNumber];

        switch (playerNumber)
        {
            case 0:
                arrow.layer = LayerMask.NameToLayer("Player 1");
                arrow.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player 1");
                arrow.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Player 1");
                arrow.transform.GetChild(2).gameObject.layer = LayerMask.NameToLayer("Player 1");
                break;
            case 1:
                arrow.layer = LayerMask.NameToLayer("Player 2");
                arrow.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player 2");
                arrow.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Player 2");
                arrow.transform.GetChild(2).gameObject.layer = LayerMask.NameToLayer("Player 2");
                break;
            case 2:
                arrow.layer = LayerMask.NameToLayer("Player 3");
                arrow.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player 3");
                arrow.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Player 3");
                arrow.transform.GetChild(2).gameObject.layer = LayerMask.NameToLayer("Player 3");
                break;
            case 3:
                arrow.layer = LayerMask.NameToLayer("Player 4");
                arrow.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player 4");
                arrow.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Player 4");
                arrow.transform.GetChild(2).gameObject.layer = LayerMask.NameToLayer("Player 4");
                break;
        }
    }

    private void GenerateRacingArrows()
    {
        switch (SimpleInput.NumberOfPlayers)
        {
            case 1:
                CreateRacingArrow(0, "Player 1 Arrow");
                break;
            case 2:
                CreateRacingArrow(0, "Player 1 Arrow");
                CreateRacingArrow(1, "Player 2 Arrow");
                break;
            case 3:
                CreateRacingArrow(0, "Player 1 Arrow");
                CreateRacingArrow(1, "Player 2 Arrow");
                CreateRacingArrow(2, "Player 3 Arrow");
                break;
            case 4:
                CreateRacingArrow(0, "Player 1 Arrow");
                CreateRacingArrow(1, "Player 2 Arrow");
                CreateRacingArrow(2, "Player 3 Arrow");
                CreateRacingArrow(3, "Player 4 Arrow");
                break;
        }
    }
}
