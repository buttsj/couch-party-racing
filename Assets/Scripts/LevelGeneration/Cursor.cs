using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Cursor : MonoBehaviour {

    public GameObject trackParent;

    private const string TRACK_DIR = "Prefabs/TrackPrefabs/";
    private const string CHILD_NAME = "Track Piece";
    private const int TRACK_SCALE = 60;

    private const float OFFSET = 0.001f;
    private static readonly Vector3 CENTER_OFFSET = new Vector3(-TRACK_SCALE, TRACK_SCALE, TRACK_SCALE) / 2f;
    private static readonly Vector3 BOX_HALF_SCALE = new Vector3(TRACK_SCALE - OFFSET, TRACK_SCALE - OFFSET, TRACK_SCALE - OFFSET) / 2f;

    private static readonly List<string> CANT_SPAWN_TRACKS = new List<string>() { "CrossTrack", "Minimap"};

    private GameObject childTrackPiece;
    private List<GameObject> trackList;
    private int trackIndex = 0;
    private bool canAcceptInput = false;

	// Use this for initialization
	void Start () {
        trackList = new List<GameObject>(Resources.LoadAll<GameObject>(TRACK_DIR));

        NextTrackPiece();
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
            //trackIndex = (++trackIndex) % trackList.Count;
            NextTrackPiece();
            SwapCurrentTrack();
            PrintTrackName();
        }
        if (SimpleInput.GetButtonDown("Previous Track")) {
            //trackIndex = (--trackIndex + trackList.Count) % trackList.Count;
            PreviousTrackPiece();
            SwapCurrentTrack();
            PrintTrackName();
        }

        canAcceptInput = IsInputReset();
    }

    private void SwapCurrentTrack() {
        Destroy(childTrackPiece);
        childTrackPiece = Instantiate(trackList[trackIndex], transform.position, transform.rotation, transform);

        DisableColliders(childTrackPiece);
    }

    private void PrintTrackName() {
        Debug.Log(trackList[trackIndex].name);
    }

    private void SpawnTrack() {
        if (!IsOverlap()) {
            var spawnedTrackPiece = Instantiate(trackList[trackIndex], transform.position, transform.rotation, trackParent.transform);
            spawnedTrackPiece.name = trackList[trackIndex].name;

            DisableColliders(spawnedTrackPiece);

            foreach (var center in GetCenterPoints(CENTER_OFFSET, true)) {
                spawnedTrackPiece.AddComponent<BoxCollider>().center = center;
            }

            // Set Size of Colliders
            foreach (var collider in spawnedTrackPiece.GetComponentsInChildren<BoxCollider>()) {
                if(collider.enabled) {
                    collider.size = Vector3.one * TRACK_SCALE;
                }
            }
        }
    }

    private bool IsOverlap() {
        var centerPosition = transform.position + CENTER_OFFSET;
        var collisions = GetOverlapBoxes(centerPosition, BOX_HALF_SCALE);

        foreach (var collision in collisions) {
            Debug.Log(collision.ToString());
        }

        return collisions.Count > 0;
    }

    private IList GetOverlapBoxes(Vector3 centerPosition, Vector3 halfScale) {
        List<Collider> collisions = new List<Collider>();

        foreach (var center in GetCenterPoints(centerPosition, false)) {
            collisions.AddRange(Physics.OverlapBox(center, halfScale, childTrackPiece.transform.rotation, ~0, QueryTriggerInteraction.Ignore).ToList());
        }

        return collisions;
    }

    private List<Vector3> GetCenterPoints(Vector3 centerPosition, bool isLocal) {
        List<Vector3> centerList = new List<Vector3>();
        
        centerList.Add(centerPosition);

        if (childTrackPiece.name.Contains("Ramp")) {
            centerList.Add(centerPosition + new Vector3(-TRACK_SCALE, 0, 0));
            centerList.Add(centerPosition + new Vector3(0, TRACK_SCALE, 0));
            centerList.Add(centerPosition + new Vector3(-TRACK_SCALE, TRACK_SCALE, 0));
        } else if (childTrackPiece.name.Contains("RightTurn")) {
            centerList.Add(centerPosition + new Vector3(-TRACK_SCALE, 0, 0));
            centerList.Add(centerPosition + new Vector3(0, 0, TRACK_SCALE));
            centerList.Add(centerPosition + new Vector3(-TRACK_SCALE, 0, TRACK_SCALE));
        } else if (childTrackPiece.name.Contains("LeftTurn")) {
            centerList.Add(centerPosition + new Vector3(-TRACK_SCALE, 0, 0));
            centerList.Add(centerPosition + new Vector3(0, 0, -TRACK_SCALE));
            centerList.Add(centerPosition + new Vector3(-TRACK_SCALE, 0, -TRACK_SCALE));
        }

        // Adjust for Rotation if not Local
        for (int i = 0; i < centerList.Count && !isLocal; i++) {
            centerList[i] = RotateFromPivot(centerList[i], transform.position, childTrackPiece.transform.rotation);
        }

        return centerList;
    }

    private bool IsInputReset() {
        return SimpleInput.GetAxis("Horizontal") == 0 && SimpleInput.GetAxis("Vertical") == 0;
    }

    private void DisableColliders(GameObject parent) {
        foreach (Collider collider in parent.GetComponentsInChildren<Collider>()) {
            collider.enabled = false;
        }
    }

    private Vector3 RotateFromPivot(Vector3 point, Vector3 pivot, Quaternion rotation) {
        // Direction vector from pivot to point
        Vector3 directionVector = point - pivot;
        Vector3 rotatedDirectionVector = rotation * directionVector;
        Vector3 rotatedPoint = pivot + rotatedDirectionVector;
        return rotatedPoint;
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
}
