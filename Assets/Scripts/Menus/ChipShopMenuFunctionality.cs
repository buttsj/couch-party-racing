using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ChipShopMenuFunctionality : MonoBehaviour {

    public Text chips;
    public GameObject moreInfo;
    public GameObject purchaseMenu;
    public GameObject kart;
    private WhiteFadeUniversal fader;

    private string currentColor;

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
        chips.text = GameObject.Find("AccountManager").GetComponent<AccountManager>().CurrentChips.ToString();
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
    }

    public void MoreInfoButton()
    {
        Debug.Log("checking more info");
        moreInfo.SetActive(true);
    }

    public void ShowPurchase(int choice)
    {
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
                currentColor = "Midnight Black";
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
