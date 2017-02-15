using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuFunctionality : MonoBehaviour {

    public Canvas pauseMenu;

    public Button resume;
    public Button quit;

    private float defaultTimeScale;

    void Start () {

        pauseMenu = pauseMenu.GetComponent<Canvas>();

        resume = resume.GetComponent<Button>();
        quit = quit.GetComponent<Button>();

        pauseMenu.enabled = false;

        defaultTimeScale = Time.timeScale;

    }
	
	void Update () {
        if (!GameObject.Find("RacingGameManager").GetComponent<RacingGameManager>().RaceOver)
        {
            if (SimpleInput.GetButtonDown("Pause", 1) && !pauseMenu.enabled)
            {
                pauseMenu.enabled = true;
                Time.timeScale = 0;
                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("ResumeText"));
            }
        }
	}

    public void resumePress()
    {
        pauseMenu.enabled = false;
        Time.timeScale = defaultTimeScale;
    }

    public void quitPress()
    {
        pauseMenu.enabled = false;
        Time.timeScale = defaultTimeScale;
        SceneManager.LoadScene("MainMenu");
    }

}
