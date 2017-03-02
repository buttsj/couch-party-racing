using UnityEngine;

public class ConveyorScript : MonoBehaviour {

    public GameObject conveyor;
    public bool direction = true;
    private float scrollSpeed = 0.5f;

    // Use this for initialization
    void Start () {
        if (direction)
        {
            Vector2 tmp = conveyor.GetComponent<Renderer>().material.GetTextureScale("_MainTex");
            conveyor.GetComponent<Renderer>().material.SetTextureScale("_MainTex", -tmp);
        }
    }
	
	// Update is called once per frame
	void Update () {
        float offset = Time.time * scrollSpeed;
        conveyor.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
    }
}
