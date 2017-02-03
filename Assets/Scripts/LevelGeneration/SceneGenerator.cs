using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGenerator : MonoBehaviour {
    public string LevelName { get; set; }

    private LevelGenerator levelGenerator;
    private const string TRACK_SCENE_NAME = "InputTestingScene";

    void Awake () {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (IsLoaded()) {
            GameObject track = new GameObject(LevelName);
            levelGenerator = new LevelGenerator(track.transform);
            levelGenerator.GenerateLevel(LevelName);
            GenerateCameras();
            DestroyGenerator();
        }
	}

    public void LoadLevel(string levelName) {
        LevelName = levelName;
        SceneManager.LoadScene(TRACK_SCENE_NAME);
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
                CreateCamera("Camera (Player 1)", FULL_SCREEN);
                break;
            case 2:
                CreateCamera("Camera (Player 1)", TOP_HALF);
                CreateCamera("Camera (Player 2)", BOT_HALF);
                break;
            case 3:
                CreateCamera("Camera (Player 1)", TOP_LEFT);
                CreateCamera("Camera (Player 2)", TOP_RIGH);
                CreateCamera("Camera (Player 3)", BOT_HALF);
                break;
            case 4:
                CreateCamera("Camera (Player 1)", TOP_LEFT);
                CreateCamera("Camera (Player 2)", TOP_RIGH);
                CreateCamera("Camera (Player 3)", BOT_LEFT);
                CreateCamera("Camera (Player 4)", BOT_RIGH);
                break;
        }
    }

    private GameObject CreateCamera(string cameraName, Rect rect) {
        GameObject camera = new GameObject(cameraName);
        camera.AddComponent<Camera>().rect = rect;
        return camera;
    }

    private bool IsLoaded() {
        return TRACK_SCENE_NAME == SceneManager.GetActiveScene().name;
    }

    private void DestroyGenerator() {
        Destroy(gameObject);
    }
}
