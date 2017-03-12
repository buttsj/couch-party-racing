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
    private WhiteFadeUniversal fader;

    private string currentColor;
    private int purchaseNum;
    private Dictionary<string, int> unlocks;
    private bool refreshUnlocks;

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
        switch (choice)
        {
            case 1:
                currentColor = "Berry";
                break;
            case 2:
                currentColor = "Chocolate";
                break;
            case 3:
                currentColor = "Pink";
                break;
            case 4:
                currentColor = "Beige";
                break;
            case 5:
                currentColor = "Ice";
                break;
            case 6:
                currentColor = "MidnightBlack";
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
        GameObject btn = GameObject.Find(currentColor);
        kart.GetComponentInChildren<Renderer>().material.color = btn.GetComponent<Image>().color;
    }

    public void PurchaseMade()
    {
        purchaseMenu.SetActive(false);
        kart.SetActive(false);
        switch (purchaseNum)
        {
            case 1:
                PlayerPrefs.SetInt("Berry", 1);
                break;
            case 2:
                PlayerPrefs.SetInt("Chocolate", 1);
                break;
            case 3:
                PlayerPrefs.SetInt("Pink", 1);
                break;
            case 4:
                PlayerPrefs.SetInt("Beige", 1);
                break;
            case 5:
                PlayerPrefs.SetInt("Ice", 1);
                break;
            case 6:
                PlayerPrefs.SetInt("MidnightBlack", 1);
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
        refreshUnlocks = true;
        PlayerPrefs.Save();
    }

    public void PurchaseCancelled()
    {
        purchaseMenu.SetActive(false);
        kart.SetActive(false);
    }

    private IEnumerator LeaveScene()
    {
        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;
        SceneManager.LoadScene(0);
    }
}
