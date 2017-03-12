using UnityEngine;
using System.Collections.Generic;

public class AccountManager : MonoBehaviour {
    
    private int chips;
    public int CurrentChips { get { return chips; } set { chips = value; } }

    private Dictionary<string, int> unlocks = new Dictionary<string, int>();
    public Dictionary<string, int> GetUnlocks { get { return unlocks; } }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start () {
        // load account info
        if (PlayerPrefs.HasKey("chips")){
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
            unlocks.Add("Berry", 0);
            unlocks.Add("Chocolate", 0);
            unlocks.Add("Pink", 0);
            unlocks.Add("Beige", 0);
            unlocks.Add("Ice", 0);
            unlocks.Add("MidnightBlack", 0);
        }
	}

    void Update()
    {
        if (SimpleInput.GetButtonDown("Cancel"))
        {
            PlayerPrefs.SetInt("chips", 0);
            PlayerPrefs.SetInt("Berry", 0);
            PlayerPrefs.SetInt("Chocolate", 0);
            PlayerPrefs.SetInt("Pink", 0);
            PlayerPrefs.SetInt("Beige", 0);
            PlayerPrefs.SetInt("Ice", 0);
            PlayerPrefs.SetInt("MidnightBlack", 0);
            PlayerPrefs.Save();
        }
    }

    void LoadUnlocks()
    {
        unlocks.Add("Berry", PlayerPrefs.GetInt("Berry"));
        unlocks.Add("Chocolate", PlayerPrefs.GetInt("Chocolate"));
        unlocks.Add("Pink", PlayerPrefs.GetInt("Pink"));
        unlocks.Add("Beige", PlayerPrefs.GetInt("Beige"));
        unlocks.Add("Ice", PlayerPrefs.GetInt("Ice"));
        unlocks.Add("MidnightBlack", PlayerPrefs.GetInt("MidnightBlack"));
    }

    public void RefreshUnlocks()
    {
        unlocks.Clear();
        unlocks.Add("Berry", PlayerPrefs.GetInt("Berry"));
        unlocks.Add("Chocolate", PlayerPrefs.GetInt("Chocolate"));
        unlocks.Add("Pink", PlayerPrefs.GetInt("Pink"));
        unlocks.Add("Beige", PlayerPrefs.GetInt("Beige"));
        unlocks.Add("Ice", PlayerPrefs.GetInt("Ice"));
        unlocks.Add("MidnightBlack", PlayerPrefs.GetInt("MidnightBlack"));
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("chips", chips); // save chips
    }

}
