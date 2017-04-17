using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectRaceModeLogic : MonoBehaviour {

    private const string TRANSITION = "Sounds/KartEffects/screen_transition";

    public Sprite kitchenSprite;
    public Sprite bedRoomSprite;
    public Sprite bathRoomSprite;
    public Sprite livingRoomSprite;

    public Sprite customSprite;

    private AudioClip transition;
    private AudioSource source;
    private bool pressed;

    public Text levelText;
    public Image levelImage;
    public Text leftSelectionCursor;
    public Text rightSelectionCursor;

    private const int NUMBEROFBUTTONS = 5;

    private bool axisEnabled;

    private string[] levels;
    private int currentCustomIndex;

    private SceneGenerator sceneGenerator;

    private AccountManager account;

    void Start () {

        transition = (AudioClip)Resources.Load(TRANSITION);
        source = GameObject.Find("Sound").GetComponent<AudioSource>();
        pressed = false;

        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();

        axisEnabled = true;

        fetchLevelList();
        currentCustomIndex = 0;

        account = GameObject.Find("AccountManager").GetComponent<AccountManager>();

        levelText.text = levels[currentCustomIndex].Substring(0, levels[currentCustomIndex].Length - 4);
        levelText.color = account.getCurrColor;

        setTrackImage();
    }
	
	void Update () {

        if (!pressed)
        {
            if (SimpleInput.GetAxis("Vertical") == 0 && SimpleInput.GetAxis("Horizontal") == 0)
            {
                axisEnabled = true;
                leftSelectionCursor.color = Color.white;
                rightSelectionCursor.color = Color.white;
            }

            scrollMenu();
            buttonPress();
        }
        else
        {
            GameObject background = GameObject.Find("Background");

            foreach (Transform child in background.transform)
            {
                if(child.name != "Background")
                {
                    child.transform.Translate(Vector3.left * 1000.0f * Time.deltaTime);
                }
            }
        }
    }

    private void buttonPress()
    {
        if (SimpleInput.GetButtonDown("Pause") || SimpleInput.GetButtonDown("Bump Kart"))
        {
            sceneGenerator.LevelName = levels[currentCustomIndex];
            StartCoroutine(Transition());
        }

    }

    private void scrollMenu()
    {
        if (SimpleInput.GetAxis("Horizontal") > 0 && axisEnabled)
        {
            axisEnabled = false;
            currentCustomIndex++;
            if (currentCustomIndex >= levels.Length)
            {
                currentCustomIndex = 0;
            }
            levelText.text = levels[currentCustomIndex].Substring(0, levels[currentCustomIndex].Length - 4);

            rightSelectionCursor.color = account.getCurrColor;

            setTrackImage();
        }
        else if (SimpleInput.GetAxis("Horizontal") < 0 && axisEnabled)
        {
            axisEnabled = false;
            currentCustomIndex--;
            if(currentCustomIndex < 0)
            {
                currentCustomIndex = levels.Length - 1;
            }
            levelText.text = levels[currentCustomIndex].Substring(0, levels[currentCustomIndex].Length - 4);

            leftSelectionCursor.color = account.getCurrColor;

            setTrackImage();
        }
    }

    private void fetchLevelList()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
        FileInfo[] files = directoryInfo.GetFiles();
        List<string> levelList = new List<string>();

        levelList.Add("KitchenTrack1.xml");
        levelList.Add("BedroomTrack.xml");
        levelList.Add("BathroomTrack.xml");
        levelList.Add("Living Room 1.xml");

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

    private void setTrackImage()
    {
        if(currentCustomIndex == 0)
        {
            levelImage.sprite = kitchenSprite;
        }
        else if (currentCustomIndex == 1)
        {
            levelImage.sprite = bedRoomSprite;
        }
        else if (currentCustomIndex == 2)
        {
            levelImage.sprite = bathRoomSprite;
        }
        else if (currentCustomIndex == 3)
        {
            levelImage.sprite = livingRoomSprite;
        }
        else
        {
            levelImage.sprite = customSprite;
        }
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
