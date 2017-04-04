using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionMenuFunctionality : MonoBehaviour {

    private WhiteFadeUniversal fader;

    private int player1Color;
    private int player2Color;
    private int player3Color;
    private int player4Color;

    public Text player1ColorText;
    public Text player2ColorText;
    public Text player3ColorText;
    public Text player4ColorText;

    private int player1Kart;
    private int player2Kart;
    private int player3Kart;
    private int player4Kart;

    public Text player1KartText;
    public Text player2KartText;
    public Text player3KartText;
    public Text player4KartText;

    private List<string> kartColorName = new List<string>
    {
        "Blue",
        "Gray",
        "Red",
        "Magenta",
        "Green",
        "Yellow",
        "Cyan",
    };

    private List<Color> kartColorList = new List<Color> {
        Color.blue,
        Color.gray,
        Color.red,
        Color.magenta,
        Color.green,
        Color.yellow,
        Color.cyan,
    };

    private Dictionary<string, Color> rareColorDictionary = new Dictionary<string, Color>()
    {
        {"Berry", new Color(0.44f, 0.12f, 0.16f, 1.0f)},
        {"Chocolate", new Color(0.24f, 0.11f, 0.06f, 1.0f)},
        {"Pink", new Color(1.00f, 0.41f, 0.71f, 1.0f)},
        {"Beige", new Color(0.87f, 0.82f, 0.65f, 1.0f)},
        {"Ice", new Color(0.65f, 0.95f, 0.95f, 1.0f)},
        {"MidnightBlack", new Color(0.00f, 0.01f, 0.09f, 1.0f)}
    };

    private List<string> kartNames = new List<string>
    {
        "Default"
    };

    public Text loading;

    public const string READY = "Ready";
    public const string UNREADY = "Press Start";

    public Text player1ReadyText;
    public Text player2ReadyText;
    public Text player3ReadyText;
    public Text player4ReadyText;

    public Text startToContinueText;

    private SceneGenerator sceneGenerator;

    void Awake()
    {
        // fade in/out initializer
        GameObject fadeObject = new GameObject();
        fadeObject.name = "Fader";
        fadeObject.transform.SetParent(transform);
        fadeObject.SetActive(true);
        fader = fadeObject.AddComponent<WhiteFadeUniversal>();
        fader.BeginExitScene("Sound");
        //
    }

    void Start() {
        GameObject acct = GameObject.Find("AccountManager");
        AccountManager mng = acct.GetComponent<AccountManager>();

        foreach (KeyValuePair<string, int> pair in mng.GetColorUnlocks)
        {
            if (pair.Value == 1)
            {
                kartColorName.Add(pair.Key);
                kartColorList.Add(rareColorDictionary[pair.Key]);
            }
        }

        foreach (KeyValuePair<string, int> pair in mng.GetCarUnlocks)
        {
            if (pair.Value == 1)
            {
                kartNames.Add(pair.Key);
            }
        }

        player1Color = 0;
        player1ColorText.text = kartColorName[player1Color];
        player1ColorText.color = kartColorList[player1Color];

        player2Color = 1;
        player2ColorText.text = kartColorName[player2Color];
        player2ColorText.color = kartColorList[player2Color];

        player3Color = 2;
        player3ColorText.text = kartColorName[player3Color];
        player3ColorText.color = kartColorList[player3Color];

        player4Color = 3;
        player4ColorText.text = kartColorName[player4Color];
        player4ColorText.color = kartColorList[player4Color];

        player1Kart = 0;
        player1KartText.text = kartNames[player1Kart];

        player2Kart = 0;
        player2KartText.text = kartNames[player2Kart];

        player3Kart = 0;
        player3KartText.text = kartNames[player3Kart];

        player4Kart = 0;
        player4KartText.text = kartNames[player4Kart];

        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();

        player1ReadyText.text = UNREADY;
        player2ReadyText.text = UNREADY;
        player3ReadyText.text = UNREADY;
        player4ReadyText.text = UNREADY;

        startToContinueText.text = "";

        SimpleInput.MapPlayersToDefaultPref();
    }

    void Update() {
        player1ColorText.text = kartColorName[player1Color];
        player1ColorText.color = kartColorList[player1Color];
        
        player2ColorText.text = kartColorName[player2Color];
        player2ColorText.color = kartColorList[player2Color];
        
        player3ColorText.text = kartColorName[player3Color];
        player3ColorText.color = kartColorList[player3Color];
        
        player4ColorText.text = kartColorName[player4Color];
        player4ColorText.color = kartColorList[player4Color];

        player1KartText.text = kartNames[player1Kart];
        player2KartText.text = kartNames[player2Kart];
        player3KartText.text = kartNames[player3Kart];
        player4KartText.text = kartNames[player4Kart];

        if (sceneGenerator.GamemodeName == "RaceMode" && SimpleInput.GetButtonDown("Pause", 1) && (player1ReadyText.text == READY)) {
            StartCoroutine(LoadScene());
        } else if (SimpleInput.GetButtonDown("Pause", 1) && (player1ReadyText.text == READY) && NumberOfReadyPlayers() > 1) {
            StartCoroutine(LoadScene());
        } else {
            checkForReadyPlayers();
        }
    }

    private IEnumerator LoadScene() {
        // Configure Controls (Player Testing Order Matters)

        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;

        loading.enabled = true;
        loading.transform.SetAsLastSibling();

        SimpleInput.ClearCurrentPlayerDevices();

        if (player1ReadyText.text == READY) {
            SimpleInput.MapPlayerToDevice(1);
        }

        if (player2ReadyText.text == READY) {
            SimpleInput.MapPlayerToDevice(2);
        }

        if (player3ReadyText.text == READY) {
            SimpleInput.MapPlayerToDevice(3);
        }

        if (player4ReadyText.text == READY) {
            SimpleInput.MapPlayerToDevice(4);
        }

        sceneGenerator.ClearColors();
        sceneGenerator.ClearKarts();
        sceneGenerator.KartColorizer = kartColorList[player1Color];
        sceneGenerator.KartColorizer = kartColorList[player2Color];
        sceneGenerator.KartColorizer = kartColorList[player3Color];
        sceneGenerator.KartColorizer = kartColorList[player4Color];
        sceneGenerator.KartDesigner = kartNames[player1Kart];
        sceneGenerator.KartDesigner = kartNames[player2Kart];
        sceneGenerator.KartDesigner = kartNames[player3Kart];
        sceneGenerator.KartDesigner = kartNames[player4Kart];
        sceneGenerator.LoadScene();
    }

    private void checkForReadyPlayers() {
        // player 1
        if (SimpleInput.GetButtonDown("Bump Kart", 1))
        {
            player1Color += 1;
            if (player1Color >= kartColorList.Count)
                player1Color = 0;
        }
        else if (SimpleInput.GetButtonDown("Pause", 1) && (player1ReadyText.text == UNREADY))
        {
            player1ReadyText.text = READY;
            //startToContinueText.text = "Press Start to Continue!";
        }
        else if ((SimpleInput.GetButtonDown("Boost", 1) && !SimpleInput.GetButtonDown("Pause", 1)) && (player1ReadyText.text == READY))
        {
            player1ReadyText.text = UNREADY;
            startToContinueText.text = "";
        }
        else if (SimpleInput.GetButtonDown("Boost", 1))
        {
            player1Kart += 1;
            if (player1Kart >= kartNames.Count)
                player1Kart = 0;
        }
        
        // player 2
        if (SimpleInput.GetButtonDown("Bump Kart", 2))
        {
            player2Color += 1;
            if (player2Color >= kartColorList.Count)
                player2Color = 0;
        }
        else if (SimpleInput.GetButtonDown("Pause", 2) && (player2ReadyText.text == UNREADY))
        {
            player2ReadyText.text = READY;
        }
        else if (SimpleInput.GetButtonDown("Boost", 2) && (player2ReadyText.text == READY))
        {
            player2ReadyText.text = UNREADY;
        }
        else if (SimpleInput.GetButtonDown("Boost", 2))
        {
            player2Kart += 1;
            if (player2Kart >= kartNames.Count)
                player2Kart = 0;
        }

        // player 3
        if (SimpleInput.GetButtonDown("Bump Kart", 3))
        {
            player3Color += 1;
            if (player3Color >= kartColorList.Count)
                player3Color = 0;
        }
        else if (SimpleInput.GetButtonDown("Pause", 3) && (player3ReadyText.text == UNREADY))
        {
            player3ReadyText.text = READY;
        }
        else if (SimpleInput.GetButtonDown("Boost", 3) && (player3ReadyText.text == READY))
        {
            player3ReadyText.text = UNREADY;
        }
        else if (SimpleInput.GetButtonDown("Boost", 3))
        {
            player3Kart += 1;
            if (player3Kart >= kartNames.Count)
                player3Kart = 0;
        }

        // player 4
        if (SimpleInput.GetButtonDown("Bump Kart", 4))
        {
            player4Color += 1;
            if (player4Color >= kartColorList.Count)
                player4Color = 0;
        }
        else if (SimpleInput.GetButtonDown("Pause", 4) && (player4ReadyText.text == UNREADY))
        {
            player4ReadyText.text = READY;
        }
        else if (SimpleInput.GetButtonDown("Boost", 4) && (player4ReadyText.text == READY))
        {
            player4ReadyText.text = UNREADY;
        }
        else if (SimpleInput.GetButtonDown("Boost", 4))
        {
            player4Kart += 1;
            if (player4Kart >= kartNames.Count)
                player4Kart = 0;
        }

        // Ready to start
        if (sceneGenerator.GamemodeName == "RaceMode" && player1ReadyText.text == READY) {
            startToContinueText.text = "Press Start to Continue";
        } else if (player1ReadyText.text == READY && NumberOfReadyPlayers() > 1) {
            startToContinueText.text = "Press Start to Continue";
        }
    }


    private int NumberOfReadyPlayers() {
        int count = 0;
        if (player1ReadyText.text == READY) {
            count++;
        }

        if (player2ReadyText.text == READY) {
            count++;
        }

        if (player3ReadyText.text == READY) {
            count++;
        }

        if (player4ReadyText.text == READY) {
            count++;
        }
        return count;
    }
}
