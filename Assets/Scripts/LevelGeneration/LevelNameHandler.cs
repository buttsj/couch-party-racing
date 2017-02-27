using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNameHandler : MonoBehaviour {

    public InputField inputField;
    private LevelGenerator levelGenerator;
    
    void Start() {
        levelGenerator = new LevelGenerator(transform);
    } 

	public void SaveName() {
        name = inputField.text;
        levelGenerator.SaveLevel(name + ".xml");
    }

    public void LoadName() {
        name = inputField.text;
        levelGenerator.GenerateLevel(name + ".xml");
    }
}
