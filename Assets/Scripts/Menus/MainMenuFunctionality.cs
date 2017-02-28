using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuFunctionality : MonoBehaviour
{

    public GameObject waypointAI;

    public Image img;
    private float alpha;
    private float fadeSpeed = .5f;
    private bool FadeInBool;
    private bool FadeOutBool;

    private const string TRANSITION = "Sounds/KartEffects/screen_transition";
    private AudioClip transition;
    private AudioSource source;
    private bool pressed;

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

    void Start()
    {

        Color col = img.color;
        col.a = 255;
        img.color = col;
        alpha = 1.0f;
        FadeInBool = true;
        FadeOutBool = false;

        pressed = false;
        transition = (AudioClip)Resources.Load(TRANSITION);
        source = GameObject.Find("Sound").GetComponent<AudioSource>();

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
        if (FadeInBool)
        {
            FadeIn();
        }
        if (FadeOutBool)
        {
            FadeOut();
        }
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

    private void SetFadeOut()
    {
        pressed = true;
        FadeOutBool = true;
        FadeInBool = false;
        fadeSpeed = .9f;
    }

    private void buttonPress()
    {
        if (SimpleInput.GetButtonDown("Bump Kart") && !pressed)
        {
            if (ReferenceEquals(buttons[currentButtonX, currentButtonY], raceMode))
            {
                SetFadeOut();
                StartCoroutine(raceModePress());
            }
            else if (ReferenceEquals(buttons[currentButtonX, currentButtonY], trackBuilder))
            {
                SetFadeOut();
                StartCoroutine(trackBuilderPress());
            }
            else if (ReferenceEquals(buttons[currentButtonX, currentButtonY], deathRun))
            {
                SetFadeOut();
                deathRunPress();
            }
            else if (ReferenceEquals(buttons[currentButtonX, currentButtonY], playground))
            {
                SetFadeOut();
                playgroundPress();
            }
            else if (ReferenceEquals(buttons[currentButtonX, currentButtonY], spudRun))
            {
                SetFadeOut();
                StartCoroutine(spudRunPress());
            }
            else if (ReferenceEquals(buttons[currentButtonX, currentButtonY], settings))
            {
                SetFadeOut();
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
        PlaySound();
        yield return new WaitWhile(() => source.isPlaying);
        sceneGenerator.GamemodeName = "RaceMode";
        sceneGenerator.SceneName = "HomeScene";
        sceneGenerator.LevelName = null;
        GoToNextMenu();
    }

    private IEnumerator trackBuilderPress()
    {
        PlaySound();
        yield return new WaitWhile(() => source.isPlaying);
        sceneGenerator.GamemodeName = "TrackBuilder";
        sceneGenerator.SceneName = "TrackBuilderScene";
        sceneGenerator.LevelName = null;
        GoToNextMenu();
    }

    private void deathRunPress()
    {

    }

    private IEnumerator playgroundPress()
    {
        PlaySound();
        yield return new WaitWhile(() => source.isPlaying);
        sceneGenerator.GamemodeName = "TotShot";
        sceneGenerator.SceneName = "TotShotScene";
        sceneGenerator.LevelName = null;
        GoToNextMenu();
    }

    private IEnumerator spudRunPress()
    {
        PlaySound();
        yield return new WaitWhile(() => source.isPlaying);
        sceneGenerator.GamemodeName = "SpudRun";
        sceneGenerator.SceneName = "SpudRunScene";
        sceneGenerator.LevelName = null;
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

    private void PlaySound()
    {
        source.clip = transition;
        source.loop = false;
        source.Play();
    }

    private void FadeIn()
    {
        alpha -= fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        Color col = img.color;
        col.a = alpha;
        img.color = col;
        if (alpha == 0)
        {
            FadeInBool = false;
        }
    }

    private void FadeOut()
    {
        alpha += fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        Color col = img.color;
        col.a = alpha;
        img.color = col;
        if (alpha == 1)
        {
            FadeOutBool = false;
        }
    }

}
