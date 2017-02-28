using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpudRunMenu : MonoBehaviour {

    private const string TRANSITION = "Sounds/KartEffects/screen_transition";
    private AudioClip transition;
    private AudioSource source;
    private bool pressed;

    public Button Arena1;
    private GameObject levelSelection;

    private SceneGenerator sceneGenerator;

    void Start() {
        pressed = false;
        transition = (AudioClip)Resources.Load(TRANSITION);
        source = GameObject.Find("Sound").GetComponent<AudioSource>();

        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Arena 1"));
        levelSelection = GameObject.Find("LevelSelectionText");
        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();
    }

    void Update()
    {
        if (pressed)
        {
            Arena1.transform.Translate(Vector3.left * 800.0f * Time.deltaTime);
            levelSelection.transform.Translate(Vector3.left * 800.0f * Time.deltaTime);
        }
    }

    public void Arena1Press() {
        //sceneGenerator.LevelName = "Arena1.xml";
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
