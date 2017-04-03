using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenuFunctionality : MonoBehaviour {

    public Image img;
    private float alpha;
    private float fadeSpeed = 1f;
    private bool FadeOutBool;

    private const string TRANSITION = "Sounds/KartEffects/screen_transition";
    private AudioClip transition;
    private AudioSource source;
    private bool pressed;

    private const int NUMBEROFBUTTONS = 3;

    public Canvas pauseMenu;

    public Text resumeText;
    public Text controlsText;
    public Text quitText;

    public Image controlsImage;

    private float defaultTimeScale;

    private Text[] buttons;
    private int currentButton;

    private bool axisEnabled;

    private string gameStateName;

    private bool controlView;

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

        buttons = new Text[NUMBEROFBUTTONS];
        buttons[0] = resumeText;
        buttons[1] = controlsText;
        buttons[2] = quitText;

        currentButton = 0;
        resumeText.color = Color.gray;

        axisEnabled = true;
        controlView = false;
        controlsImage.enabled = false;
    }
	
	void Update () {
        if (FadeOutBool)
        {
            FadeOut();
        }

        if (SimpleInput.GetButtonDown("Pause", 1) && !pauseMenu.enabled)
        {
            pauseMenu.enabled = true;
            Time.timeScale = 0f;
            resumeText.color = Color.gray;
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
                FadeOutBool = true;
                StartCoroutine(quitPress());
            }
                
        }
    }

    private void resumePress()
    {
        source.PlayOneShot(transition);
        pauseMenu.enabled = false;
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
    }

    private void colorSelectedButton()
    {
        for(int i = 0; i < NUMBEROFBUTTONS; i++)
        {
            buttons[i].color = Color.white;
        }
        buttons[currentButton].color = Color.gray;
    }

    private void PlaySound()
    {
        source.clip = transition;
        source.loop = false;
        source.Play();
    }

    private void FadeOut()
    {
        Debug.Log(alpha);
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
