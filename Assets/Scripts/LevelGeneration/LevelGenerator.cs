using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class LevelGenerator {

    private TileContainer tileCollection;
    private Transform transform;
    private const string TRACK_DIR = "Prefabs/";

    public LevelGenerator(Transform transform) {
        this.transform = transform;
    }

    public void GenerateLevel(string levelName) {
        tileCollection = TileContainer.Load(Path.Combine(Application.dataPath, levelName));
        
        foreach (var item in tileCollection.tileList) {
            item.Instantiate(TRACK_DIR, transform);
        }
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
