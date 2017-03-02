using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class WhiteFadeUniversal : MonoBehaviour {
    
    private const string TRANSITION = "Sounds/KartEffects/screen_transition";
    private AudioClip transition;
    private AudioSource source;

    public Image img;
    private float alpha;
    private float fadeSpeed = 1f;
    private bool FadeInBool;
    private bool FadeOutBool;

    private bool played;
    public bool Faded { get { return played; } }

    void Start() {
        transform.position = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(1000, 1000, 1000);
    }
    
	void Update() {
        if (FadeInBool)
            AddFadeIn();
        if (FadeOutBool)
            AddFadeOut();
	}

    public void BeginNewScene()
    {
        played = false;
        transition = (AudioClip)Resources.Load(TRANSITION);
        source = GameObject.Find("Sound").GetComponent<AudioSource>();
        transform.SetAsFirstSibling();
        img = gameObject.AddComponent<Image>();
        Color fadeCol = img.color;
        fadeCol.a = 255;
        img.color = fadeCol;
        alpha = 1.0f;
        FadeInBool = true;
        FadeOutBool = false;
        transform.SetAsLastSibling();
    }

    public void SceneSwitch()
    {
        Color fadeCol = img.color;
        fadeCol.a = 0;
        img.color = fadeCol;
        alpha = 0.0f;
        FadeInBool = false;
        FadeOutBool = true;
        source.PlayOneShot(transition);
    }

    private void AddFadeIn()
    {
        alpha -= fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        Color col = img.color;
        col.a = alpha;
        img.color = col;
        if (alpha == 0)
        {
            FadeInBool = false;
        }
    }

    private void AddFadeOut()
    {
        alpha += fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        Color col = img.color;
        col.a = alpha;
        img.color = col;
        if (alpha == 1)
        {
            FadeOutBool = false;
            played = true;
        }
    }
}
