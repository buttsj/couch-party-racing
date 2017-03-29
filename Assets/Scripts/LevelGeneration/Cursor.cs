using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

    public GameObject trackParent;

    private const string TRACK_DIR = "Prefabs/TrackPrefabs/";
    private const string START_TRACK_NAME = "StartTrack";

    private static readonly List<string> CANT_SPAWN_TRACKS = new List<string>() { "CrossTrack", "Minimap"};

    private TrackToGridSpawner trackToGrid;
    private GameObject cursorTrackPiece;
    private List<GameObject> trackList;
    private int trackIndex = 0;

	// Use this for initialization
	void Start () {
        trackList = new List<GameObject>(Resources.LoadAll<GameObject>(TRACK_DIR));

        trackToGrid = new TrackToGridSpawner(trackParent);

        SetToTrackPiece(START_TRACK_NAME);
        SwapCurrentTrack();
        PrintTrackName();
	}
	
	// Update is called once per frame
	void Update () {
        // Rotate Track
        if (SimpleInput.GetButtonDown("Rotate", 1)) {
            transform.Rotate(new Vector3(0, 90, 0));
        }

        // Spawn Track
        if (SimpleInput.GetButtonDown("Bump Kart", 1)) {
            trackToGrid.SpawnTrack(cursorTrackPiece);
        }

        // Delete Track
        if (SimpleInput.GetButtonDown("Delete Track", 1)) {
            trackToGrid.DeleteTrack(cursorTrackPiece, trackList);
        }

        // Switch Track
        if (SimpleInput.GetButtonDown("Next Track", 1)) {
            NextTrackPiece();
            SwapCurrentTrack();
            PrintTrackName();
        }
        if (SimpleInput.GetButtonDown("Previous Track", 1)) {
            PreviousTrackPiece();
            SwapCurrentTrack();
            PrintTrackName();
        }
    }

    private void SwapCurrentTrack() {
        Destroy(cursorTrackPiece);
        cursorTrackPiece = Instantiate(trackList[trackIndex], transform.position, transform.rotation, transform);
        cursorTrackPiece.name = trackList[trackIndex].name;

        trackToGrid.DisableColliders(cursorTrackPiece);
    }

    private void PrintTrackName() {
        Debug.Log(trackList[trackIndex].name);
    }

    private void NextTrackPiece() {
        do {
            trackIndex = (++trackIndex) % trackList.Count;
        } while (CANT_SPAWN_TRACKS.Find(x => trackList[trackIndex].name.Contains(x)) != null);
    }

    private void PreviousTrackPiece() {
        do {
            trackIndex = (--trackIndex + trackList.Count) % trackList.Count;
        } while (CANT_SPAWN_TRACKS.Find(x => trackList[trackIndex].name.Contains(x)) != null);
    }

    private void SetToTrackPiece(string trackPieceName) {
        while(trackPieceName != trackList[trackIndex].name) {
            NextTrackPiece();
        }
    }
}
