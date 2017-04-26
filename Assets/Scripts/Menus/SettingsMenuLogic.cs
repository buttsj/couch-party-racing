using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuLogic : MonoBehaviour
{

    public GameObject mainMenu;
    private GameObject accountMng;

    private const int NUMBER_OF_SELECTION_OPTIONS = 11;

    public Canvas credits;

    public Text volumePercentageText;
    public Text fullScreenToggleText;
    public Text vSyncToggleText;
    public Text p1ToggleText;
    public Text p2ToggleText;
    public Text p3ToggleText;
    public Text p4ToggleText;
    public Text saveText;
    public Text discardText;
    public Text creditsText;

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

    public Text resetUnlocks;
    public Text cursorToggleText;
    public Text cursorLeft;
    public Text cursorRight;
    public int cursorColor;
    public int[] possibleCursors;

    void Awake()
    {
        accountMng = GameObject.Find("AccountManager");

        buttons = new Text[NUMBER_OF_SELECTION_OPTIONS];

        buttons[0] = volumePercentageText;
        buttons[1] = fullScreenToggleText;
        buttons[2] = vSyncToggleText;
        buttons[3] = p1ToggleText;
        buttons[4] = p2ToggleText;
        buttons[5] = p3ToggleText;
        buttons[6] = p4ToggleText;
        buttons[7] = cursorToggleText; // cursor
        buttons[8] = resetUnlocks; // reset everything button
        buttons[9] = creditsText; // show credits scroll
        buttons[10] = saveText;

        currentButton = 0;

        axisEnabled = true;

        toggles = new bool[6];

        possibleCursors = new int[7] { 0, 1, 2, 3, 4, 5, 6 };


        // Set Default Settings
        if (!PlayerPrefs.HasKey("Settings: Volume"))
        {
            // Audio
            volumePercentage = AudioListener.volume;
            PlayerPrefs.SetFloat("Settings: Volume", volumePercentage);

            // Fullscreen
            toggles[0] = Screen.fullScreen;
            PlayerPrefs.SetInt("Settings: Fullscreen", toggles[0] ? 1 : 0);

            // VSync
            toggles[1] = QualitySettings.vSyncCount != 0;
            PlayerPrefs.SetInt("Settings: VSync", toggles[1] ? 1 : 0);

            // Player 1 Device
            toggles[2] = false;
            PlayerPrefs.SetString("Player 1 Controls", "Keyboard");

            // Player 2 Device
            toggles[3] = true;
            PlayerPrefs.SetString("Player 2 Controls", "Xbox");

            // Player 3 Device
            toggles[4] = true;
            PlayerPrefs.SetString("Player 3 Controls", "Xbox");

            // Player 4 Device
            toggles[5] = true;
            PlayerPrefs.SetString("Player 4 Controls", "Xbox");

            // Cursor color
            cursorColor = accountMng.GetComponent<AccountManager>().CursorSelection;

            PlayerPrefs.Save();
        }
        else
        {
            // Retrieve Saved Prefs
            volumePercentage = PlayerPrefs.GetFloat("Settings: Volume");
            AudioListener.volume = volumePercentage;

            toggles[0] = PlayerPrefs.GetInt("Settings: Fullscreen") != 0;
            Screen.fullScreen = toggles[0];

            toggles[1] = PlayerPrefs.GetInt("Settings: VSync") != 0;
            QualitySettings.vSyncCount = toggles[1] ? 1 : 0;

            toggles[2] = PlayerPrefs.GetString("Player 1 Controls") != "Keyboard";
            toggles[3] = PlayerPrefs.GetString("Player 2 Controls") != "Keyboard";
            toggles[4] = PlayerPrefs.GetString("Player 3 Controls") != "Keyboard";
            toggles[5] = PlayerPrefs.GetString("Player 4 Controls") != "Keyboard";

            cursorColor = accountMng.GetComponent<AccountManager>().CursorSelection;
        }

        updateUIToPreferences();
        SimpleInput.MapPlayersToDefaultPref();

        //buttons[currentButton].color = Color.cyan;
        buttons[currentButton].color = accountMng.GetComponent<AccountManager>().getCurrColor;
    }

    void Update()
    {
        buttons[currentButton].color = accountMng.GetComponent<AccountManager>().getCurrColor;
        cursorToggleText.text = accountMng.GetComponent<AccountManager>().getColorName;
        resetAxis();
        scrollMenu();
        updateUIToPreferences();
        buttonPress();
    }

    private void scrollMenu()
    {
        if (SimpleInput.GetAxis("Horizontal") < 0 && axisEnabled && currentButton == 0)
        {
            //volumeLeft.color = Color.cyan;
            volumeLeft.color = accountMng.GetComponent<AccountManager>().getCurrColor;
            volumePercentage -= 0.01f;
            if (volumePercentage < 0.0f)
            {
                volumePercentage = 0.0f;
            }

        }
        else if (SimpleInput.GetAxis("Horizontal") > 0 && axisEnabled && currentButton == 0)
        {
            //volumeRight.color = Color.cyan;
            volumeRight.color = accountMng.GetComponent<AccountManager>().getCurrColor;
            volumePercentage += 0.01f;
            if (volumePercentage > 1.0f)
            {
                volumePercentage = 1.0f;
            }

        }
        else if (SimpleInput.GetAxis("Horizontal") < 0 && axisEnabled && currentButton == 7)
        {
            axisEnabled = false;
            // shift color slider
            cursorColor--;
            if (cursorColor < 0)
                cursorColor = 6;
            accountMng.GetComponent<AccountManager>().CursorSelection = cursorColor;
            buttons[currentButton].color = accountMng.GetComponent<AccountManager>().getCurrColor;
            cursorLeft.color = accountMng.GetComponent<AccountManager>().getCurrColor;
            cursorToggleText.text = accountMng.GetComponent<AccountManager>().getColorName;
            PlayerPrefs.Save();
        }
        else if (SimpleInput.GetAxis("Horizontal") > 0 && axisEnabled && currentButton == 7)
        {
            axisEnabled = false;
            // shift color slider
            cursorColor++;
            if (cursorColor > 6)
                cursorColor = 0;
            accountMng.GetComponent<AccountManager>().CursorSelection = cursorColor;
            buttons[currentButton].color = accountMng.GetComponent<AccountManager>().getCurrColor;
            cursorRight.color = accountMng.GetComponent<AccountManager>().getCurrColor;
            cursorToggleText.text = accountMng.GetComponent<AccountManager>().getColorName;
            PlayerPrefs.Save();
        }
        else if (SimpleInput.GetAxis("Horizontal") > 0 && axisEnabled && currentButton == 8)
        {
            // do nothing
        }
        else if (SimpleInput.GetAxis("Horizontal") < 0 && axisEnabled && currentButton == 8)
        {
            // do nothing
        }
        else if (SimpleInput.GetAxis("Horizontal") > 0 && axisEnabled && currentButton == 9)
        {
            // do nothing
        }
        else if (SimpleInput.GetAxis("Horizontal") < 0 && axisEnabled && currentButton == 9)
        {
            // do nothing
        }
        else if (SimpleInput.GetAxis("Horizontal") < 0 && axisEnabled && currentButton > 0 && currentButton < NUMBER_OF_SELECTION_OPTIONS - 1)
        {
            axisEnabled = false;
            toggles[currentButton - 1] = !toggles[currentButton - 1];
            //buttons[currentButton].transform.GetChild(0).GetComponent<Text>().color = Color.cyan;
            buttons[currentButton].transform.GetChild(0).GetComponent<Text>().color = accountMng.GetComponent<AccountManager>().getCurrColor;
        }
        else if (SimpleInput.GetAxis("Horizontal") > 0 && axisEnabled && currentButton > 0 && currentButton < NUMBER_OF_SELECTION_OPTIONS - 1)
        {
            axisEnabled = false;
            toggles[currentButton - 1] = !toggles[currentButton - 1];
            //buttons[currentButton].transform.GetChild(1).GetComponent<Text>().color = Color.cyan;
            buttons[currentButton].transform.GetChild(1).GetComponent<Text>().color = accountMng.GetComponent<AccountManager>().getCurrColor;
        }
        else if (SimpleInput.GetAxis("Horizontal") != 0 && axisEnabled && currentButton == NUMBER_OF_SELECTION_OPTIONS - 1)
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
        else if ((SimpleInput.GetAxis("Vertical") < 0 && axisEnabled) || SimpleInput.GetButtonDown("Reverse"))
        {
            axisEnabled = false;
            currentButton++;
            if (currentButton >= NUMBER_OF_SELECTION_OPTIONS)
            {
                currentButton = 0;
            }
            colorSelectedButton();
        }
        else if ((SimpleInput.GetAxis("Vertical") > 0 && axisEnabled) || SimpleInput.GetButtonDown("Accelerate"))
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
        else if (SimpleInput.GetButtonDown("Bump Kart") && buttons[currentButton].text.ToLower() == "reset unlocks")
        {
            accountMng.GetComponent<AccountManager>().ResetEverything();
            accountMng.GetComponent<AccountManager>().RefreshUnlocks();
        }
        else if (SimpleInput.GetButtonDown("Bump Kart") && buttons[currentButton].text.ToLower() == "credits")
        {
            // roll the credits
            credits.gameObject.SetActive(true);
        }
    }

    private void colorSelectedButton()
    {
        for (int i = 0; i < NUMBER_OF_SELECTION_OPTIONS; i++)
        {
            buttons[i].color = Color.white;
        }
        //buttons[currentButton].color = Color.cyan;
        buttons[currentButton].color = accountMng.GetComponent<AccountManager>().getCurrColor;
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
            cursorLeft.color = Color.white;
            cursorRight.color = Color.white;
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
        // Audio
        PlayerPrefs.SetFloat("Settings: Volume", volumePercentage);
        AudioListener.volume = volumePercentage;

        // Fullscreen
        PlayerPrefs.SetInt("Settings: Fullscreen", toggles[0] ? 1 : 0);
        Screen.fullScreen = toggles[0]; //Sets fullscreen to the bool associated with fullscreen

        // VSync
        PlayerPrefs.SetInt("Settings: VSync", toggles[1] ? 1 : 0);
        QualitySettings.vSyncCount = toggles[1] ? 1 : 0;

        // Player 1 Device
        PlayerPrefs.SetString("Player 1 Controls", toggles[2] ? "Xbox" : "Keyboard");
        Debug.Log("Player 1: " + (toggles[2] ? "Xbox" : "Keyboard"));

        // Player 2 Device
        PlayerPrefs.SetString("Player 2 Controls", toggles[3] ? "Xbox" : "Keyboard");
        Debug.Log("Player 2: " + (toggles[3] ? "Xbox" : "Keyboard"));

        // Player 3 Device
        PlayerPrefs.SetString("Player 3 Controls", toggles[4] ? "Xbox" : "Keyboard");
        Debug.Log("Player 3: " + (toggles[4] ? "Xbox" : "Keyboard"));

        // Player 4 Device
        PlayerPrefs.SetString("Player 4 Controls", toggles[5] ? "Xbox" : "Keyboard");
        Debug.Log("Player 4: " + (toggles[5] ? "Xbox" : "Keyboard"));

        PlayerPrefs.Save();
        SimpleInput.MapPlayersToDefaultPref();

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
