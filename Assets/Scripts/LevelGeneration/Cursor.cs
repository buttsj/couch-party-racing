using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour {

    public GameObject trackParent;
    public Text trackText;

    private const string TRACK_UI_DIR = "Prefabs/UI Prefabs/TrackBuilder UI/";
    private const string TRACK_DIR = "Prefabs/TrackPrefabs/";
    private const string START_TRACK_NAME = "StartTrack";
    private const string EMPTY_TRACK_NAME = "EmptyTrack";
    private const string DEATH_TRACK_NAME = "DeathCubePrefab";

    private static readonly List<string> CANT_SPAWN_TRACKS = new List<string>() { "crosstrack", "minimap"};
    private static readonly List<string> UNIQUE_TRACKS = new List<string>() { "starttrack"};

    private TrackToGridSpawner trackToGrid;
    private GameObject cursorTrackPiece;
    private List<GameObject> trackList;
    private int trackIndex = 0;

    private PauseMenuFunctionality pauseMenu;

	void Start () {
        trackList = new List<GameObject>(Resources.LoadAll<GameObject>(TRACK_DIR));

        trackToGrid = new TrackToGridSpawner(trackParent);

        SetToTrackPiece(START_TRACK_NAME);

        pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenuFunctionality>();
	}
	
	void Update () {

        if(!pauseMenu.isPaused())
        {
            // Rotate Track
            if (SimpleInput.GetButtonDown("Rotate", 1))
            {
                transform.Rotate(new Vector3(0, 90, 0));
            }

            // Spawn Track
            if (SimpleInput.GetButtonDown("Bump Kart", 1))
            {
                if (!UniquePieceAlreadyExists()) {
                    trackToGrid.SpawnTrack(cursorTrackPiece);
                }

                if (IsUniquePiece(cursorTrackPiece.name))
                {
                    SwapToNextValidTrack();
                }
            }

            // Delete Track
            if (SimpleInput.GetButtonDown("Delete Track", 1))
            {
                trackToGrid.DeleteTrack(cursorTrackPiece, trackList);
            }

            // Switch Track
            if (SimpleInput.GetButtonDown("Next Track", 1))
            {
                SwapToNextValidTrack();
            }
            if (SimpleInput.GetButtonDown("Previous Track", 1))
            {
                SwapToPreviousValidTrack();
            }
        }
    }

    private void SwapToNextValidTrack() {
        do {
            NextTrackPiece();
        } while (UniquePieceAlreadyExists());

        if (!DoesStartTrackExist()) {
            SetToTrackPiece(START_TRACK_NAME);
        } else {
            SwapCurrentTrack();
            PrintTrackName();
        }
    }

    private void SwapToPreviousValidTrack() {
        do {
            PreviousTrackPiece();
        } while (UniquePieceAlreadyExists());

        if (!DoesStartTrackExist()) {
            SetToTrackPiece(START_TRACK_NAME);
        } else {
            SwapCurrentTrack();
            PrintTrackName();
        }
    }

    private void SwapCurrentTrack() {
        Destroy(cursorTrackPiece);
        cursorTrackPiece = Instantiate(trackList[trackIndex], transform.position, transform.rotation, transform);
        cursorTrackPiece.name = trackList[trackIndex].name;

        // Add UI to spawnedTrackPiece
        if (cursorTrackPiece.name == EMPTY_TRACK_NAME) {
            Instantiate(Resources.Load<GameObject>(TRACK_UI_DIR + EMPTY_TRACK_NAME), cursorTrackPiece.transform, false);
        } else if (cursorTrackPiece.name == DEATH_TRACK_NAME) {
            Instantiate(Resources.Load<GameObject>(TRACK_UI_DIR + DEATH_TRACK_NAME), cursorTrackPiece.transform, false);
        }

        trackToGrid.DisableColliders(cursorTrackPiece);
    }

    private void PrintTrackName() {
        var name = trackList[trackIndex].name;
        var stringCase = System.StringComparison.CurrentCultureIgnoreCase;

        if (name.EndsWith("Track", stringCase)) {
            name = name.Remove(name.IndexOf("Track", stringCase));
        } else if (name.EndsWith("Prefab", stringCase)) {
            name = name.Remove(name.IndexOf("Prefab", stringCase));
        }

        name = "<  " + name + "   >";

        trackText.text = name;
    }

    private void NextTrackPiece() {
        do {
            trackIndex = (++trackIndex) % trackList.Count;
        } while (CANT_SPAWN_TRACKS.Find(x => trackList[trackIndex].name.ToLower().Contains(x)) != null);
    }

    private void PreviousTrackPiece() {
        do {
            trackIndex = (--trackIndex + trackList.Count) % trackList.Count;
        } while (CANT_SPAWN_TRACKS.Find(x => trackList[trackIndex].name.ToLower().Contains(x)) != null);
    }

    private bool IsUniquePiece(string pieceName) {
        return UNIQUE_TRACKS.Find(x => pieceName.ToLower().Contains(x)) != null;
    }

    private bool UniquePieceAlreadyExists() {
        bool doesUniquePieceExist = false;

        if (IsUniquePiece(trackList[trackIndex].name)) {
            foreach (Transform child in trackParent.transform) {
                doesUniquePieceExist = IsUniquePiece(child.name);
                if (doesUniquePieceExist) {
                    break;
                }
            }
        }

        return doesUniquePieceExist;
    }

    private void SetToTrackPiece(string trackPieceName) {
        while(trackPieceName != trackList[trackIndex].name) {
            NextTrackPiece();
        }

        SwapCurrentTrack();
        PrintTrackName();
    }

    private bool DoesStartTrackExist() {
        bool doesStartTrackExist = false;

        foreach (Transform child in trackParent.transform) {
            doesStartTrackExist = child.name == START_TRACK_NAME;
            if (doesStartTrackExist) {
                break;
            }
        }

        return doesStartTrackExist;
    }
}
