using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            pauseMenu.enabled = true;
            Time.timeScale = 0;
        }

	}

    public void resumePress()
    {
        pauseMenu.enabled = false;
        Time.timeScale = defaultTimeScale;
    }

    public void quitPress()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
