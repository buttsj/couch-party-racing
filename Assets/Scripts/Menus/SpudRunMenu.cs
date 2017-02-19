using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpudRunMenu : MonoBehaviour {

    public Button Arena1;

    private SceneGenerator sceneGenerator;

    void Start() {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Arena 1"));

        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();
    }

    public void Arena1Press() {
        //sceneGenerator.LevelName = "Arena1.xml";
        GoToNextMenu();
    }

    private void GoToNextMenu() {
        SceneManager.LoadScene("SelectionMenu");
    }
}
