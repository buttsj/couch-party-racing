using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChipShopMenuFunctionality : MonoBehaviour {

    public Text chips;
    public Text invalid;
    public Text costText;
    public GameObject moreInfo;
    public GameObject purchaseMenu;

    public Camera shopcam;
    public GameObject kart;
    public GameObject streetcar;
    public GameObject taxi;
    public GameObject hearse;
    public GameObject rare;
    public GameObject fan;

    public GameObject colorHolder;
    public GameObject carHolder;
    private WhiteFadeUniversal fader;
    private AudioSource source;
    private AudioClip cash;
    private const string REGISTER = "Sounds/KartEffects/cash";
    private const string COST = " CHIPS";
    
    private string currentColor;
    private int currentCost;
    private int purchaseNum;

    private Dictionary<string, int> colorUnlocks;
    private Dictionary<string, int> carUnlocks;
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
            streetcar.transform.Rotate(Vector3.up, 10.0f * Time.deltaTime);
            hearse.transform.Rotate(Vector3.up, 10.0f * Time.deltaTime);
            taxi.transform.Rotate(Vector3.up, 10.0f * Time.deltaTime);
            rare.transform.Rotate(Vector3.up, 10.0f * Time.deltaTime);
        }
        fan.transform.Rotate(Vector3.up, 5.0f);
    }

    private void LoadUnlocks()
    {
        GameObject acct = GameObject.Find("AccountManager");
        acct.GetComponent<AccountManager>().RefreshUnlocks();
        colorUnlocks = acct.GetComponent<AccountManager>().GetColorUnlocks;
        foreach (KeyValuePair<string, int> pair in colorUnlocks)
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

        carUnlocks = acct.GetComponent<AccountManager>().GetCarUnlocks;
        foreach (KeyValuePair<string, int> pair in carUnlocks)
        {
            if (pair.Value == 0)
            {
                print(pair.Key);
                carHolder.transform.FindChild(pair.Key).gameObject.SetActive(true);
            }
            else if (pair.Value == 1)
            {
                carHolder.transform.FindChild(pair.Key).gameObject.SetActive(false);
            }
        }
    }

    public void MoreInfoButton()
    {
        moreInfo.SetActive(true);
    }

    public void ShowPurchase(int choice)
    {
        shopcam.GetComponent<ChipShopCamera>().FocusIn();
        purchaseNum = choice;
        purchaseMenu.SetActive(true);
        rotateKart = true;
        invalid.enabled = false;
        colorPicked = false;
        kartPicked = false;
        kart.SetActive(false);
        streetcar.SetActive(false);
        hearse.SetActive(false);
        rare.SetActive(false);
        taxi.SetActive(false);
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
                streetcar.SetActive(true);
                currentCost = 30;
                kartPicked = true;
                break;
            case 8:
                hearse.SetActive(true);
                currentCost = 30;
                kartPicked = true;
                break;
            case 9:
                taxi.SetActive(true);
                currentCost = 30;
                kartPicked = true;
                break;
            case 10:
                rare.SetActive(true);
                currentCost = 50;
                kartPicked = true;
                break;
        }
        if (colorPicked)
        {
            kart.SetActive(true);
            GameObject btn = GameObject.Find(currentColor);
            kart.GetComponentInChildren<Renderer>().material.color = btn.GetComponent<Image>().color;
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
                    PlayerPrefs.SetInt("CityCar", 1); // purch unlocked
                    purcCheck = true;
                    streetcar.SetActive(false);
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
            case 8:
                if (playerChips >= currentCost)
                {
                    PlayerPrefs.SetInt("Hearse", 1); // purch unlocked
                    purcCheck = true;
                    hearse.SetActive(false);
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
            case 9:
                if (playerChips >= currentCost)
                {
                    PlayerPrefs.SetInt("Taxi", 1); // purch unlocked
                    purcCheck = true;
                    taxi.SetActive(false);
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
            case 10:
                if (playerChips >= currentCost)
                {
                    PlayerPrefs.SetInt("RareKart", 1); // purch unlocked
                    purcCheck = true;
                    rare.SetActive(false);
                    acct.GetComponent<AccountManager>().DeductChips(currentCost);
                }
                break;
        }
        if (purcCheck)
        {
            shopcam.GetComponent<ChipShopCamera>().FocusOut();
            purchaseMenu.SetActive(false);
            kart.SetActive(false);
            streetcar.SetActive(false);
            hearse.SetActive(false);
            rare.SetActive(false);
            taxi.SetActive(false);
            rotateKart = false;
            refreshUnlocks = true;
            invalid.enabled = false;
            colorPicked = false;
            kartPicked = false;
            PlayerPrefs.Save();
            source.PlayOneShot(cash);
        }
        else
        {
            invalid.enabled = true;
        }
    }

    public void PurchaseCancelled()
    {
        shopcam.GetComponent<ChipShopCamera>().FocusOut();
        purchaseMenu.SetActive(false);
        kart.SetActive(false);
        streetcar.SetActive(false);
        hearse.SetActive(false);
        rare.SetActive(false);
        taxi.SetActive(false);
        rotateKart = false;
        invalid.enabled = false;
        colorPicked = false;
        kartPicked = false;
    }

    public void ResetEverything()
    {
        // Debug way to reset the cash shop items
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("chips", 0);
        PlayerPrefs.SetInt("Berry", 0);
        PlayerPrefs.SetInt("Chocolate", 0);
        PlayerPrefs.SetInt("Pink", 0);
        PlayerPrefs.SetInt("Beige", 0);
        PlayerPrefs.SetInt("Ice", 0);
        PlayerPrefs.SetInt("MidnightBlack", 0);
        PlayerPrefs.SetInt("CityCar", 0);
        PlayerPrefs.SetInt("Hearse", 0);
        PlayerPrefs.SetInt("RareKart", 0);
        PlayerPrefs.SetInt("Taxi", 0);
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
