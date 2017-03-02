using UnityEngine.UI;
using UnityEngine;

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

    // called when a scene is using a Fade In
    public void BeginNewScene(string musicPlayer)
    {
        played = false;
        transition = (AudioClip)Resources.Load(TRANSITION);
        source = GameObject.Find(musicPlayer).GetComponent<AudioSource>();
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

    // called when a scene is only using a Fade Out
    public void BeginExitScene(string musicPlayer)
    {
        played = false;
        transition = (AudioClip)Resources.Load(TRANSITION);
        source = GameObject.Find(musicPlayer).GetComponent<AudioSource>();
        transform.SetAsFirstSibling();
        img = gameObject.AddComponent<Image>();
        Color fadeCol = img.color;
        fadeCol.a = 0;
        img.color = fadeCol;
        alpha = 0.0f;
        FadeInBool = false;
        FadeOutBool = false;
        transform.SetAsLastSibling();
    }

    // begin Fade Out sequence
    public void SceneSwitch()
    {
        FadeInBool = false;
        FadeOutBool = true;
        source.PlayOneShot(transition);
    }

    // increment the Fade In (via deltaTime)
    private void AddFadeIn()
    {
        if (Time.timeScale == 0)
            alpha -= fadeSpeed * .02f;
        else if (Time.timeScale == 1)
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

    // increment the Fade Out (via deltaTime)
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
