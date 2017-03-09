using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChipShopMenuFunctionality : MonoBehaviour {

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
		
	}
	
	// Update is called once per frame
	void Update () {
        if (SimpleInput.GetButtonDown("Cancel"))
        {
            StartCoroutine(LeaveScene());
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
