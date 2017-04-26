using UnityEngine.UI;
using UnityEngine;

public class CreditsMenuFunctionality : MonoBehaviour {

    public Text creditsText;
    public Text developersText;
    public Image logo;

    public float timer = 0.0f;
	
    void Start ()
    {
        timer = 0.0f;
        gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
	    if (timer < 35.0f)
        {
            creditsText.gameObject.transform.Translate(Vector3.up * Time.deltaTime * 50.0f);
            developersText.gameObject.transform.Translate(Vector3.up * Time.deltaTime * 50.0f);
            logo.gameObject.transform.Translate(Vector3.up * Time.deltaTime * 50.0f);
        }
        else
        {
            creditsText.gameObject.transform.localPosition = new Vector3(0, -250, 0);
            developersText.gameObject.transform.localPosition = new Vector3(0, -450, 0);
            logo.gameObject.transform.localPosition = new Vector3(0, -700, 0);
            timer = 0.0f;
            gameObject.SetActive(false);
        }
        timer = timer + Time.deltaTime;
	}
}
