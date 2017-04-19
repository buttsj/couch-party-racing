using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChipShopLogic : MonoBehaviour {

    private const int NUMBER_OF_SELECTION_OPTIONS = 5;
    private const int NUMBER_OF_COLOR_OPTIONS = 6;

    private const string PURCHASE_AVAILABLE = "Purchase  ?";
    private const string PURCHASE_UNAVAILABLE = "Already Owned  !";
    private const string PURCHASED = "Purchased  !";
    private const string INVALID_FUNDS = "Invalid Funds  !";

    private const string REGISTER_SOUND = "Sounds/KartEffects/cash";

    private Text[] buttons;
    private int currentButton;

    public Image colorDisplay;

    public Text colorText;
    public Text cityCarText;
    public Text hearseText;
    public Text taxiText;
    public Text muscleText;

    private bool axisEnabled;

    private int colorIndex;

    private AccountManager account;

    private string[] customColorNames;
    private Color[] customColors;

    public GameObject standardKart;
    public GameObject cityCar;
    public GameObject hearse;
    public GameObject taxi;
    public GameObject muscle;

    public Image itemCostBackground;
    public Text itemCostText;
    public Text purchaseStatusText;

    public Text currentChipsText;

    private int[] carCosts;
    private int colorCost;

    private ChipShopCamera chipCamera;
    private AudioSource source;
    private AudioClip cash;

    void Start () {

        buttons = new Text[NUMBER_OF_SELECTION_OPTIONS];
        buttons[0] = colorText;
        buttons[1] = cityCarText;
        buttons[2] = hearseText;
        buttons[3] = taxiText;
        buttons[4] = muscleText;

        currentButton = 0;
        buttons[currentButton].color = Color.cyan;

        axisEnabled = true;

        initColorSelections();

        chipCamera = GameObject.Find("shop_camera").GetComponent<ChipShopCamera>();

        itemCostBackground.enabled = false;
        itemCostText.text = "";

        carCosts = new int[4];
        carCosts[0] = 30;
        carCosts[1] = 30;
        carCosts[2] = 30;
        carCosts[3] = 50;

        colorCost = 10;

        account = GameObject.Find("AccountManager").GetComponent<AccountManager>();
        currentChipsText.text = account.CurrentChips.ToString();

        source = GameObject.Find("Sound").GetComponent<AudioSource>();
        cash = (AudioClip)Resources.Load(REGISTER_SOUND);
    }

    void Update()
    {
        if (SimpleInput.GetAxis("Vertical") == 0 && SimpleInput.GetAxis("Horizontal") == 0)
        {
            axisEnabled = true;
        }
        scrollMenu();
        buttonPress();
        rotateSelection();
    }

    private void colorSelectedButton()
    {
        for (int i = 0; i < NUMBER_OF_SELECTION_OPTIONS; i++)
        {
            buttons[i].color = Color.white;
        }
        buttons[currentButton].color = Color.cyan;
    }

    private void initColorSelections()
    {
        colorIndex = 0;
        customColorNames = new string[NUMBER_OF_COLOR_OPTIONS];
        customColors = new Color[NUMBER_OF_COLOR_OPTIONS];

        customColorNames[0] = "Berry";
        customColors[0] = new Color(0.44f, 0.12f, 0.16f, 1.0f);

        customColorNames[1] = "Chocolate";
        customColors[1] = new Color(0.24f, 0.11f, 0.06f, 1.0f);

        customColorNames[2] = "Pink";
        customColors[2] = new Color(1.00f, 0.41f, 0.71f, 1.0f);

        customColorNames[3] = "Beige";
        customColors[3] = new Color(0.87f, 0.82f, 0.65f, 1.0f);

        customColorNames[4] = "Ice";
        customColors[4] = new Color(0.65f, 0.95f, 0.95f, 1.0f);

        customColorNames[5] = "MidnightBlack";
        customColors[5] = new Color(0.00f, 0.01f, 0.09f, 1.0f);

        colorText.text = "<  " + customColorNames[colorIndex] + "   >";
        colorDisplay.color = customColors[colorIndex];

    }

    private void scrollMenu()
    {
        if (SimpleInput.GetAxis("Horizontal") > 0 && axisEnabled && ReferenceEquals(buttons[currentButton], colorText))
        {
            axisEnabled = false;

            colorIndex++;

            if (colorIndex >= customColorNames.Length)
            {
                colorIndex = 0;
            }

            colorText.text = "<  " + customColorNames[colorIndex] + "   >";
            colorDisplay.color = customColors[colorIndex];

            colorSelectedButton();

            if (standardKart.activeInHierarchy)
            {
                itemCostText.text = "Cost : " + colorCost + " Chips";
                standardKart.GetComponentInChildren<Renderer>().material.color = customColors[colorIndex];
                setStatusText(customColorNames[colorIndex]);
            }
        }
        else if (SimpleInput.GetAxis("Horizontal") < 0 && axisEnabled && ReferenceEquals(buttons[currentButton], colorText))
        {
            axisEnabled = false;

            colorIndex--;
            if (colorIndex < 0)
            {
                colorIndex = customColorNames.Length - 1;
            }
            colorText.text = "<  " + customColorNames[colorIndex] + "   >";
            colorDisplay.color = customColors[colorIndex];

            colorSelectedButton();

            if (standardKart.activeInHierarchy)
            {
                itemCostText.text = "Cost : " + colorCost + " Chips";
                standardKart.GetComponentInChildren<Renderer>().material.color = customColors[colorIndex];
                setStatusText(customColorNames[colorIndex]);
            }

        }
        else if((SimpleInput.GetAxis("Vertical") < 0 || SimpleInput.GetButtonDown("Reverse")) && axisEnabled)
        {
            axisEnabled = false;

            currentButton++;
            if(currentButton > NUMBER_OF_SELECTION_OPTIONS - 1)
            {
                currentButton = 0;
            }
            colorSelectedButton();

            unHighlightKart();
        }
        else if ((SimpleInput.GetAxis("Vertical") > 0 || SimpleInput.GetButtonDown("Accelerate")) && axisEnabled)
        {
            axisEnabled = false;

            currentButton--;
            if (currentButton < 0)
            {
                currentButton = NUMBER_OF_SELECTION_OPTIONS - 1;
            }
            colorSelectedButton();

            unHighlightKart();
        }
    }

    private void unHighlightKart()
    {
        chipCamera.FocusOut();
        itemCostBackground.enabled = false;
        itemCostText.text = "";
        purchaseStatusText.text = "";

        standardKart.SetActive(false);
        cityCar.SetActive(false);
        hearse.SetActive(false);
        taxi.SetActive(false);
        muscle.SetActive(false);

        standardKart.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        cityCar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        hearse.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        taxi.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        muscle.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    private void rotateSelection()
    {
        if (standardKart.activeInHierarchy)
        {
            standardKart.transform.Rotate(Vector3.up, 60.0f * Time.deltaTime);
        }
        else if (cityCar.activeInHierarchy)
        {
            cityCar.transform.Rotate(Vector3.up, 60.0f * Time.deltaTime);
        }
        else if (hearse.activeInHierarchy)
        {
            hearse.transform.Rotate(Vector3.up, 60.0f * Time.deltaTime);
        }
        else if (taxi.activeInHierarchy)
        {
            taxi.transform.Rotate(Vector3.up, 60.0f * Time.deltaTime);
        }
        else if (muscle.activeInHierarchy)
        {
            muscle.transform.Rotate(Vector3.up, 60.0f * Time.deltaTime);
        }
    }

    private void buttonPress()
    {
        if(SimpleInput.GetButtonDown("Cancel") || SimpleInput.GetButtonDown("Boost"))
        {
            account.RefreshUnlocks();
            SceneManager.LoadScene(0);
        }
        else if (ReferenceEquals(buttons[currentButton], colorText) && SimpleInput.GetButtonDown("Bump Kart"))
        {
            if (standardKart.activeInHierarchy)
            {
                purchaseSelection(colorCost, customColorNames[colorIndex]);
            }
            else
            {
                standardKart.SetActive(true);
                chipCamera.FocusIn();
                itemCostBackground.enabled = true;
                itemCostText.text = "Cost : " + colorCost + " Chips";
                standardKart.GetComponentInChildren<Renderer>().material.color = customColors[colorIndex];
                setStatusText(customColorNames[colorIndex]);
            }
        }
        else if(ReferenceEquals(buttons[currentButton], cityCarText) && SimpleInput.GetButtonDown("Bump Kart"))
        {
            if (cityCar.activeInHierarchy)
            {
                purchaseSelection(carCosts[currentButton - 1], "CityCar");
            }
            else
            {
                cityCar.SetActive(true);
                chipCamera.FocusIn();
                itemCostBackground.enabled = true;
                itemCostText.text = "Cost : " + carCosts[currentButton - 1] + " Chips";
                setStatusText("CityCar");
            }
        }
        else if (ReferenceEquals(buttons[currentButton], hearseText) && SimpleInput.GetButtonDown("Bump Kart"))
        {
            if (hearse.activeInHierarchy)
            {
                purchaseSelection(carCosts[currentButton - 1], "Hearse");
            }
            else
            {
                hearse.SetActive(true);
                chipCamera.FocusIn();
                itemCostBackground.enabled = true;
                itemCostText.text = "Cost : " + carCosts[currentButton - 1] + " Chips";
                setStatusText("Hearse");
            }
        }
        else if (ReferenceEquals(buttons[currentButton], taxiText) && SimpleInput.GetButtonDown("Bump Kart"))
        {
            if (taxi.activeInHierarchy)
            {
                purchaseSelection(carCosts[currentButton - 1], "Taxi");
            }
            else
            {
                taxi.SetActive(true);
                chipCamera.FocusIn();
                itemCostBackground.enabled = true;
                itemCostText.text = "Cost : " + carCosts[currentButton - 1] + " Chips";
                setStatusText("Taxi");
            }
        }
        else if (ReferenceEquals(buttons[currentButton], muscleText) && SimpleInput.GetButtonDown("Bump Kart"))
        {
            if (muscle.activeInHierarchy)
            {
                purchaseSelection(carCosts[currentButton - 1], "Muscle");
            }
            else
            {
                muscle.SetActive(true);
                chipCamera.FocusIn();
                itemCostBackground.enabled = true;
                itemCostText.text = "Cost : " + carCosts[currentButton - 1] + " Chips";
                setStatusText("Muscle");
            }
        }
    }

    private void purchaseSelection(int cost, string item)
    {
        if (!account.unlockStatus(item))
        {
            bool purchStatus = account.purchaseItem(cost, item);
            if(purchStatus == true)
            {
                purchaseStatusText.text = PURCHASED;
                purchaseStatusText.color = Color.blue;
                source.PlayOneShot(cash);
            }
            else
            {
                purchaseStatusText.text = INVALID_FUNDS;
                purchaseStatusText.color = Color.red;
            }

            currentChipsText.text = account.CurrentChips.ToString();
        }
    }

    private void setStatusText(string item)
    {
        if (account.unlockStatus(item))
        {
            purchaseStatusText.text = PURCHASE_UNAVAILABLE;
            purchaseStatusText.color = Color.red;
        }
        else
        {
            purchaseStatusText.text = PURCHASE_AVAILABLE;
            purchaseStatusText.color = Color.green;
        }
    }

}
