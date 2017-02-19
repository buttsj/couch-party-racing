using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectionMenuFunctionality : MonoBehaviour {
   
    public GameObject RaceMode;
    public GameObject SpudRun;

    private SceneGenerator sceneGenerator;

    void Start() {
        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();

        DetermineLevelMenu();
    }

    void Update() {

    }

    private void DetermineLevelMenu() {
        var gamemodeName = sceneGenerator.GamemodeName;

        switch (gamemodeName) {
            case "RaceMode":
                RaceMode.SetActive(true);
                break;
            case "SpudRun":
                SpudRun.SetActive(true);
                break;
        }
    }
}
