using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuFunctionality : MonoBehaviour
{
    private WhiteFadeUniversal fader;

    private const int NUMBEROFBUTTONS = 8;

    public Text race;
    public Text trackBuilder;
    public Text spudRun;
    public Text totShot;
    public Text couchParty;
    public Text store;
    public Text settings;
    public Text exit;
    public Text mainmenuText;

    private float fadeoutTimer;

    private SceneGenerator sceneGenerator;

    private Text[] buttons;
    private int currentButton;

    private bool axisEnabled;

    public GameObject settingsMenu;

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

        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();

        settingsMenu.SetActive(false);
        CouchPartyManager.IsCouchPartyMode = false;
        CouchPartyManager.ResetScores();
        buttons = new Text[NUMBEROFBUTTONS];
        buttons[0] = race;
        buttons[1] = trackBuilder;
        buttons[2] = spudRun;
        buttons[3] = totShot;
        buttons[4] = couchParty;
        buttons[5] = store;
        buttons[6] = settings;
        buttons[7] = exit;

        currentButton = 0;

        axisEnabled = true;

        buttons[currentButton].color = Color.cyan;
    }

    void Update()
    {
        if (SimpleInput.GetAxis("Vertical") == 0 && SimpleInput.GetAxis("Horizontal") == 0)
        {
            axisEnabled = true;
        }

            scrollMenu();
            buttonPress();
        fadeoutTimer += Time.deltaTime;
        if (fadeoutTimer >= 10.0f)
        {
            foreach (Text button in buttons)
            {
                button.CrossFadeAlpha(0f, 0.5f, true);
    }
            mainmenuText.CrossFadeAlpha(0f, 0.5f, true);
        }
    }

    private void scrollMenu()
    {
        if ((SimpleInput.GetAxis("Vertical", 1) < 0 && axisEnabled) || SimpleInput.GetButtonDown("Reverse"))
        {
            fadeoutTimer = 0.0f;
            foreach (Text button in buttons)
            {
                button.CrossFadeAlpha(1f, 0.5f, true);
            }
            mainmenuText.CrossFadeAlpha(1f, 0.5f, true);
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
            fadeoutTimer = 0.0f;
            foreach (Text button in buttons)
            {
                button.CrossFadeAlpha(1f, 0.5f, true);
            }
            mainmenuText.CrossFadeAlpha(1f, 0.5f, true);
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
            else if (ReferenceEquals(buttons[currentButton], couchParty)) {
                StartCoroutine(couchPartyPress());
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

    private IEnumerator couchPartyPress() {
        sceneGenerator.GamemodeName = "RaceMode";
        sceneGenerator.SceneName = "HomeScene";
        sceneGenerator.LevelName = null;
        CouchPartyManager.IsCouchPartyMode = true;
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
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
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

            SceneManager.LoadScene("BuilderSelect");
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
        buttons[currentButton].color = Color.cyan;
    }
}
