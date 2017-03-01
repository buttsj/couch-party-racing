using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Cursor : MonoBehaviour {

    private const string TRACK_DIR = "Prefabs/TrackPrefabs/";
    private const string CHILD_NAME = "Track Piece";
    private const int TRACK_SCALE = 60;

    public GameObject trackParent;
    private GameObject childTrackPiece;
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
        Destroy(childTrackPiece);
        childTrackPiece = Instantiate(trackList[trackIndex], transform.position, transform.rotation, transform);

        DisableColliders(childTrackPiece);
    }

    private void PrintTrackName() {
        Debug.Log(trackList[trackIndex].name);
    }

    private void SpawnTrack() {
        if (!IsOverlap()) {
            var trackPiece = Instantiate(trackList[trackIndex], transform.position, transform.rotation, trackParent.transform);
            trackPiece.name = trackList[trackIndex].name;

            DisableColliders(trackPiece);

            trackPiece.AddComponent<BoxCollider>().center = new Vector3(-TRACK_SCALE, TRACK_SCALE, TRACK_SCALE) / 2f;
            trackPiece.GetComponent<BoxCollider>().size = new Vector3(TRACK_SCALE, TRACK_SCALE, TRACK_SCALE);
        }
    }

    private bool IsOverlap() {
        const float OFFSET = 0.001f;
        Vector3 CENTER_OFFSET = new Vector3(-TRACK_SCALE, TRACK_SCALE, TRACK_SCALE) / 2f;
        Vector3 BOX_HALF_SCALE = new Vector3(TRACK_SCALE - OFFSET, TRACK_SCALE - OFFSET, TRACK_SCALE - OFFSET) / 2f;

        var centerPosition = transform.position + CENTER_OFFSET;
        var collisions = DetermineOverlapBoxes(centerPosition, BOX_HALF_SCALE);

        foreach (var collision in collisions) {
            Debug.Log(collision.ToString());
        }

        return collisions.Count > 0;
    }

    private IList DetermineOverlapBoxes(Vector3 centerPosition, Vector3 halfScale) {
        var rotatedCenter = RotateFromPivot(centerPosition, transform.position, childTrackPiece.transform.rotation);

        var collisions = Physics.OverlapBox(rotatedCenter, halfScale, Quaternion.identity, ~0, QueryTriggerInteraction.Ignore).ToList();

        if (childTrackPiece.name.Contains("Ramp")) {
            Vector3 rotatedLeftCenter = RotateFromPivot(centerPosition + new Vector3(-TRACK_SCALE, 0, 0), transform.position, childTrackPiece.transform.rotation);
            Vector3 rotatedTopCenter = RotateFromPivot(centerPosition + new Vector3(0, TRACK_SCALE, 0), transform.position, childTrackPiece.transform.rotation);
            Vector3 rotatedTopLeftCenter = RotateFromPivot(centerPosition + new Vector3(-TRACK_SCALE, TRACK_SCALE, 0), transform.position, childTrackPiece.transform.rotation);

            collisions.AddRange(Physics.OverlapBox(rotatedLeftCenter, halfScale, childTrackPiece.transform.rotation, ~0, QueryTriggerInteraction.Ignore));
            collisions.AddRange(Physics.OverlapBox(rotatedTopCenter, halfScale, childTrackPiece.transform.rotation, ~0, QueryTriggerInteraction.Ignore));
            collisions.AddRange(Physics.OverlapBox(rotatedTopLeftCenter, halfScale, childTrackPiece.transform.rotation, ~0, QueryTriggerInteraction.Ignore));
        } 

        return collisions;
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
}
