using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectRaceModeLogic : MonoBehaviour {

    private const string TRANSITION = "Sounds/KartEffects/screen_transition";
    private AudioClip transition;
    private AudioSource source;
    private bool pressed;

    public Text kitchenText;
    public Text bedRoomText;
    public Text livingRoomText;
    public Text bathRoomText;
    public Text customText;

    private const int NUMBEROFBUTTONS = 5;

    private Text[] buttons;
    private int currentButton;
    private int previousButton;

    private bool axisEnabled;

    private string[] levels;
    private int currentCustomIndex;

    private SceneGenerator sceneGenerator;

    void Start () {

        transition = (AudioClip)Resources.Load(TRANSITION);
        source = GameObject.Find("Sound").GetComponent<AudioSource>();
        pressed = false;

        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();

        buttons = new Text[NUMBEROFBUTTONS];
        buttons[0] = kitchenText;
        buttons[1] = bedRoomText;
        buttons[2] = livingRoomText;
        buttons[3] = bathRoomText;
        buttons[4] = customText;

        currentButton = 0;
        previousButton = 0;

        axisEnabled = true;

        buttons[currentButton].color = Color.gray;

        fetchLevelList();
        currentCustomIndex = 0;
        if(levels.Length > 0)
        {
            customText.text = "<  " + levels[currentCustomIndex].Substring(0, levels[currentCustomIndex].Length - 4) + "   >";
        }
        else
        {
            customText.text = "<  Create some levels in the Track Builder!   >";
        }
        
    }
	
	void Update () {

        if (!pressed)
        {
            if (SimpleInput.GetAxis("Vertical") == 0 && SimpleInput.GetAxis("Horizontal") == 0)
            {
                axisEnabled = true;
            }

            scrollMenu();
            buttonPress();
        }


    }

    private void buttonPress()
    {
        if (SimpleInput.GetButtonDown("Bump Kart"))
        {
            if (ReferenceEquals(buttons[currentButton], kitchenText))
            {
                sceneGenerator.LevelName = "KitchenTrack1.xml";
                StartCoroutine(Transition());
            }
            else if (ReferenceEquals(buttons[currentButton], bedRoomText))
            {
                sceneGenerator.LevelName = "BedroomTrack.xml";
                StartCoroutine(Transition());
            }
            else if (ReferenceEquals(buttons[currentButton], livingRoomText))
            {
            }
            else if (ReferenceEquals(buttons[currentButton], bathRoomText))
            {
            }
            else if (ReferenceEquals(buttons[currentButton], customText))
            {
                sceneGenerator.LevelName = levels[currentCustomIndex];
                StartCoroutine(Transition());
            }
        }
    }

    private void scrollMenu()
    {
        if ((SimpleInput.GetAxis("Horizontal", 1) > 0 && (currentButton != NUMBEROFBUTTONS - 1) && axisEnabled))
        {
            axisEnabled = false;
            currentButton++;
            if (currentButton >= NUMBEROFBUTTONS - 1)
            {
                currentButton = 0;
            }
            colorSelectedButton();
        }
        else if ((SimpleInput.GetAxis("Horizontal", 1) < 0 && (currentButton != NUMBEROFBUTTONS - 1) && axisEnabled))
        {
            axisEnabled = false;
            currentButton--;
            if (currentButton < 0)
            {
                currentButton = NUMBEROFBUTTONS - 2;
            }
            colorSelectedButton();
        }
        else if (SimpleInput.GetAxis("Horizontal", 1) > 0 && axisEnabled && levels.Length > 0)
        {
            axisEnabled = false;
            currentCustomIndex++;
            if (currentCustomIndex >= levels.Length)
            {
                currentCustomIndex = 0;
            }
            customText.text = "<  " + levels[currentCustomIndex].Substring(0, levels[currentCustomIndex].Length - 4) + "   >";
        }
        else if (SimpleInput.GetAxis("Horizontal", 1) < 0 && axisEnabled && levels.Length > 0)
        {
            axisEnabled = false;
            currentCustomIndex--;
            if(currentCustomIndex < 0)
            {
                currentCustomIndex = levels.Length - 1;
            }
            customText.text = "<  " + levels[currentCustomIndex].Substring(0, levels[currentCustomIndex].Length - 4) + "   >";
        }
        else if(((SimpleInput.GetAxis("Vertical", 1) != 0) || SimpleInput.GetButtonDown("Reverse") || SimpleInput.GetButtonDown("Accelerate")) && axisEnabled && levels.Length > 0)
        {
            axisEnabled = false;
            if(currentButton == NUMBEROFBUTTONS - 1)
            {
                currentButton = previousButton;
            }
            else
            {
                previousButton = currentButton;
                currentButton = NUMBEROFBUTTONS - 1;
            }
            colorSelectedButton();
        }
    }

    private void colorSelectedButton()
    {
        for (int i = 0; i < NUMBEROFBUTTONS; i++)
        {
            buttons[i].color = Color.white;
        }
        buttons[currentButton].color = Color.gray;
    }

    private void fetchLevelList()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
        FileInfo[] files = directoryInfo.GetFiles();
        List<string> levelList = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension == ".xml")
            {
                levelList.Add(files[i].Name);
            }
        }

        levels = levelList.ToArray();
    }

    private void GoToNextMenu()
    {
        SceneManager.LoadScene("SelectionMenu");
    }

    private IEnumerator Transition()
    {
        source.clip = transition;
        source.loop = false;
        pressed = true;
        source.Play();
        yield return new WaitWhile(() => source.isPlaying);
        GoToNextMenu();
    }
}
