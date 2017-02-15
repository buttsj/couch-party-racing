using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class MusicManager : MonoBehaviour
{

    // source: http://answers.unity3d.com/questions/652919/music-player-get-songs-from-directory.html

    public enum SeekDirection { Forward, Backward }

    public AudioSource source;
    public List<AudioClip> clips = new List<AudioClip>();
    public Text playing;

    [SerializeField]
    [HideInInspector]
    private int currentIndex = 0;

    private FileInfo[] soundFiles;
    private List<string> validExtensions = new List<string> { ".ogg", ".wav" }; // Don't forget the "." i.e. "ogg" won't work - cause Path.GetExtension(filePath) will return .ext, not just ext.
    private string absolutePath = "./"; // relative path to where the app is running - change this to "./music" in your case

    void Start()
    {
        //being able to test in unity
        if (Application.isEditor) absolutePath = "Assets/Resources/Sounds/";

        if (source == null) source = gameObject.AddComponent<AudioSource>();

        ReloadSounds();
    }

    /*void OnGUI()
    {
        if (GUILayout.Button("Back"))
        {
            Seek(SeekDirection.Backward);
            PlayCurrent();
        }
        if (GUILayout.Button("Play"))
        {
            PlayCurrent();
        }
        if (GUILayout.Button("Forward"))
        {
            Seek(SeekDirection.Forward);
            PlayCurrent();
        }
        if (GUILayout.Button("Reload"))
        {
            ReloadSounds();
        }
    }*/

    public void Forward()
    {
        Seek(SeekDirection.Forward);
        PlayCurrent();
    }

    public void Backward()
    {
        Seek(SeekDirection.Backward);
        PlayCurrent();
    }
    
    void Seek(SeekDirection d)
    {
        if (d == SeekDirection.Forward)
            currentIndex = (currentIndex + 1) % clips.Count;
        else
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = clips.Count - 1;
        }
    }

    public void PlayCurrent()
    {
        source.clip = clips[currentIndex];
        source.Play();
    }

    public void ReloadSounds()
    {
        clips.Clear();
        // get all valid files
        var info = new DirectoryInfo(absolutePath);
        soundFiles = info.GetFiles()
            .Where(f => IsValidFileType(f.Name))
            .ToArray();

        // and load them
        foreach (var s in soundFiles)
            StartCoroutine(LoadFile(s.FullName));
    }

    bool IsValidFileType(string fileName)
    {
        return validExtensions.Contains(Path.GetExtension(fileName));
        // Alternatively, you could go fileName.SubString(fileName.LastIndexOf('.') + 1); that way you don't need the '.' when you add your extensions
    }

    IEnumerator LoadFile(string path)
    {
        WWW www = new WWW("file://" + path);
        print("loading " + path);

        AudioClip clip = www.GetAudioClip(false);
        /*while (!clip.isReadyToPlay)
            yield return www;*/
            
        while (clip.loadState == AudioDataLoadState.Loading || clip.loadState == AudioDataLoadState.Unloaded || clip.loadState == AudioDataLoadState.Failed)
            yield return www;

        print("done loading");
        clip.name = Path.GetFileName(path);
        clips.Add(clip);
    }
	
	// Update is called once per frame
	void Update () {
        if (clips.Count > 0)
            playing.text = clips[currentIndex].name;
	}

    void FixedUpdate ()
    {
        if (clips.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Backward();
            if (Input.GetKeyDown(KeyCode.RightArrow))
                Forward();
        }
    }
}
