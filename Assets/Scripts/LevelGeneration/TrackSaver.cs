using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackSaver : MonoBehaviour {

    private const string TRACK_DIR = "Prefabs/TrackPrefabs/";
    private const string MINIMAP_NAME = "RaceModeMinimap";
    
    private LevelGenerator levelGenerator;

    void Start() {
        levelGenerator = new LevelGenerator(transform);
    }

    public void Save(string trackName) {
        GenerateMinimap();
        
        if (!trackName.EndsWith(".xml")) {
            trackName += ".xml";
        }

        levelGenerator.SaveLevel(trackName);
    }

    private void GenerateMinimap() {
        var average = AverageTrackPosition();
        var height = MaxTrackPosition() + 120f;
        Vector3 position = new Vector3(average.x, height, average.z);

        var minimap = Instantiate(Resources.Load<GameObject>(TRACK_DIR + MINIMAP_NAME), transform);
        minimap.transform.position = position;
        minimap.name = MINIMAP_NAME;
    }

    private Vector3 AverageTrackPosition() {
        Vector3 average = Vector3.zero;

        foreach (var child in GetComponentsInChildren<Transform>()) {
            average += child.position;
        }
        average /= GetComponentsInChildren<Transform>().Length;

        return average;
    }

    private float MaxTrackPosition() {
        float max = 0.0f;

        foreach (var child in GetComponentsInChildren<Transform>()) {
            max = Mathf.Max(child.position.y);
        }

        return max;
    }
}
