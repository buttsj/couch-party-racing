using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Cursor : MonoBehaviour {

    private const string TRACK_DIR = "Prefabs/TrackPrefabs/";
    private const string CHILD_NAME = "Track Piece";
    private const int TRACK_SCALE = 60;

    public GameObject trackParent;
    private List<GameObject> trackList;
    private int trackIndex = 0;
    private bool canAcceptInput = false;

	// Use this for initialization
	void Start () {
        trackList = new List<GameObject>(Resources.LoadAll<GameObject>(TRACK_DIR));

        SwapCurrentTrack();
        PrintTrackName();
	}
	
	// Update is called once per frame
	void Update () {
        HandleInput();
    }

    private void HandleInput() {
        if (canAcceptInput) {
            // X-Axis
            if (SimpleInput.GetAxis("Horizontal") > 0) {
                transform.position += new Vector3(TRACK_SCALE, 0, 0);
            }
            if (SimpleInput.GetAxis("Horizontal") < 0) {
                transform.position -= new Vector3(TRACK_SCALE, 0, 0);
            }

            // Y-Axis
            if (SimpleInput.GetAxis("Vertical") > 0) {
                transform.position += new Vector3(0, TRACK_SCALE, 0);
            }
            if (SimpleInput.GetAxis("Vertical") < 0) {
                transform.position -= new Vector3(0, TRACK_SCALE, 0);
            }   
        }

        // Z-Axis
        if (SimpleInput.GetButtonDown("Accelerate")) {
            transform.position += new Vector3(0, 0, TRACK_SCALE);
        }
        if (SimpleInput.GetButtonDown("Reverse")) {
            transform.position -= new Vector3(0, 0, TRACK_SCALE);
        }

        // Rotate
        if (SimpleInput.GetButtonDown("Rotate")) {
            transform.Rotate(new Vector3(0, 90, 0));
        }

        // Spawn Track
        if (SimpleInput.GetButtonDown("Bump Kart")) {
            SpawnTrack();
        }

        // Switch Track
        if (SimpleInput.GetButtonDown("Next Track")) {
            trackIndex = (++trackIndex) % trackList.Count;
            SwapCurrentTrack();
            PrintTrackName();
        }
        if (SimpleInput.GetButtonDown("Previous Track")) {
            trackIndex = (--trackIndex + trackList.Count) % trackList.Count;
            SwapCurrentTrack();
            PrintTrackName();
        }

        canAcceptInput = IsInputReset();
    }

    private void SwapCurrentTrack() {
        // Remove Previous Track
        var child = transform.FindChild(CHILD_NAME);
        if (child) {
            Destroy(child.gameObject);
        }

        Instantiate(trackList[trackIndex], transform.position, transform.rotation, transform).name = CHILD_NAME;
    }

    private void PrintTrackName() {
        Debug.Log(trackList[trackIndex].name);
    }

    private void SpawnTrack() {
        Instantiate(trackList[trackIndex], transform.position, transform.rotation, trackParent.transform).name = trackList[trackIndex].name;
    }

    private bool IsInputReset() {
        return SimpleInput.GetAxis("Horizontal") == 0 && SimpleInput.GetAxis("Vertical") == 0;
    }
}
