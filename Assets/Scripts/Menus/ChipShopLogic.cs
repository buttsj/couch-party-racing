using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipShopLogic : MonoBehaviour {

    private const int NUMBER_OF_SELECTION_OPTIONS = 5;
    private const int NUMBER_OF_COLOR_OPTIONS = 6;

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

    private string[] customColorNames;
    private Color[] customColors;

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

    }
	
	void Update () {

        if (SimpleInput.GetAxis("Vertical") == 0 && SimpleInput.GetAxis("Horizontal") == 0)
        {
            axisEnabled = true;
        }

        scrollMenu();
        buttonPress();

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
        }
    }

    private void buttonPress()
    {

    }

}
