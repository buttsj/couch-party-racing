using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RaceModeMenu : MonoBehaviour {

    private const string TRANSITION = "Sounds/KartEffects/screen_transition";
    private AudioClip transition;
    private AudioSource source;
    private bool pressed;

    public Button Kitchen1;
    public Button Bedroom1;
    private GameObject levelSelection;

    private SceneGenerator sceneGenerator;

    void Start() {
        pressed = false;
        transition = (AudioClip)Resources.Load(TRANSITION);
        source = GameObject.Find("Sound").GetComponent<AudioSource>();

        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Kitchen 1"));
        levelSelection = GameObject.Find("LevelSelectionText");
        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();
    }

    void Update()
    {
        if (pressed)
        {
            Kitchen1.transform.Translate(Vector3.left * 800.0f * Time.deltaTime);
            Bedroom1.transform.Translate(Vector3.left * 800.0f * Time.deltaTime);
            levelSelection.transform.Translate(Vector3.left * 800.0f * Time.deltaTime);
        }
    }

    public void Kitchen1Press() {
        sceneGenerator.LevelName = "KitchenTrack1.xml";
        StartCoroutine(Transition());
    }

    public void Bedroom1Press() {
        sceneGenerator.LevelName = "BedroomTrack.xml";
        StartCoroutine(Transition());
    }

    public void UserInputEnter(InputField userInputField) {
        sceneGenerator.LevelName = userInputField.text;
        StartCoroutine(Transition());
    }

    private void GoToNextMenu() {
        SceneManager.LoadScene("SelectionMenu");
    }

    private IEnumerator Transition()
    {
        source.clip = transition;
        source.loop = false;
        pressed = true;
        source.Play();
        yield return new WaitWhile(() => source.isPlaying);
        GoToNextMenu();
    }
}
