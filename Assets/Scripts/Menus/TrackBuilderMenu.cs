using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrackBuilderMenu : MonoBehaviour {

    public Button NewTrack;
    public Button LoadTrack;

    private SceneGenerator sceneGenerator;

    void Start() {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("NewTrack"));

        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();

        SimpleInput.ClearCurrentPlayerDevices();
        SimpleInput.MapPlayerToDevice(1);

        Destroy(sceneGenerator.gameObject);
    }

    public void NewTrackPress() {
        //sceneGenerator.LevelName = "Basement.xml";
        GoToNextMenu();
    }

    public void LoadTrackPress(InputField userInputField) {
        //sceneGenerator.LevelName = userInputField.text;
        GoToNextMenu();
    }

    private void GoToNextMenu() {
        SceneManager.LoadScene("TrackBuilderScene");
    }
}
