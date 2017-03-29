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

    private const int NUMBEROFBUTTONS = 2;

    public Canvas pauseMenu;

    public Text resumeText;
    public Text quitText;

    private float defaultTimeScale;

    private Text[] buttons;
    private int currentButton;

    private bool axisEnabled;

    private string gameStateName;

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
        buttons[1] = quitText;

        currentButton = 0;
        resumeText.color = Color.gray;

        axisEnabled = true;
        //gameStateName = GameObject.Find("Player 1").GetComponent<Kart>().GameState.GetGameStateName();
    }
	
	void Update () {
        if (FadeOutBool)
        {
            FadeOut();
        }

        if (SimpleInput.GetButtonDown("Pause", 1) && !pauseMenu.enabled)
        {
            pauseMenu.enabled = true;
            Time.timeScale = 0;
            resumeText.color = Color.gray;
        }

        scrollMenu();

        buttonPress();

    }

    private void buttonPress()
    {
        if(pauseMenu.enabled && SimpleInput.GetButtonDown("Bump Kart"))
        {
            if(ReferenceEquals(buttons[currentButton], resumeText))
            {
                resumePress();
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

    private IEnumerator quitPress()
    {
        PlaySound();
        transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = defaultTimeScale;
        yield return new WaitWhile(() => source.isPlaying);
        //pauseMenu.enabled = false;
        SceneManager.LoadScene("MainMenu");
    }

    private void scrollMenu()
    {
        if (pauseMenu.enabled)
        {
            if((SimpleInput.GetAxis("Vertical", 1) > 0 && axisEnabled) || SimpleInput.GetButtonDown("Accelerate"))
            {
                axisEnabled = false;
                currentButton++;
                if(currentButton >= NUMBEROFBUTTONS)
                {
                    currentButton = 0;
                }
                colorSelectedButton();
            }
            else if((SimpleInput.GetAxis("Vertical", 1) < 0 && axisEnabled) || SimpleInput.GetButtonDown("Reverse"))
            {
                axisEnabled = false;
                currentButton--;
                if(currentButton < 0)
                {
                    currentButton = NUMBEROFBUTTONS - 1;
                }
                colorSelectedButton();
            }

            if(SimpleInput.GetAxis("Vertical", 1) == 0)
            {
                axisEnabled = true;
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
