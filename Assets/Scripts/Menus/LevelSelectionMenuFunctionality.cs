using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectionMenuFunctionality : MonoBehaviour {

    public Button Kitchen1;
    public Button Bedroom1;

    private SceneGenerator sceneGenerator;

    void Start() {

        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Kitchen 1"));

        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();
    }

    void Update() {

    }

    public void Kitchen1Press() {
        sceneGenerator.LevelName = "KitchenTrack1.xml";
        GoToLevel();
    }

    public void Bedroom1Press() {
        sceneGenerator.LevelName = "BedroomTrack.xml";
        GoToLevel();
    }

    private void GoToLevel() {
        SceneManager.LoadScene(sceneGenerator.SceneName);
    }
}
