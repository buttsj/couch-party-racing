using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChipShopMenuFunctionality : MonoBehaviour {

    public Text chips;
    public Text invalid;
    public Text costText;
    public GameObject moreInfo;
    public GameObject purchaseMenu;
    public GameObject kart;
    public GameObject colorHolder;
    private WhiteFadeUniversal fader;
    private AudioSource source;
    private AudioClip cash;
    private const string REGISTER = "Sounds/KartEffects/cash";
    private const string COST = " CHIPS";

    private string currentKart;
    private string currentColor;
    private int currentCost;
    private int purchaseNum;
    private Dictionary<string, int> unlocks;
    private bool refreshUnlocks;
    private bool rotateKart;
    private bool colorPicked;
    private bool kartPicked;

    void Awake()
    {
        GameObject fadeObject = new GameObject();
        fadeObject.name = "Fader";
        fadeObject.transform.SetParent(transform);
        fadeObject.SetActive(true);
        fader = fadeObject.AddComponent<WhiteFadeUniversal>();
        fader.BeginNewScene("Sound");
    }

    // Use this for initialization
    void Start () {
        source = GameObject.Find("Sound").GetComponent<AudioSource>();
        cash = (AudioClip)Resources.Load(REGISTER);
        GameObject acct = GameObject.Find("AccountManager");
        chips.text = acct.GetComponent<AccountManager>().CurrentChips.ToString();
        LoadUnlocks();
        refreshUnlocks = false;
        rotateKart = false;
        colorPicked = false;
        kartPicked = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (SimpleInput.GetButtonDown("Cancel"))
        {
            StartCoroutine(LeaveScene());
        }
        if (SimpleInput.GetButtonDown("Bump Kart"))
        {
            GameObject.Find("AccountManager").GetComponent<AccountManager>().CurrentChips++;
            chips.text = GameObject.Find("AccountManager").GetComponent<AccountManager>().CurrentChips.ToString();
        }
        if (refreshUnlocks == true)
        {
            refreshUnlocks = false;
            LoadUnlocks();
            chips.text = GameObject.Find("AccountManager").GetComponent<AccountManager>().CurrentChips.ToString();
        }
        if (rotateKart)
        {
            kart.transform.Rotate(Vector3.up, 10.0f * Time.deltaTime);
        }
    }

    private void LoadUnlocks()
    {
        GameObject acct = GameObject.Find("AccountManager");
        acct.GetComponent<AccountManager>().RefreshUnlocks();
        unlocks = acct.GetComponent<AccountManager>().GetUnlocks;
        foreach (KeyValuePair<string, int> pair in unlocks)
        {
            if (pair.Value == 0)
            {
                colorHolder.transform.FindChild(pair.Key).gameObject.SetActive(true);
            }
            else if(pair.Value == 1)
            {
                colorHolder.transform.FindChild(pair.Key).gameObject.SetActive(false);
            }
        }
    }

    public void MoreInfoButton()
    {
        Debug.Log("checking more info");
        moreInfo.SetActive(true);
    }

    public void ShowPurchase(int choice)
    {
        purchaseNum = choice;
        purchaseMenu.SetActive(true);
        kart.SetActive(true);
        rotateKart = true;
        invalid.enabled = false;
        switch (choice)
        {
            case 1:
                currentColor = "Berry";
                currentCost = 10;
                colorPicked = true;
                break;
            case 2:
                currentColor = "Chocolate";
                currentCost = 10;
                colorPicked = true;
                break;
            case 3:
                currentColor = "Pink";
                currentCost = 10;
                colorPicked = true;
                break;
            case 4:
                currentColor = "Beige";
                currentCost = 10;
                colorPicked = true;
                break;
            case 5:
                currentColor = "Ice";
                currentCost = 10;
                colorPicked = true;
                break;
            case 6:
                currentColor = "MidnightBlack";
                currentCost = 10;
                colorPicked = true;
                break;
            case 7:
                // kart 1
                currentKart = "";
                currentCost = 30;
                kartPicked = true;
                break;
            case 8:
                // kart 2
                currentKart = "";
                currentCost = 30;
                kartPicked = true;
                break;
            case 9:
                // kart 3
                currentKart = "";
                currentCost = 30;
                kartPicked = true;
                break;
            case 10:
                // kart 4
                currentKart = "";
                currentCost = 50;
                kartPicked = true;
                break;
            case 11:
                // kart 5
                currentKart = "";
                currentCost = 50;
                kartPicked = true;
                break;
        }
        if (colorPicked)
        {
            GameObject btn = GameObject.Find(currentColor);
            kart.GetComponentInChildren<Renderer>().material.color = btn.GetComponent<Image>().color;
        }
        if (kartPicked)
        {

        }
        costText.text = currentCost.ToString() + COST;
    }

    public void PurchaseMade()
    {
        GameObject acct = GameObject.Find("AccountManager");
        int playerChips = acct.GetComponent<AccountManager>().CurrentChips;
        bool purcCheck = false;
        switch (purchaseNum)
        {
            case 1:
                if (playerChips >= currentCost)
                {
                    PlayerPrefs.SetInt("Berry", 1); // purch unlocked
                    purcCheck = true;
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
            case 2:
                if (playerChips >= currentCost)
                {
                    PlayerPrefs.SetInt("Chocolate", 1); // purch unlocked
                    purcCheck = true;
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
            case 3:
                if (playerChips >= currentCost)
                {
                    PlayerPrefs.SetInt("Pink", 1); // purch unlocked
                    purcCheck = true;
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
            case 4:
                if (playerChips >= currentCost)
                {
                    PlayerPrefs.SetInt("Beige", 1); // purch unlocked
                    purcCheck = true;
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
            case 5:
                if (playerChips >= currentCost)
                {
                    PlayerPrefs.SetInt("Ice", 1); // purch unlocked
                    purcCheck = true;
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
            case 6:
                if (playerChips >= currentCost)
                {
                    PlayerPrefs.SetInt("MidnightBlack", 1); // purch unlocked
                    purcCheck = true;
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
            case 7:
                if (playerChips >= currentCost)
                {
                    //PlayerPrefs.SetInt("MidnightBlack", 1); // purch unlocked
                    purcCheck = true;
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
            case 8:
                if (playerChips >= currentCost)
                {
                    //PlayerPrefs.SetInt("MidnightBlack", 1); // purch unlocked
                    purcCheck = true;
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
            case 9:
                if (playerChips >= currentCost)
                {
                    //PlayerPrefs.SetInt("MidnightBlack", 1); // purch unlocked
                    purcCheck = true;
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
            case 10:
                if (playerChips >= currentCost)
                {
                    //PlayerPrefs.SetInt("MidnightBlack", 1); // purch unlocked
                    purcCheck = true;
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
            case 11:
                if (playerChips >= currentCost)
                {
                    //PlayerPrefs.SetInt("MidnightBlack", 1); // purch unlocked
                    purcCheck = true;
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
        }
        if (purcCheck)
        {
            purchaseMenu.SetActive(false);
            kart.SetActive(false);
            rotateKart = false;
            refreshUnlocks = true;
            invalid.enabled = false;
            PlayerPrefs.Save();
            source.PlayOneShot(cash);
            colorPicked = false;
            kartPicked = false;
        }
        else
        {
            invalid.enabled = true;
        }
    }

    public void PurchaseCancelled()
    {
        purchaseMenu.SetActive(false);
        kart.SetActive(false);
        rotateKart = false;
        invalid.enabled = false;
    }

    public void ResetEverything()
    {
        // Debug way to reset the cash shop items
        PlayerPrefs.SetInt("chips", 0);
        PlayerPrefs.SetInt("Berry", 0);
        PlayerPrefs.SetInt("Chocolate", 0);
        PlayerPrefs.SetInt("Pink", 0);
        PlayerPrefs.SetInt("Beige", 0);
        PlayerPrefs.SetInt("Ice", 0);
        PlayerPrefs.SetInt("MidnightBlack", 0);
        PlayerPrefs.Save();
        refreshUnlocks = true;
    }

    private IEnumerator LeaveScene()
    {
        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}
