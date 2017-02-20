using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuFunctionality : MonoBehaviour {

    private const int NUMBEROFBUTTONS = 2;

    public Canvas pauseMenu;

    public Text resumeText;
    public Text quitText;

    private float defaultTimeScale;

    private Text[] buttons;
    private int currentButton;

    private bool axisEnabled;

    private Color highlight;

    void Start () {
        highlight = new Color(255, 255, 0);
        pauseMenu = pauseMenu.GetComponent<Canvas>();

        pauseMenu.enabled = false;

        defaultTimeScale = Time.timeScale;

        buttons = new Text[NUMBEROFBUTTONS];
        buttons[0] = resumeText;
        buttons[1] = quitText;

        currentButton = 0;
        resumeText.color = highlight;

        axisEnabled = true;
    }
	
	void Update () {

        if (!GameObject.Find("RacingEndMenu(Clone)").GetComponent<RacingEndGameMenu>().RaceOver)
        {
            if (SimpleInput.GetButtonDown("Pause", 1) && !pauseMenu.enabled)
            {
                pauseMenu.enabled = true;
                Time.timeScale = 0;
                resumeText.color = highlight;
            }
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
                quitPress();
            }
                
        }
    }

    private void resumePress()
    {
        pauseMenu.enabled = false;
        Time.timeScale = defaultTimeScale;
    }

    private void quitPress()
    {
        pauseMenu.enabled = false;
        Time.timeScale = defaultTimeScale;
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
        buttons[currentButton].color = highlight;
    }

}
