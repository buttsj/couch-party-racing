using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleManager : MonoBehaviour {

    private const float MAXSPEED = 250f;

    private int targetIndex;
    private GameObject owner;
    private GameObject[] targets;
    private float lifeTimer;
    private const float MAXTIME = 8.0f;
    private AudioClip marbleHit;

    public GameObject Owner { set { owner = value; } }

	void Start () {
        targetIndex = -1;

        lifeTimer = 0.0f;

        gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * MAXSPEED;

        targets = GameObject.FindGameObjectsWithTag("Player");

        marbleHit = Resources.Load<AudioClip>("Sounds/KartEffects/marbleCollision");
    }
	
	void Update () {
        
        if (targetIndex != -1)
        {
            transform.LookAt(targets[targetIndex].transform);
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * MAXSPEED;
        }
        else
        {
            for(int i = 0; i < targets.Length && targetIndex == -1; i++)
            {
                Vector3 heading = targets[i].transform.position - transform.position;
                float dot = Vector3.Dot(heading, transform.forward);
                if(Vector3.Distance(targets[i].transform.position, gameObject.transform.position) < 150.0f && !ReferenceEquals(targets[i], owner) && dot > 0)
                {
                    targetIndex = i;
                }
            }
        }

        lifeTimer += Time.deltaTime;
        if(lifeTimer >= MAXTIME)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag.Contains("Track"))
        {
            playCollisionEffect();
            Destroy(gameObject);
        }
        else if (other.gameObject.tag.Contains("Player") && !ReferenceEquals(other.gameObject, owner))
        {
            playCollisionEffect();
        }

    }

    public bool validTarget(GameObject target)
    {
        return !ReferenceEquals(target, owner);
    }

    private void playCollisionEffect()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(marbleHit, 1.0f);
    }

}
