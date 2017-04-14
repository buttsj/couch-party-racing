using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuFunctionality : MonoBehaviour
{
    private WhiteFadeUniversal fader;
    private GameObject acctManager;

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

    public Image raceModeInfo;
    public Image trackModeInfo;
    public Image spudModeInfo;
    public Image totModeInfo;
    public Image couchModeInfo;
    public Image storeModeInfo;
    private bool infoOn;

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
        raceModeInfo.gameObject.SetActive(false);
        trackModeInfo.gameObject.SetActive(false);
        spudModeInfo.gameObject.SetActive(false);
        totModeInfo.gameObject.SetActive(false);
        couchModeInfo.gameObject.SetActive(false);
        storeModeInfo.gameObject.SetActive(false);
        infoOn = false;
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

        //buttons[currentButton].color = Color.cyan;
        acctManager = GameObject.Find("AccountManager");
    }

    void Update()
    {
        buttons[currentButton].color = acctManager.GetComponent<AccountManager>().getCurrColor;
        if (SimpleInput.GetAxis("Vertical") == 0 && SimpleInput.GetAxis("Horizontal") == 0)
        {
            axisEnabled = true;
        }

        scrollMenu();
        buttonPress();
        fadeoutTimer += Time.deltaTime;
        if (fadeoutTimer >= 10.0f)
        {
            infoOn = false;
            raceModeInfo.gameObject.SetActive(false);
            trackModeInfo.gameObject.SetActive(false);
            spudModeInfo.gameObject.SetActive(false);
            totModeInfo.gameObject.SetActive(false);
            couchModeInfo.gameObject.SetActive(false);
            storeModeInfo.gameObject.SetActive(false);
            foreach (Text button in buttons)
            {
                button.CrossFadeAlpha(0f, 0.5f, true);
            }
            mainmenuText.CrossFadeAlpha(0f, 0.5f, true);
            fadeoutTimer = 11.0f; // prevent this float from increasing forever
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
            if (infoOn)
            {
                raceModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                raceModeInfo.gameObject.SetActive(false);
                trackModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                trackModeInfo.gameObject.SetActive(false);
                spudModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                spudModeInfo.gameObject.SetActive(false);
                totModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                totModeInfo.gameObject.SetActive(false);
                couchModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                couchModeInfo.gameObject.SetActive(false);
                storeModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                storeModeInfo.gameObject.SetActive(false);
                if (ReferenceEquals(buttons[currentButton], race))
                {
                    raceModeInfo.gameObject.SetActive(true);
                    raceModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
                }
                else if (ReferenceEquals(buttons[currentButton], trackBuilder))
                {
                    trackModeInfo.gameObject.SetActive(true);
                    trackModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
                }
                else if (ReferenceEquals(buttons[currentButton], totShot))
                {
                    totModeInfo.gameObject.SetActive(true);
                    totModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
                }
                else if (ReferenceEquals(buttons[currentButton], spudRun))
                {
                    spudModeInfo.gameObject.SetActive(true);
                    spudModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
                }
                else if (ReferenceEquals(buttons[currentButton], couchParty))
                {
                    couchModeInfo.gameObject.SetActive(true);
                    couchModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
                }
                else if (ReferenceEquals(buttons[currentButton], settings))
                {
                    // do nothing
                }
                else if (ReferenceEquals(buttons[currentButton], exit))
                {
                    // do nothing
                }
                else if (ReferenceEquals(buttons[currentButton], store))
                {
                    storeModeInfo.gameObject.SetActive(true);
                    storeModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
                }
            }
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
            if (infoOn)
            {
                raceModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                raceModeInfo.gameObject.SetActive(false);
                trackModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                trackModeInfo.gameObject.SetActive(false);
                spudModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                spudModeInfo.gameObject.SetActive(false);
                totModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                totModeInfo.gameObject.SetActive(false);
                couchModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                couchModeInfo.gameObject.SetActive(false);
                storeModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                storeModeInfo.gameObject.SetActive(false);
                if (ReferenceEquals(buttons[currentButton], race))
                {
                    raceModeInfo.gameObject.SetActive(true);
                    raceModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
                }
                else if (ReferenceEquals(buttons[currentButton], trackBuilder))
                {
                    trackModeInfo.gameObject.SetActive(true);
                    trackModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
                }
                else if (ReferenceEquals(buttons[currentButton], totShot))
                {
                    totModeInfo.gameObject.SetActive(true);
                    totModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
                }
                else if (ReferenceEquals(buttons[currentButton], spudRun))
                {
                    spudModeInfo.gameObject.SetActive(true);
                    spudModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
                }
                else if (ReferenceEquals(buttons[currentButton], couchParty))
                {
                    couchModeInfo.gameObject.SetActive(true);
                    couchModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
                }
                else if (ReferenceEquals(buttons[currentButton], settings))
                {
                    // do nothing
                }
                else if (ReferenceEquals(buttons[currentButton], exit))
                {
                    // do nothing
                }
                else if (ReferenceEquals(buttons[currentButton], store))
                {
                    storeModeInfo.gameObject.SetActive(true);
                    storeModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
                }
            }
        }
    }

    private void buttonPress()
    {
        if (SimpleInput.GetButtonDown("Use PowerUp"))
        {
            infoOn = !infoOn;
            if (ReferenceEquals(buttons[currentButton], race))
            {
                raceModeInfo.gameObject.SetActive(true);
                raceModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
            }
            else if (ReferenceEquals(buttons[currentButton], trackBuilder))
            {
                trackModeInfo.gameObject.SetActive(true);
                trackModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
            }
            else if (ReferenceEquals(buttons[currentButton], totShot))
            {
                totModeInfo.gameObject.SetActive(true);
                totModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
            }
            else if (ReferenceEquals(buttons[currentButton], spudRun))
            {
                spudModeInfo.gameObject.SetActive(true);
                spudModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
            }
            else if (ReferenceEquals(buttons[currentButton], couchParty))
            {
                couchModeInfo.gameObject.SetActive(true);
                couchModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
            }
            else if (ReferenceEquals(buttons[currentButton], settings))
            {
                // do nothing
            }
            else if (ReferenceEquals(buttons[currentButton], exit))
            {
                // do nothing
            }
            else if (ReferenceEquals(buttons[currentButton], store))
            {
                storeModeInfo.gameObject.SetActive(true);
                storeModeInfo.CrossFadeAlpha(1.0f, 0.5f, true);
            }
            if (!infoOn)
            {
                raceModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                raceModeInfo.gameObject.SetActive(false);
                trackModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                trackModeInfo.gameObject.SetActive(false);
                spudModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                spudModeInfo.gameObject.SetActive(false);
                totModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                totModeInfo.gameObject.SetActive(false);
                couchModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                couchModeInfo.gameObject.SetActive(false);
                storeModeInfo.CrossFadeAlpha(0.0f, 0.5f, true);
                storeModeInfo.gameObject.SetActive(false);
            }
        }
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
            else if (ReferenceEquals(buttons[currentButton], couchParty))
            {
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

    private IEnumerator couchPartyPress()
    {
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
        if (sceneGenerator.GamemodeName == "RaceMode")
        {
            SceneManager.LoadScene("LevelSelectionMenu");
        }
        else if (sceneGenerator.GamemodeName == "TrackBuilder")
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
        //buttons[currentButton].color = Color.cyan;
        buttons[currentButton].color = acctManager.GetComponent<AccountManager>().getCurrColor;
    }
}
