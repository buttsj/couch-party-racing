using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class LevelGenerator {

    private TileContainer tileCollection;
    private Transform transform;
    private const string TRACK_DIR = "Prefabs/TrackPrefabs/";
    private const string START_TRACK_NAME = "StartTrack";

    public LevelGenerator(Transform transform) {
        this.transform = transform;
    }

    public GameObject GenerateLevel(string levelName) {
        GameObject startingObj = new GameObject();
        var path = Path.Combine(Application.dataPath, levelName);

        if (File.Exists(path)) {
            tileCollection = TileContainer.Load(path);

            foreach (var item in tileCollection.tileList) {
                Debug.Log("Generating " + item.Name);
                item.Instantiate(TRACK_DIR, transform);
                if (item.Name == START_TRACK_NAME) {
                    startingObj = item.prefab;
                }
            }
        }
        return startingObj;
    }

    public void SaveLevel(string levelName) {
        tileCollection = new TileContainer();
        tileCollection.tileList.Clear();

        foreach (Transform child in transform) {
            tileCollection.tileList.Add(new Tile(child));
        }

        tileCollection.Save(Path.Combine(Application.dataPath, levelName));
    }

}
