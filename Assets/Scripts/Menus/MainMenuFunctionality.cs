using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuFunctionality : MonoBehaviour {

    public Canvas quitMenu;

    public Button raceMode;
    public Button trackBuilderMode;
    public Button asymmetricMode;
    public Button playgroundMode;
    public Button hotPotatoMode;
    public Button settings;
    public Button exit;

    void Start () {
        quitMenu = quitMenu.GetComponent<Canvas>();
        quitMenu.enabled = false;

        raceMode = raceMode.GetComponent<Button>();
        trackBuilderMode = trackBuilderMode.GetComponent<Button>();
        asymmetricMode = asymmetricMode.GetComponent<Button>();
        playgroundMode = playgroundMode.GetComponent<Button>();
        hotPotatoMode = hotPotatoMode.GetComponent<Button>();
        settings = settings.GetComponent<Button>();
        exit = exit.GetComponent<Button>();

        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("RaceMode"));
    }

    public void settingsPress()
    {
        // Add Settings
    }

    public void exitPress()
    {
        quitMenu.enabled = true;

        raceMode.enabled = false;
        trackBuilderMode.enabled = false;
        asymmetricMode.enabled = false;
        playgroundMode.enabled = false;
        hotPotatoMode.enabled = false;
        settings.enabled = false;
        exit.enabled = false;

        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("No"));
    }

    public void noQuitPress()
    {
        quitMenu.enabled = false;

        raceMode.enabled = true;
        trackBuilderMode.enabled = true;
        asymmetricMode.enabled = true;
        playgroundMode.enabled = true;
        hotPotatoMode.enabled = true;
        settings.enabled = true;
        exit.enabled = true;

        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Exit"));
    }

    public void yesQuitPress()
    {
        Application.Quit();
    }

    public void raceModePress()
    {
        SceneManager.LoadScene("SelectionMenu");
    }

    public void trackBuilderModePress()
    {
        //SceneManager.LoadScene("BuilderSelection");
    }

    public void asymmetricModePress()
    {
        //SceneManager.LoadScene("AsymmetricSelection");
    }

    public void playgroundModePress()
    {
        //SceneManager.LoadScene("PlaygroundSelection");
    }

    public void hotPotatoModePress()
    {
        SceneManager.LoadScene("SelectionMenuSpud");
    }

}
