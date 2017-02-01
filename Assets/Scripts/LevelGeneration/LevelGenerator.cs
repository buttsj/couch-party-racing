using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    private TileContainer tileCollection = new TileContainer();
    private string levelName;
    
    public LevelGenerator(string levelName) {
        this.levelName = levelName;
    }

    public void Start() {
        GenerateLevel();
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


}
