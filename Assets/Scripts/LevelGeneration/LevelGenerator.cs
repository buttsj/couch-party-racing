using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    private TileContainer tileCollection;
    private string levelName;
    
    public LevelGenerator(string levelName) {
        this.levelName = levelName;
    }

    public void Start() {

    }

    public void Update() {
        if(Input.GetKey(KeyCode.M)) {
            SaveLevel();
        }
        if(Input.GetKey(KeyCode.L)) {
            GenerateLevel();
        }
    }

    public void GenerateLevel() {
        tileCollection = TileContainer.Load(Path.Combine(Application.dataPath, "tiles.xml"));
        
        foreach (var item in tileCollection.tileList) {
            Debug.Log(item.Size.X);
            Debug.Log(item.Size.Y);
            Debug.Log(item.Size.Z);
            Debug.Log(item.Name);

            item.Instantiate("Prefabs/Track/", transform);
        }
    }

    public void SaveLevel() {
        tileCollection = new TileContainer();
        tileCollection.tileList.Clear();

        foreach (Transform child in transform) {
            tileCollection.tileList.Add(new Tile(child));
        }

        tileCollection.Save(Path.Combine(Application.dataPath, levelName));
    }

}
