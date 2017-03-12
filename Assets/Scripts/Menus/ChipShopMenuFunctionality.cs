using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChipShopMenuFunctionality : MonoBehaviour {

    public Text chips;
    public GameObject moreInfo;
    public GameObject purchaseMenu;
    public GameObject kart;
    public Text invalid;
    private WhiteFadeUniversal fader;

    private string currentColor;
    private int purchaseNum;
    private Dictionary<string, int> unlocks;
    private bool refreshUnlocks;
    private bool rotateKart;
    private int currentCost;

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
        GameObject acct = GameObject.Find("AccountManager");
        chips.text = acct.GetComponent<AccountManager>().CurrentChips.ToString();
        LoadUnlocks();
        refreshUnlocks = false;
        rotateKart = false;
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
            if (pair.Value == 1)
            {
                try
                {
                    if (GameObject.Find(pair.Key).activeInHierarchy) 
                        GameObject.Find(pair.Key).SetActive(false);
                }
                catch (Exception e)
                {
                    Debug.Log("could not find " + pair.Key);
                }
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
        switch (choice)
        {
            case 1:
                currentColor = "Berry";
                currentCost = 10;
                break;
            case 2:
                currentColor = "Chocolate";
                currentCost = 10;
                break;
            case 3:
                currentColor = "Pink";
                currentCost = 10;
                break;
            case 4:
                currentColor = "Beige";
                currentCost = 10;
                break;
            case 5:
                currentColor = "Ice";
                currentCost = 10;
                break;
            case 6:
                currentColor = "MidnightBlack";
                currentCost = 10;
                break;
            case 7:
                currentCost = 30;
                break;
            case 8:
                currentCost = 30;
                break;
            case 9:
                currentCost = 30;
                break;
            case 10:
                currentCost = 50;
                break;
            case 11:
                currentCost = 50;
                break;
        }
        GameObject btn = GameObject.Find(currentColor);
        kart.GetComponentInChildren<Renderer>().material.color = btn.GetComponent<Image>().color;
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
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
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

    private IEnumerator LeaveScene()
    {
        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}
