using UnityEngine;
using System.Collections.Generic;

public class AccountManager : MonoBehaviour {
    
    private int chips;
    public int CurrentChips { get { return chips; } set { chips = value; } }

    private Dictionary<string, int> colorUnlocks = new Dictionary<string, int>();
    public Dictionary<string, int> GetColorUnlocks { get { return colorUnlocks; } }

    private Dictionary<string, int> carUnlocks = new Dictionary<string, int>();
    public Dictionary<string, int> GetCarUnlocks { get { return carUnlocks; } }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    void Start () {
        if (PlayerPrefs.HasKey("chips")){
            // load account info
            chips = PlayerPrefs.GetInt("chips");
            LoadUnlocks();
        }
        else
        {
            // new account, set PlayerPref
            chips = 0;
            PlayerPrefs.SetInt("chips", chips);

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
            PlayerPrefs.SetInt("IceCream", 0);
            PlayerPrefs.SetInt("Taxi", 0);
            carUnlocks.Add("CityCar", 0);
            carUnlocks.Add("IceCream", 0);
            carUnlocks.Add("Taxi", 0);
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
        carUnlocks.Add("IceCream", PlayerPrefs.GetInt("IceCream"));
        carUnlocks.Add("Taxi", PlayerPrefs.GetInt("Taxi"));
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
        carUnlocks.Add("IceCream", PlayerPrefs.GetInt("IceCream"));
        carUnlocks.Add("Taxi", PlayerPrefs.GetInt("Taxi"));

        chips = PlayerPrefs.GetInt("chips");
    }

    public void DeductChips(int cost)
    {
        chips -= cost;
        if (chips < 0)
            chips = 0;
        PlayerPrefs.SetInt("chips", chips);
        PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("chips", chips); // save chips
        PlayerPrefs.Save();
    }

}
