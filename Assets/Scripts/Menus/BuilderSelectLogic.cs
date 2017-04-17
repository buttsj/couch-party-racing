using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BuilderSelectLogic : MonoBehaviour {

    private const int NUMBER_OF_SELECTION_OPTIONS = 2;
    private const int MAX_LEVEL_NAME_LENGTH = 12;

    private Text[] buttons;
    private int currentButton;

    public Text newLevelButton;
    public Text loadLevelButton;

    public GameObject keyboardMenu;

    private bool axisEnabled;

    private string[] levels;
    private int currentCustomIndex;

    public Text customLevelText;

    private Image[,] keyboardKeys;
    private int keyRow;
    private int keyColumn;

    private SceneGenerator sceneGenerator;

    void Start () {
        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();
        
        initKeyboard();

        buttons = new Text[NUMBER_OF_SELECTION_OPTIONS];
        buttons[0] = newLevelButton;
        buttons[1] = loadLevelButton;

        currentButton = 0;
        buttons[currentButton].color = Color.cyan;

        axisEnabled = true;

        fetchLevelList();
        currentCustomIndex = 0;
        if (levels.Length > 0)
        {
            loadLevelButton.text = "<  " + levels[currentCustomIndex].Substring(0, levels[currentCustomIndex].Length - 4) + "   >";
        }
        else
        {
            loadLevelButton.text = "<  No Custom Levels Found   >";
        }

    }
	
	void Update () {

        if (SimpleInput.GetAxis("Vertical") == 0 && SimpleInput.GetAxis("Horizontal") == 0)
        {
            axisEnabled = true;
        }

        if (!keyboardMenu.activeInHierarchy)
        {
            scrollMenu();
            buttonPress();
        }
        else
        {
            scrollKeyboard();
            handleKeyboardPress();
        }

	}

    private void initKeyboard()
    {
        keyboardKeys = new Image[4, 7];
        int childNum = 2;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                keyboardKeys[i, j] = keyboardMenu.transform.GetChild(childNum).GetComponent<Image>();
                childNum++;
            }
        }
        keyRow = 0;
        keyColumn = 0;

        keyboardKeys[keyRow, keyColumn].color = Color.cyan;

        keyboardMenu.SetActive(false);
    }

    private void scrollMenu()
    {
        if(SimpleInput.GetAxis("Horizontal") > 0 && axisEnabled && ReferenceEquals(buttons[currentButton], loadLevelButton))
        {
            axisEnabled = false;
            currentCustomIndex++;

            if (currentCustomIndex >= levels.Length)
            {
                currentCustomIndex = 0;
            }

            loadLevelButton.text = "<  " + levels[currentCustomIndex].Substring(0, levels[currentCustomIndex].Length - 4) + "   >";
        }
        else if (SimpleInput.GetAxis("Horizontal") < 0 && axisEnabled && ReferenceEquals(buttons[currentButton], loadLevelButton))
        {
            axisEnabled = false;
            currentCustomIndex--;
            if (currentCustomIndex < 0)
            {
                currentCustomIndex = levels.Length - 1;
            }
            loadLevelButton.text = "<  " + levels[currentCustomIndex].Substring(0, levels[currentCustomIndex].Length - 4) + "   >";
        }
        else if((SimpleInput.GetAxis("Vertical") != 0 && axisEnabled) || SimpleInput.GetButtonDown("Reverse") || SimpleInput.GetButtonDown("Accelerate"))
        {
            axisEnabled = false;

            currentButton++;
            currentButton %= 2;

            colorSelectedButton();
        }
    }

    private void buttonPress()
    {
        if (SimpleInput.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(0);
        }
        else if (SimpleInput.GetButtonDown("Bump Kart") && ReferenceEquals(buttons[currentButton], newLevelButton))
        {
            handleNewLevelPress();
        }
        else if (SimpleInput.GetButtonDown("Bump Kart") && ReferenceEquals(buttons[currentButton], loadLevelButton))
        {
            handleExistingLevelLoad();
        }
    }

    private void colorSelectedButton()
    {
        for (int i = 0; i < NUMBER_OF_SELECTION_OPTIONS; i++)
        {
            buttons[i].color = Color.white;
        }
        buttons[currentButton].color = Color.cyan;
    }

    private void colorSelectedKey()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                keyboardKeys[i, j].color = Color.white;
            }
        }
        keyboardKeys[keyRow, keyColumn].color = Color.cyan;
    }

    private void scrollKeyboard()
    {
        if (SimpleInput.GetAxis("Horizontal") > 0 && axisEnabled)
        {
            axisEnabled = false;
            keyColumn++;
            if (keyColumn > 6)
            {
                keyColumn = 6;
            }
            colorSelectedKey();
        }
        else if (SimpleInput.GetAxis("Horizontal") < 0 && axisEnabled)
        {
            axisEnabled = false;
            keyColumn--;
            if (keyColumn < 0)
            {
                keyColumn = 0;
            }
            colorSelectedKey();
        }
        else if ((SimpleInput.GetAxis("Vertical") < 0 && axisEnabled) || SimpleInput.GetButtonDown("Reverse"))
        {
            axisEnabled = false;
            keyRow++;
            if (keyRow > 3)
            {
                keyRow = 3;
            }
            colorSelectedKey();
        }
        else if ((SimpleInput.GetAxis("Vertical") > 0 && axisEnabled) || SimpleInput.GetButtonDown("Accelerate"))
        {
            axisEnabled = false;
            keyRow--;
            if (keyRow < 0)
            {
                keyRow = 0;
            }
            colorSelectedKey();
        }

    }

    private void fetchLevelList()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
        FileInfo[] files = directoryInfo.GetFiles();
        List<string> levelList = new List<string>();
        Debug.Log(Application.dataPath);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension == ".xml" && files[i].Name.Length > 4 && files[i].Name != "BathroomTrack.xml"
                && files[i].Name != "BedroomTrack.xml" && files[i].Name != "KitchenTrack1.xml" && files[i].Name != "Living Room 1.xml")
            {
                levelList.Add(files[i].Name);
            }
        }

        levels = levelList.ToArray();
    }

    private void handleKeyboardPress()
    {
        int customLevelTextLength = customLevelText.text.Length;
        if (SimpleInput.GetButtonDown("Boost") || SimpleInput.GetButtonDown("Cancel"))
        {
            keyboardMenu.SetActive(false);
        }
        else if(SimpleInput.GetButtonDown("Bump Kart") && keyRow == 3 && keyColumn == 6)
        {
            if(customLevelTextLength > 0)
            {
                handleNewLevelLoad();
            }
        }
        else if(SimpleInput.GetButtonDown("Bump Kart") && keyRow == 3 && keyColumn == 5)
        {
            if(customLevelTextLength <= 1)
            {
                customLevelText.text = "";
            }
            else
            {
                customLevelText.text = customLevelText.text.Substring(0, customLevelText.text.Length - 1);
            }
            
        }
        else if(SimpleInput.GetButtonDown("Bump Kart") && customLevelTextLength < MAX_LEVEL_NAME_LENGTH)
        {
            customLevelText.text += keyboardKeys[keyRow, keyColumn].transform.GetChild(0).name;
        }
    }

    private void handleNewLevelPress()
    {
        keyboardMenu.SetActive(true);
        customLevelText.text = "";
    }

    private void handleExistingLevelLoad()
    {
        sceneGenerator.LevelName = convertTextToFiles(loadLevelButton.text);
        sceneGenerator.LoadScene();
    }

    private void handleNewLevelLoad()
    {
        sceneGenerator.LevelName = convertTextToFiles(customLevelText.text);
        sceneGenerator.LoadScene();
    }

    private string convertTextToFiles(string text) {
        char[] charsToTrim = { '<', ' ', '>' }; 
        string fileName = text.Trim(charsToTrim) + ".xml";

        return fileName;
    }

}
