using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuLogic : MonoBehaviour {

    public GameObject mainMenu;

    private const int NUMBER_OF_SELECTION_OPTIONS = 8;

    public Text volumePercentageText;

    public Text fullScreenToggleText;
    public Text vSyncToggleText;
    public Text p1ToggleText;
    public Text p2ToggleText;
    public Text p3ToggleText;
    public Text p4ToggleText;
    public Text saveText;
    public Text discardText;

    private Text[] buttons;

    private int currentButton;

    private bool axisEnabled;

    private float volumePercentage;

    private bool[] toggles;

    public Text volumeLeft;
    public Text volumeRight;
    public Text fullScreenLeft;
    public Text fullScreenRight;
    public Text vSyncLeft;
    public Text vSyncRight;
    public Text p1Left;
    public Text p1Right;
    public Text p2Left;
    public Text p2Right;
    public Text p3Left;
    public Text p3Right;
    public Text p4Left;
    public Text p4Right;

    void Start () {

        buttons = new Text[NUMBER_OF_SELECTION_OPTIONS];

        buttons[0] = volumePercentageText;
        buttons[1] = fullScreenToggleText;
        buttons[2] = vSyncToggleText;
        buttons[3] = p1ToggleText;
        buttons[4] = p2ToggleText;
        buttons[5] = p3ToggleText;
        buttons[6] = p4ToggleText;
        buttons[7] = saveText;

        currentButton = 0;

        axisEnabled = true;

        //Temporary preferences
        volumePercentage = 1.0f;

        toggles = new bool[6];
        toggles[0] = true; //fullScreenEnabled
        toggles[1] = false; //vSyncEnabled
        toggles[2] = false; //p1PadEnabled
        toggles[3] = true; //p2PadEnabled
        toggles[4] = true; //p3PadEnabled
        toggles[5] = true; //p4PadEnabled

        //End temp

        updateUIToPreferences();

        buttons[currentButton].color = Color.gray;
    }
	
	void Update () {

            resetAxis();
            scrollMenu();
            updateUIToPreferences();
            buttonPress();

    }

    private void scrollMenu()
    {
        if(SimpleInput.GetAxis("Horizontal", 1) < 0 && axisEnabled && currentButton == 0)
        {
            volumeLeft.color = Color.gray;
            volumePercentage -= 0.01f;
            if(volumePercentage < 0.0f)
            {
                volumePercentage = 0.0f;
            }

        }
        else if (SimpleInput.GetAxis("Horizontal", 1) > 0 && axisEnabled && currentButton == 0)
        {
            volumeRight.color = Color.gray;
            volumePercentage += 0.01f;
            if (volumePercentage > 1.0f)
            {
                volumePercentage = 1.0f;
            }

        }
        else if (SimpleInput.GetAxis("Horizontal", 1) < 0 && axisEnabled && currentButton > 0 && currentButton < NUMBER_OF_SELECTION_OPTIONS - 1)
        {
            axisEnabled = false;
            toggles[currentButton - 1] = !toggles[currentButton - 1];
            buttons[currentButton].transform.GetChild(0).GetComponent<Text>().color = Color.gray;
        }
        else if (SimpleInput.GetAxis("Horizontal", 1) > 0 && axisEnabled && currentButton > 0 && currentButton < NUMBER_OF_SELECTION_OPTIONS - 1)
        {
            axisEnabled = false;
            toggles[currentButton - 1] = !toggles[currentButton - 1];
            buttons[currentButton].transform.GetChild(1).GetComponent<Text>().color = Color.gray;
        }
        else if(SimpleInput.GetAxis("Horizontal", 1) != 0 && axisEnabled && currentButton == NUMBER_OF_SELECTION_OPTIONS - 1)
        {
            axisEnabled = false;
            buttons[currentButton].color = Color.white;
            if (buttons[currentButton].text == "Save")
            {
                buttons[currentButton] = discardText;
            }
            else
            {
                buttons[currentButton] = saveText;
            }
            colorSelectedButton();
        }
        else if ((SimpleInput.GetAxis("Vertical", 1) < 0 && axisEnabled) || SimpleInput.GetButtonDown("Reverse"))
        {
            axisEnabled = false;
            currentButton++;
            if (currentButton >= NUMBER_OF_SELECTION_OPTIONS)
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
                currentButton = NUMBER_OF_SELECTION_OPTIONS - 1;
            }
            colorSelectedButton();
        }
    }

    private void buttonPress()
    {
        if (SimpleInput.GetButtonDown("Bump Kart") && currentButton == NUMBER_OF_SELECTION_OPTIONS - 1)
        {
            if (buttons[currentButton].text.ToLower() == "save")
            {
                applyChanges();
            }
            else
            {
                exitSettings();
            }
        }
    }

    private void colorSelectedButton()
    {
        for (int i = 0; i < NUMBER_OF_SELECTION_OPTIONS; i++)
        {
            buttons[i].color = Color.white;
        }
        buttons[currentButton].color = Color.gray;
    }

    private void resetAxis()
    {
        if (SimpleInput.GetAxis("Vertical") == 0 && SimpleInput.GetAxis("Horizontal") == 0)
        {
            axisEnabled = true;

            volumeLeft.color = Color.white;
            volumeRight.color = Color.white;
            fullScreenLeft.color = Color.white;
            fullScreenRight.color = Color.white;
            vSyncLeft.color = Color.white;
            vSyncRight.color = Color.white;
            p1Left.color = Color.white;
            p1Right.color = Color.white;
            p2Left.color = Color.white;
            p2Right.color = Color.white;
            p3Left.color = Color.white;
            p3Right.color = Color.white;
            p4Left.color = Color.white;
            p4Right.color = Color.white;
        }
    }

    private void updateUIToPreferences()
    {
        volumePercentageText.text = ((int)(volumePercentage * 100)).ToString();

        if (toggles[0])
        {
            fullScreenToggleText.text = "On";
        }
        else
        {
            fullScreenToggleText.text = "Off";
        }

        if (toggles[1])
        {
            vSyncToggleText.text = "On";
        }
        else
        {
            vSyncToggleText.text = "Off";
        }

        if (toggles[2])
        {
            p1ToggleText.text = "GamePad";
        }
        else
        {
            p1ToggleText.text = "Keyboard";
        }

        if (toggles[3])
        {
            p2ToggleText.text = "GamePad";
        }
        else
        {
            p2ToggleText.text = "Keyboard";
        }

        if (toggles[4])
        {
            p3ToggleText.text = "GamePad";
        }
        else
        {
            p3ToggleText.text = "Keyboard";
        }

        if (toggles[5])
        {
            p4ToggleText.text = "GamePad";
        }
        else
        {
            p4ToggleText.text = "Keyboard";
        }
    }

    private void applyChanges()
    {
        AudioListener.volume = volumePercentage;
        Screen.fullScreen = toggles[0]; //Sets fullscreen to the bool associated with fullscreen

        //Add additional settings logic here

        //End of additional logic

        exitSettings();
    }

    private void exitSettings()
    {
        buttons[NUMBER_OF_SELECTION_OPTIONS - 1] = saveText;
        currentButton = 0;
        colorSelectedButton();
        discardText.color = Color.white;
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

}
