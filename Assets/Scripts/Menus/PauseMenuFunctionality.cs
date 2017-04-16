using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PauseMenuFunctionality : MonoBehaviour {

    public Image img;
    private float alpha;
    private float fadeSpeed = 1f;
    private bool FadeOutBool;

    private const string TRANSITION = "Sounds/KartEffects/screen_transition";
    private const string START_TRACK_NAME = "StartTrack";
    private AudioClip transition;
    private AudioSource source;
    private bool pressed;
    
    public Canvas pauseMenu;

    public Text resumeText;
    public Text controlsText;
    public Text quitText;
    public Text quitWithoutSavingText;

    public Image controlsImage;
    public GameObject trackParent;

    private float defaultTimeScale;

    private List<Text> buttons;
    private int currentButton;

    private bool axisEnabled;

    private string gameStateName;

    private bool controlView;

    private bool delayInput;

    private string sceneName;


    void Start () {

        img = gameObject.AddComponent<Image>();
        Color col = img.color;
        col.a = 0;
        img.color = col;
        alpha = 0f;
        FadeOutBool = false;

        pressed = false;
        transition = (AudioClip)Resources.Load(TRANSITION);
        source = GameObject.Find("Music Manager HUD").GetComponent<MusicManager>().source;

        pauseMenu = pauseMenu.GetComponent<Canvas>();

        pauseMenu.enabled = false;

        defaultTimeScale = Time.timeScale;

        buttons = new List<Text>();
        buttons.Add(resumeText);
        buttons.Add(controlsText);
        buttons.Add(quitText);

        currentButton = 0;
        resumeText.color = Color.cyan;

        axisEnabled = true;
        controlView = false;
        controlsImage.enabled = false;

        applySceneSpecifics();
    }
	
	void Update () {
        if (FadeOutBool)
        {
            FadeOut();
        }

        delayInputOnUnPause();

        if (SimpleInput.GetButtonDown("Pause", 1) && !pauseMenu.enabled)
        {
            pauseMenu.enabled = true;
            Time.timeScale = 0f;
            resumeText.color = Color.cyan;
        }

        if (!controlView)
        {
            scrollMenu();
            buttonPress();
        }
        else
        {
            if (SimpleInput.GetAnyButtonDown())
            {
                controlView = false;
                controlsImage.enabled = false;
            }
        }

    }

    private void buttonPress()
    {
        if(pauseMenu.enabled && SimpleInput.GetButtonDown("Bump Kart"))
        {
            if(ReferenceEquals(buttons[currentButton], resumeText))
            {
                resumePress();
            }
            else if (ReferenceEquals(buttons[currentButton], controlsText))
            {
                controlsPress();
            }
            else if(ReferenceEquals(buttons[currentButton], quitText))
            {
                if(sceneName == "TrackBuilderScene" && DoesStartTrackExist())
                {
                    FadeOutBool = true;
                    saveCustomTrack();
                    StartCoroutine(quitPress());
                } else if(sceneName != "TrackBuilderScene") {
                    FadeOutBool = true;
                    StartCoroutine(quitPress());
                }

            } else if(ReferenceEquals(buttons[currentButton], quitWithoutSavingText)) {
                FadeOutBool = true;
                StartCoroutine(quitPress());
            }
        }
    }

    private void resumePress()
    {
        source.PlayOneShot(transition);
        delayInput = true;
        
        Time.timeScale = defaultTimeScale;
    }

    private void controlsPress()
    {
        controlView = true;
        controlsImage.enabled = true;
    }

    private IEnumerator quitPress()
    {
        PlaySound();
        transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = defaultTimeScale;
        yield return new WaitWhile(() => source.isPlaying);
        SceneManager.LoadScene("MainMenu");
    }

    private void scrollMenu()
    {
        if (pauseMenu.enabled)
        {
            if (SimpleInput.GetAxis("Vertical", 1) == 0 && SimpleInput.GetAxis("Horizontal", 1) == 0)
            {
                axisEnabled = true;
            }

            if ((SimpleInput.GetAxis("Vertical", 1) < 0 && axisEnabled) || SimpleInput.GetButtonDown("Reverse"))
            {
                axisEnabled = false;
                currentButton++;
                if (currentButton >= buttons.Count)
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
                    currentButton = buttons.Count - 1;
                }
                colorSelectedButton();
            }

        }
    }

    private void colorSelectedButton()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].color = Color.white;
        }
        buttons[currentButton].color = Color.cyan;
    }

    private void delayInputOnUnPause()
    {
        if (delayInput)
        {
            delayInput = false;
            pauseMenu.enabled = false;
        }
    }

    private void PlaySound()
    {
        source.clip = transition;
        source.loop = false;
        source.Play();
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

    public bool isPaused()
    {
        return pauseMenu.enabled;
    }

    private void applySceneSpecifics()
    {
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "TrackBuilderScene")
        {
            quitText.text = "Save and Exit";
            buttons.Add(quitWithoutSavingText);
        } else {
            quitWithoutSavingText.enabled = false;
        }
    }

    private void saveCustomTrack()
    {
        trackParent.GetComponent<TrackSaver>().Save(trackParent.name);
    }


    public bool DoesStartTrackExist() {
        bool doesStartTrackExist = false;

        foreach (Transform child in trackParent.transform) {
            doesStartTrackExist = child.name == START_TRACK_NAME;
            if (doesStartTrackExist) {
                break;
            }
        }

        return doesStartTrackExist;
    }

}
