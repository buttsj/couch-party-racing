using UnityEngine;

public class LevelSelectionMenuFunctionality : MonoBehaviour {
   
    public GameObject RaceMode;
    public GameObject SpudRun;
    public GameObject TotShot;
    public GameObject TrackBuilder;

    private SceneGenerator sceneGenerator;

    void Start() {
        sceneGenerator = GameObject.Find("SceneGenerator").GetComponent<SceneGenerator>();
        DetermineLevelMenu();
    }

    private void DetermineLevelMenu() {
        var gamemodeName = sceneGenerator.GamemodeName;
        switch (gamemodeName) {
            case "RaceMode":
                RaceMode.SetActive(true);
                break;
            case "SpudRun":
                SpudRun.SetActive(true);
                break;
            case "TotShot":
                TotShot.SetActive(true);
                break;
            case "TrackBuilder":
                TrackBuilder.SetActive(true);
                break;
        }
    }
}
