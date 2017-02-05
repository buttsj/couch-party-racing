using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGenerator : MonoBehaviour {
    private const string TRACK_SCENE_NAME = "Temp_Scene";
    private const string KART_PATH = "Prefabs/Karts/KartUpdated";
    private const string AI_KART_PATH = "Prefabs/Karts/AIKart";
    private const int CAMERA_FOLLOW_DISTANCE = 20;
    private const int MAX_PLAYERS = 1;

    public string LevelName { get; set; }

    private LevelGenerator levelGenerator;
    private List<GameObject> kartList;
    private List<Color> kartColorList = new List<Color> { Color.red, Color.blue, Color.green, Color.yellow};

    void Awake () {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (IsLoaded()) {
            //GameObject track = new GameObject(LevelName);
            //levelGenerator = new LevelGenerator(track.transform);
            //levelGenerator.GenerateLevel(LevelName);
            GeneratePlayers();
            GenerateCameras();
            GenerateAI();
            DestroyGenerator();
        }
	}

    public void LoadLevel(string levelName) {
        LevelName = levelName;
        SceneManager.LoadScene(TRACK_SCENE_NAME);
    }

    private void GenerateAI() {
        for (int i = kartList.Count; i < MAX_PLAYERS; i++) {
            kartList.Add(Instantiate(Resources.Load<GameObject>(AI_KART_PATH), new Vector3(15, 15, 15 * i * 2), Quaternion.Euler(Vector3.zero)));
            kartList[i].name = "AI " + (i + 1);
            //kartList[i].GetComponent<Kart>().PlayerNumber = i + 1;
            kartList[i].GetComponentInChildren<Renderer>().material.color = kartColorList[i];
            kartList[i].transform.FindChild("MinimapColor").GetComponentInChildren<Renderer>().material.color = kartColorList[i];
        }
    }

    private void GeneratePlayers() {
        kartList = new List<GameObject>();

        for (int i = 0; i < SimpleInput.NumberOfPlayers; i++) {
            kartList.Add(Instantiate(Resources.Load<GameObject>(KART_PATH), new Vector3(15,15,15 * i * 2), Quaternion.Euler(Vector3.zero)));
            kartList[i].name = "Player " + (i+1);
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
        camera.AddComponent<PlayerCamera>().player = kartList[playerNumber - 1].transform;
        camera.GetComponent<PlayerCamera>().followDistance = CAMERA_FOLLOW_DISTANCE;

        camera.SetActive(true);

        return camera;
    }

    private bool IsLoaded() {
        return TRACK_SCENE_NAME == SceneManager.GetActiveScene().name;
    }

    private void DestroyGenerator() {
        Destroy(gameObject);
    }
}
