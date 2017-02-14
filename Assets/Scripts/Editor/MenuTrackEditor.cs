using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MenuTrackEditor {

    private static LevelGenerator levelGenerator;

    [MenuItem("GameObject/Track Generator/Load Level", false, 0)]
    private static void LoadLevel(MenuCommand menuCommand) {
        GameObject trackParent = menuCommand.context as GameObject;
        InstantiateLevelGenerator(trackParent);

        levelGenerator.GenerateLevel(trackParent.name + ".xml");

        Debug.Log("Loaded Track: " + menuCommand.context.name);
    }

    [MenuItem("GameObject/Track Generator/Save Level", false, 1)]
    private static void SaveLevel(MenuCommand menuCommand) {
        GameObject trackParent = menuCommand.context as GameObject;
        InstantiateLevelGenerator(trackParent);

        levelGenerator.SaveLevel(trackParent.name + ".xml");

        Debug.Log("Saved Track: " + menuCommand.context.name);
    }

    private static void InstantiateLevelGenerator(GameObject trackParent) {
        levelGenerator = new LevelGenerator(trackParent.transform);
    }
}
