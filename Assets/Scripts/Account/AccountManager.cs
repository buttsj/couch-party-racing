using UnityEngine;

public class AccountManager : MonoBehaviour {
    
    private int chips;
    public int CurrentChips { get { return chips; } set { chips = value; } }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start () {
        // load account info
        if (PlayerPrefs.HasKey("chips")){
            chips = PlayerPrefs.GetInt("chips");
        }
        else
        {
            // new account, set PlayerPref
            chips = 0;
            PlayerPrefs.SetInt("chips", chips);
        }
        Debug.Log("Current chips: " + chips);
	}

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("chips", chips); // save chips
    }

}
