using UnityEngine;

public class OilManager : MonoBehaviour {

    private float timer;
    private float invulnTime;
    private bool invuln;
    public bool Invulnerable { get { return invuln; } }

	// Use this for initialization
	void Start () {
        timer = 0.0f;
        invulnTime = 0.0f;
        invuln = true;
	}
	
	// Update is called once per frame
	void Update () {
        timer = timer + Time.deltaTime;
        if (invulnTime < 3.0f)
        {
            invulnTime += Time.deltaTime;
        }
        else
        {
            invuln = false;
        }
        if (timer > 500.0f)
        {
            Destroy(gameObject); // this item has lasted too long, clean up
        }
	}
}
