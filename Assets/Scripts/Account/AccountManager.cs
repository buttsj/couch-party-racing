using UnityEngine;
using System.Collections.Generic;

public class AccountManager : MonoBehaviour {

    private static bool isAlreadyInitialized = false;
    
    public int CurrentChips { get { return PlayerPrefs.GetInt("chips", 0); ; } set { PlayerPrefs.SetInt("chips", value); PlayerPrefs.Save(); } }

    private Dictionary<string, int> colorUnlocks = new Dictionary<string, int>();
    public Dictionary<string, int> GetColorUnlocks { get { return colorUnlocks; } }

    private Dictionary<string, int> carUnlocks = new Dictionary<string, int>();
    public Dictionary<string, int> GetCarUnlocks { get { return carUnlocks; } }

    void Awake()
    {
        if(isAlreadyInitialized) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }
    
    void Start () {
        isAlreadyInitialized = true;

        if (PlayerPrefs.HasKey("chips")){
            // load account info
            LoadUnlocks();
        }
        else
        {
            // new account, set PlayerPref
            PlayerPrefs.SetInt("chips", 0);

            PlayerPrefs.SetInt("Berry", 0);
            PlayerPrefs.SetInt("Chocolate", 0);
            PlayerPrefs.SetInt("Pink", 0);
            PlayerPrefs.SetInt("Beige", 0);
            PlayerPrefs.SetInt("Ice", 0);
            PlayerPrefs.SetInt("MidnightBlack", 0);
            colorUnlocks.Add("Berry", 0);
            colorUnlocks.Add("Chocolate", 0);
            colorUnlocks.Add("Pink", 0);
            colorUnlocks.Add("Beige", 0);
            colorUnlocks.Add("Ice", 0);
            colorUnlocks.Add("MidnightBlack", 0);

            PlayerPrefs.SetInt("CityCar", 0);
            PlayerPrefs.SetInt("Hearse", 0);
            PlayerPrefs.SetInt("Taxi", 0);
            PlayerPrefs.SetInt("Muscle", 0);
            carUnlocks.Add("CityCar", 0);
            carUnlocks.Add("Hearse", 0);
            carUnlocks.Add("Taxi", 0);
            carUnlocks.Add("Muscle", 0);
        }
	}

    void LoadUnlocks()
    {
        colorUnlocks.Add("Berry", PlayerPrefs.GetInt("Berry"));
        colorUnlocks.Add("Chocolate", PlayerPrefs.GetInt("Chocolate"));
        colorUnlocks.Add("Pink", PlayerPrefs.GetInt("Pink"));
        colorUnlocks.Add("Beige", PlayerPrefs.GetInt("Beige"));
        colorUnlocks.Add("Ice", PlayerPrefs.GetInt("Ice"));
        colorUnlocks.Add("MidnightBlack", PlayerPrefs.GetInt("MidnightBlack"));

        carUnlocks.Add("CityCar", PlayerPrefs.GetInt("CityCar"));
        carUnlocks.Add("Hearse", PlayerPrefs.GetInt("Hearse"));
        carUnlocks.Add("Taxi", PlayerPrefs.GetInt("Taxi"));
        carUnlocks.Add("Muscle", PlayerPrefs.GetInt("Muscle"));
    }

    public void RefreshUnlocks()
    {
        colorUnlocks.Clear();
        colorUnlocks.Add("Berry", PlayerPrefs.GetInt("Berry"));
        colorUnlocks.Add("Chocolate", PlayerPrefs.GetInt("Chocolate"));
        colorUnlocks.Add("Pink", PlayerPrefs.GetInt("Pink"));
        colorUnlocks.Add("Beige", PlayerPrefs.GetInt("Beige"));
        colorUnlocks.Add("Ice", PlayerPrefs.GetInt("Ice"));
        colorUnlocks.Add("MidnightBlack", PlayerPrefs.GetInt("MidnightBlack"));

        carUnlocks.Clear();
        carUnlocks.Add("CityCar", PlayerPrefs.GetInt("CityCar"));
        carUnlocks.Add("Hearse", PlayerPrefs.GetInt("Hearse"));
        carUnlocks.Add("Taxi", PlayerPrefs.GetInt("Taxi"));
        carUnlocks.Add("Muscle", PlayerPrefs.GetInt("Muscle"));

    }

    public void DeductChips(int cost)
    {
        int chips = PlayerPrefs.GetInt("chips", 0);
        chips -= cost;
        if (chips < 0)
            chips = 0;
        PlayerPrefs.SetInt("chips", chips);
        PlayerPrefs.Save();
    }

    public bool unlockStatus(string storeItem)
    {
        bool unlockStatus = false;
        if(PlayerPrefs.GetInt(storeItem, 0) > 0)
        {
            unlockStatus = true;
        }

        return unlockStatus;
    }

    public bool purchaseItem(int cost, string item)
    {
        int chips = PlayerPrefs.GetInt("chips", 0);
        bool validPurchase = (chips >= cost);
        if (validPurchase)
        {
            chips -= cost;
            PlayerPrefs.SetInt(item, 1);
            if (chips < 0)
                chips = 0;
            PlayerPrefs.SetInt("chips", chips);
            PlayerPrefs.Save();
        }

        return validPurchase;
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
    }

}
