using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ChipShopMenuFunctionality : MonoBehaviour {

    public Text chips;
    private WhiteFadeUniversal fader;

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

    private IEnumerator LeaveScene()
    {
        fader.SceneSwitch();
        while (!fader.Faded)
            yield return null;
        SceneManager.LoadScene(0);
    }
}
