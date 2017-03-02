using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuFunctionality : MonoBehaviour
{
    private WhiteFadeUniversal fader;

    public GameObject waypointAI;

    private const int BUTTONSWIDTH = 2;
    private const int BUTTONSHEIGHT = 4;

    public Canvas quitMenu;

    public Text raceMode;
    public Text trackBuilder;
    public Text deathRun;
    public Text playground;
    public Text spudRun;
    public Text settings;
    public Text exit;

    public Text yesExit;
    public Text noExit;

    private SceneGenerator sceneGenerator;

    private Text[,] buttons;
    private int currentButtonX;
    private int currentButtonY;

    private bool axisEnabled;

    private Color highlight;

    private Text[] quitButtons;
    private int exitIndex;

    void Awake()
    {
        // fade in/out initializer
        GameObject fadeObject = new GameObject();
        fadeObject.name = "Fader";
        fadeObject.transform.SetParent(transform);
        fadeObject.SetActive(true);
        fader = fadeObject.AddComponent<WhiteFadeUniversal>();
        fader.BeginNewScene("Sound");
        //
    }

    void Start()
    {
        highlight = new Color(255, 255, 0);

        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();

        quitMenu = quitMenu.GetComponent<Canvas>();
        quitMenu.enabled = false;

        buttons = new Text[BUTTONSHEIGHT, BUTTONSWIDTH];
        buttons[0, 0] = raceMode;
        buttons[0, 1] = trackBuilder;
        buttons[1, 0] = deathRun;
        buttons[1, 1] = playground;
        buttons[2, 0] = spudRun;
        buttons[2, 1] = settings;
        buttons[3, 0] = exit;
        buttons[3, 1] = exit;

        currentButtonX = 0;
        currentButtonY = 0;
        raceMode.color = highlight;

        quitButtons = new Text[2];
        quitButtons[0] = noExit;
        quitButtons[1] = yesExit;
        exitIndex = 0;
        noExit.color = highlight;

        axisEnabled = true;
    }

    void Update()
    {
        if (SimpleInput.GetAxis("Vertical") == 0 && SimpleInput.GetAxis("Horizontal") == 0)
        {
            axisEnabled = true;
        }

        if (!quitMenu.enabled)
        {
            scrollMenu();
            buttonPress();
        }
        else
        {
            quitScroll();
            quitButtonPress();
        }

    }

    private void scrollMenu()
    {
        if ((SimpleInput.GetAxis("Vertical") < 0 && axisEnabled) || SimpleInput.GetButtonDown("Reverse"))
        {
            axisEnabled = false;
            currentButtonX++;
            if (currentButtonX >= BUTTONSHEIGHT)
            {
                currentButtonX = 0;
            }
            colorSelectedButton();
        }
        else if ((SimpleInput.GetAxis("Vertical") > 0 && axisEnabled) || SimpleInput.GetButtonDown("Accelerate"))
        {
            axisEnabled = false;
            currentButtonX--;
            if (currentButtonX < 0)
            {
                currentButtonX = BUTTONSHEIGHT - 1;
            }
            colorSelectedButton();
        }
        else if ((SimpleInput.GetAxis("Horizontal") < 0 && axisEnabled))
        {
            axisEnabled = false;
            currentButtonY++;
            if (currentButtonY >= BUTTONSWIDTH)
            {
                currentButtonY = 0;
            }
            colorSelectedButton();
        }
        else if ((SimpleInput.GetAxis("Horizontal") > 0 && axisEnabled))
        {
            axisEnabled = false;
            currentButtonY--;
            if (currentButtonY < 0)
            {
                currentButtonY = BUTTONSWIDTH - 1;
            }
            colorSelectedButton();
        }
    }

    private void buttonPress()
    {
        if (SimpleInput.GetButtonDown("Bump Kart"))
        {
            if (ReferenceEquals(buttons[currentButtonX, currentButtonY], raceMode))
            {
                StartCoroutine(raceModePress());
            }
            else if (ReferenceEquals(buttons[currentButtonX, currentButtonY], trackBuilder))
            {
                StartCoroutine(trackBuilderPress());
            }
            else if (ReferenceEquals(buttons[currentButtonX, currentButtonY], deathRun))
            {
                StartCoroutine(deathRunPress());
            }
            else if (ReferenceEquals(buttons[currentButtonX, currentButtonY], playground))
            {
                StartCoroutine(playgroundPress());
            }
            else if (ReferenceEquals(buttons[currentButtonX, currentButtonY], spudRun))
            {
                StartCoroutine(spudRunPress());
            }
            else if (ReferenceEquals(buttons[currentButtonX, currentButtonY], settings))
            {
                settingsPress();
            }
            else if (ReferenceEquals(buttons[currentButtonX, currentButtonY], exit))
            {
                exitPress();
            }
            else if (ReferenceEquals(buttons[currentButtonX, currentButtonY], exit))
            {
                exitPress();
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

    private IEnumerator deathRunPress()
    {
        while (!fader.Faded)
            yield return null;
        //GoToNextMenu();
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

    }

    private void exitPress()
    {
        quitMenu.enabled = true;
    }

    private void quitScroll()
    {
        if (SimpleInput.GetAxis("Horizontal") != 0 && axisEnabled)
        {
            axisEnabled = false;
            exitIndex++;
            exitIndex %= 2;
            noExit.color = Color.white;
            yesExit.color = Color.white;
            quitButtons[exitIndex].color = highlight;
        }
    }

    private void quitButtonPress()
    {
        if (SimpleInput.GetButtonDown("Bump Kart"))
        {
            if (ReferenceEquals(quitButtons[exitIndex], noExit))
            {
                noQuitPress();
            }
            else if (ReferenceEquals(quitButtons[exitIndex], yesExit))
            {
                yesQuitPress();
            }
        }
    }

    private void noQuitPress()
    {
        quitMenu.enabled = false;
    }

    private void yesQuitPress()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void GoToNextMenu()
    {
        SceneManager.LoadScene("LevelSelectionMenu");
    }

    private void colorSelectedButton()
    {
        for (int i = 0; i < BUTTONSHEIGHT; i++)
        {
            for (int j = 0; j < BUTTONSWIDTH; j++)
            {
                buttons[i, j].color = Color.white;
            }
        }

        buttons[currentButtonX, currentButtonY].color = highlight;
    }
}
