﻿using UnityEngine;

public class Oil : IKartAbility {

    private GameObject owner;
    private KartAudio ownerAudio;

    private Object oil;
    private GameObject oilObj;
    private AudioClip oilUse;
    private bool destroy;
    private bool used;
    private float timer;

    public Oil(GameObject incomingOwner, KartAudio audio)
    {
        owner = incomingOwner;
        timer = 0.0f;
        destroy = false;
        used = false;
        oilUse = Resources.Load<AudioClip>("Sounds/KartEffects/OilSound");
        ownerAudio = audio;
    }

	public void UseItem()
    {
        // use Oil
        if (used == false)
        {
            used = true;
            oil = Resources.Load("Prefabs/Powerups/Oil");
            Vector3 oilPos = owner.transform.position - owner.transform.forward - owner.transform.up;
            oilObj = (GameObject)Object.Instantiate(oil, oilPos, Quaternion.Euler(new Vector3(owner.transform.eulerAngles.x - 90f, owner.transform.eulerAngles.y, owner.transform.eulerAngles.z)));
            ownerAudio.playOneOff(oilUse);
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
