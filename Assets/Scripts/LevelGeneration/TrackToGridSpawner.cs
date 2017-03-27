using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackToGridSpawner {

    private const string CHILD_NAME = "Track Piece";
    private const int TRACK_SCALE = 60;

    private const float OFFSET = 0.001f;
    private static readonly Vector3 CENTER_OFFSET = new Vector3(-TRACK_SCALE, TRACK_SCALE, TRACK_SCALE) / 2f;
    private static readonly Vector3 BOX_HALF_SCALE = new Vector3(TRACK_SCALE - OFFSET, TRACK_SCALE - OFFSET, TRACK_SCALE - OFFSET) / 2f;

    //private GameObject trackPieceToConvert;
    private GameObject trackParent;

    public TrackToGridSpawner(GameObject trackParent) {
        this.trackParent = trackParent;
    }

    public void SpawnTrack(GameObject trackPieceToConvert) {
        if (!IsOverlap(trackPieceToConvert)) {
            var spawnedTrackPiece = Object.Instantiate(trackPieceToConvert, trackParent.transform);
            spawnedTrackPiece.name = trackPieceToConvert.name;

            DisableColliders(spawnedTrackPiece);

            AddColliders(spawnedTrackPiece, trackPieceToConvert);
        }
    }

    public void DeleteTrack(GameObject trackPieceToConvert, List<GameObject> trackList) {
        if (IsOverlap(trackPieceToConvert)) {
            var centerPosition = trackPieceToConvert.transform.position + CENTER_OFFSET;
            var collisions = GetOverlapBoxes(trackPieceToConvert, centerPosition, BOX_HALF_SCALE);

            foreach (var collision in collisions) {
                if (trackList.Find(x => collision.name == x.name)) {
                    Object.Destroy(collision.gameObject);
                }
            }
        }
    }

    public void DisableColliders(GameObject parent) {
        foreach (Collider collider in parent.GetComponentsInChildren<Collider>()) {
            collider.enabled = false;
        }
    }

    public void AddColliders(GameObject spawnedTrackPiece, GameObject trackPieceToConvert) {
        foreach (var center in GetCenterPoints(trackPieceToConvert, CENTER_OFFSET, true)) {
            spawnedTrackPiece.AddComponent<BoxCollider>().center = center;
        }

        // Set Size of Colliders
        foreach (var collider in spawnedTrackPiece.GetComponentsInChildren<BoxCollider>()) {
            if (collider.enabled) {
                collider.size = Vector3.one * TRACK_SCALE;
            }
        }
    }

    private bool IsOverlap(GameObject trackPieceToConvert) {
        var centerPosition = trackPieceToConvert.transform.position + CENTER_OFFSET;
        var collisions = GetOverlapBoxes(trackPieceToConvert, centerPosition, BOX_HALF_SCALE);

        foreach (var collision in collisions) {
            Debug.Log(collision.ToString());
        }

        return collisions.Count > 0;
    }

    private List<Collider> GetOverlapBoxes(GameObject trackPieceToConvert, Vector3 centerPosition, Vector3 halfScale) {
        List<Collider> collisions = new List<Collider>();

        foreach (var center in GetCenterPoints(trackPieceToConvert, centerPosition, false)) {
            collisions.AddRange(Physics.OverlapBox(center, halfScale, trackPieceToConvert.transform.rotation, ~0, QueryTriggerInteraction.Ignore).ToList());
        }

        return collisions;
    }

    private List<Vector3> GetCenterPoints(GameObject trackPieceToConvert, Vector3 centerPosition, bool isLocal) {
        List<Vector3> centerList = new List<Vector3>();

        centerList.Add(centerPosition);

        if (trackPieceToConvert.name.Contains("Ramp")) {
            centerList.Add(centerPosition + new Vector3(-TRACK_SCALE, 0, 0));
            centerList.Add(centerPosition + new Vector3(0, TRACK_SCALE, 0));
            centerList.Add(centerPosition + new Vector3(-TRACK_SCALE, TRACK_SCALE, 0));
        } else if (trackPieceToConvert.name.Contains("RightTurn")) {
            centerList.Add(centerPosition + new Vector3(-TRACK_SCALE, 0, 0));
            centerList.Add(centerPosition + new Vector3(0, 0, TRACK_SCALE));
            centerList.Add(centerPosition + new Vector3(-TRACK_SCALE, 0, TRACK_SCALE));
        } else if (trackPieceToConvert.name.Contains("LeftTurn")) {
            centerList.Add(centerPosition + new Vector3(-TRACK_SCALE, 0, 0));
            centerList.Add(centerPosition + new Vector3(0, 0, -TRACK_SCALE));
            centerList.Add(centerPosition + new Vector3(-TRACK_SCALE, 0, -TRACK_SCALE));
        }

        // Adjust for Rotation if not Local
        for (int i = 0; i < centerList.Count && !isLocal; i++) {
            centerList[i] = RotateFromPivot(centerList[i], trackPieceToConvert.transform.position, trackPieceToConvert.transform.rotation);
        }

        return centerList;
    }

    private Vector3 RotateFromPivot(Vector3 point, Vector3 pivot, Quaternion rotation) {
        // Direction vector from pivot to point
        Vector3 directionVector = point - pivot;
        Vector3 rotatedDirectionVector = rotation * directionVector;
        Vector3 rotatedPoint = pivot + rotatedDirectionVector;
        return rotatedPoint;
    }
}
