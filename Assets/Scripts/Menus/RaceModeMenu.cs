using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RaceModeMenu : MonoBehaviour {

    public Button Kitchen1;
    public Button Bedroom1;

    private SceneGenerator sceneGenerator;

    void Start() {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Kitchen 1"));

        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();
    }

    public void Kitchen1Press() {
        sceneGenerator.LevelName = "KitchenTrack1.xml";
        GoToNextMenu();
    }

    public void Bedroom1Press() {
        sceneGenerator.LevelName = "BedroomTrack.xml";
        GoToNextMenu();
    }

    private void GoToNextMenu() {
        SceneManager.LoadScene("SelectionMenu");
    }
}
