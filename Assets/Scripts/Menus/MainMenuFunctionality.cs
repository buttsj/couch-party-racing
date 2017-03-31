using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuFunctionality : MonoBehaviour
{

    private SettingsMenuFunctionality settingsFunc;

    private WhiteFadeUniversal fader;

    private const int NUMBEROFBUTTONS = 7;

    public Canvas settingsMenu;
    public Text setApply;
    public Text setCancel;
    private Text[] settingsButtons;
    private int settingsIndex;

    public Text race;
    public Text trackBuilder;
    public Text spudRun;
    public Text totShot;
    public Text store;
    public Text settings;
    public Text exit;

    private SceneGenerator sceneGenerator;

    private Text[] buttons;
    private int currentButton;

    private bool axisEnabled;

    void Awake()
    {
        GameObject fadeObject = new GameObject();
        fadeObject.name = "Fader";
        fadeObject.transform.SetParent(transform);
        fadeObject.SetActive(true);
        fader = fadeObject.AddComponent<WhiteFadeUniversal>();
        fader.BeginNewScene("Sound");

        WaypointSetter.SetWaypoints();
        GameObject.Find("AIKart").GetComponent<WaypointAI>().GameState = new RacingGameState(GameObject.Find("AIKart"));
    }

    void Start()
    {
        settingsFunc = settingsMenu.GetComponent<SettingsMenuFunctionality>();

        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();

        settingsMenu = settingsMenu.GetComponent<Canvas>();
        settingsMenu.enabled = false;

        buttons = new Text[NUMBEROFBUTTONS];
        buttons[0] = race;
        buttons[1] = trackBuilder;
        buttons[2] = spudRun;
        buttons[3] = totShot;
        buttons[4] = store;
        buttons[5] = settings;
        buttons[6] = exit;

        currentButton = 0;

        settingsButtons = new Text[2];
        settingsButtons[0] = setApply;
        settingsButtons[1] = setCancel;
        settingsIndex = 0;

        axisEnabled = true;

        buttons[currentButton].color = Color.gray;
    }

    void Update()
    {
        if (SimpleInput.GetAxis("Vertical") == 0 && SimpleInput.GetAxis("Horizontal") == 0)
        {
            axisEnabled = true;
        }

        if (!settingsMenu.enabled)
        {
            scrollMenu();
            buttonPress();
        }
        else if (settingsMenu.enabled)
        {
            settingsScroll();
            settingsButtonPress();
        }

    }

    private void scrollMenu()
    {
        if ((SimpleInput.GetAxis("Vertical", 1) < 0 && axisEnabled) || SimpleInput.GetButtonDown("Reverse"))
        {
            axisEnabled = false;
            currentButton++;
            if (currentButton >= NUMBEROFBUTTONS)
            {
                currentButton = 0;
            }
            colorSelectedButton();
        }
        else if ((SimpleInput.GetAxis("Vertical", 1) > 0 && axisEnabled) || SimpleInput.GetButtonDown("Accelerate"))
        {
            axisEnabled = false;
            currentButton--;
            if (currentButton < 0)
            {
                currentButton = NUMBEROFBUTTONS - 1;
            }
            colorSelectedButton();
        }
    }

    private void buttonPress()
    {
        if (SimpleInput.GetButtonDown("Bump Kart"))
        {
            if (ReferenceEquals(buttons[currentButton], race))
            {
                StartCoroutine(raceModePress());
            }
            else if (ReferenceEquals(buttons[currentButton], trackBuilder))
            {
                StartCoroutine(trackBuilderPress());
            }
            else if (ReferenceEquals(buttons[currentButton], totShot))
            {
                StartCoroutine(playgroundPress());
            }
            else if (ReferenceEquals(buttons[currentButton], spudRun))
            {
                StartCoroutine(spudRunPress());
            }
            else if (ReferenceEquals(buttons[currentButton], settings))
            {
                settingsPress();
            }
            else if (ReferenceEquals(buttons[currentButton], exit))
            {
                exitPress();
            }
            else if (ReferenceEquals(buttons[currentButton], store))
            {
                StartCoroutine(storePress());
            }
        }
    }

    private IEnumerator raceModePress()
    {
        sceneGenerator.GamemodeName = "RaceMode";
        sceneGenerator.SceneName = "HomeScene";
        sceneGenerator.LevelName = null;
        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;
        GoToNextMenu();
    }

    private IEnumerator trackBuilderPress()
    {
        sceneGenerator.GamemodeName = "TrackBuilder";
        sceneGenerator.SceneName = "TrackBuilderScene";
        sceneGenerator.LevelName = null;
        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;
        GoToNextMenu();
    }

    private IEnumerator playgroundPress()
    {
        sceneGenerator.GamemodeName = "TotShot";
        sceneGenerator.SceneName = "TotShotScene";
        sceneGenerator.LevelName = null;
        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;
        GoToNextMenu();
    }

    private IEnumerator spudRunPress()
    {
        sceneGenerator.GamemodeName = "SpudRun";
        sceneGenerator.SceneName = "SpudRunScene";
        sceneGenerator.LevelName = null;
        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;
        GoToNextMenu();
    }

    private void settingsPress()
    {
        settingsMenu.enabled = true;
    }

    private void exitPress()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private IEnumerator storePress()
    {
        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;
        GoToShop();
    }

    private void settingsScroll()
    {
        if (SimpleInput.GetAxis("Horizontal") != 0 && axisEnabled)
        {
            axisEnabled = false;
            settingsIndex++;
            settingsIndex %= 2;
            setApply.color = Color.gray;
            setCancel.color = Color.gray;
            settingsButtons[settingsIndex].color = Color.white;
        }
    }

    private void settingsButtonPress()
    {
        if (SimpleInput.GetButtonDown("Bump Kart"))
        {
            if (ReferenceEquals(settingsButtons[settingsIndex], setCancel))
            {
                cancelSettings();
            }
            else if (ReferenceEquals(settingsButtons[settingsIndex], setApply))
            {
                applySettings();
            }
        }
    }

    private void cancelSettings()
    {
        settingsMenu.enabled = false;
    }

    private void applySettings()
    {
        settingsFunc.ApplySettings();
        settingsMenu.enabled = false;
    }

    private void GoToNextMenu()
    {
        if(sceneGenerator.GamemodeName == "RaceMode")
        {
            SceneManager.LoadScene("LevelSelectionMenu");
        }
        else if(sceneGenerator.GamemodeName == "TrackBuilder")
        {
            SimpleInput.ClearCurrentPlayerDevices();
            SimpleInput.MapPlayerToDevice(1);

            Destroy(sceneGenerator.gameObject);

            SceneManager.LoadScene("TrackBuilderScene");
        }
        else
        {
            SceneManager.LoadScene("SelectionMenu");
        }

    }

    private void GoToShop()
    {
        SceneManager.LoadScene("ChipShopScene");
    }

    private void colorSelectedButton()
    {
        for (int i = 0; i < NUMBEROFBUTTONS; i++)
        {
            buttons[i].color = Color.white;
        }
        buttons[currentButton].color = Color.gray;
    }
}
