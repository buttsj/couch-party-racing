using UnityEngine;

public class Oil : IKartAbility {

    private GameObject owner;
    private Object oil;
    private GameObject oilObj;
    private AudioClip oilUse;
    private bool destroy;
    private bool used;
    private float timer;

    public Oil(GameObject incomingOwner)
    {
        owner = incomingOwner;
        timer = 0.0f;
        destroy = false;
        used = false;
        oilUse = Resources.Load<AudioClip>("Sounds/KartEffects/OilSound");
    }

	public void UseItem()
    {
        // use Oil
        if (used == false)
        {
            used = true;
            oil = Resources.Load("Prefabs/Powerups/Oil");
            Vector3 oilPos = new Vector3(owner.transform.position.x, owner.transform.position.y - 1, owner.transform.position.z);
            oilObj = (GameObject)Object.Instantiate(oil, oilPos, Quaternion.Euler(new Vector3(-90, 0, 0)));
            GameObject.Find("Music Manager HUD").GetComponent<AudioSource>().PlayOneShot(oilUse);
            destroy = true;
        }
    }

    public override string ToString()
    {
        return "Oil";
    }

    public void Update()
    {
        /*if (timer < 10.0f && used)
        {
            timer += Time.deltaTime;

        }
        else if (timer > 10.0f && used)
        {
            Object.Destroy(oilObj);
            destroy = true;
        }*/
    }

    public bool IsUsed()
    {
        return destroy;
    }

    public bool IsUsing()
    {
        return used;
    }
}
