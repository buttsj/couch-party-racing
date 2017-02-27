using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleManager : MonoBehaviour {

    private int targetIndex;
    private GameObject owner;
    private GameObject[] targets;
    private float lifeTimer;
    private const float MAXTIME = 8.0f;

    public GameObject Owner { set { owner = value; } }

	void Start () {
        targetIndex = -1;

        lifeTimer = 0.0f;

        gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * 250f;

        targets = GameObject.FindGameObjectsWithTag("Player");
    }
	
	void Update () {
        
        if (targetIndex != -1)
        {
            transform.LookAt(targets[targetIndex].transform);
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * 250f;
        }
        else
        {
            for(int i = 0; i < targets.Length && targetIndex == -1; i++)
            {
                Vector3 heading = targets[i].transform.position - transform.position;
                float dot = Vector3.Dot(heading, transform.forward);
                if(Vector3.Distance(targets[i].transform.position, gameObject.transform.position) < 100.0f && !ReferenceEquals(targets[i], owner) && dot > 0)
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

    public bool validTarget(GameObject target)
    {
        return !ReferenceEquals(target, owner);
    }

}
