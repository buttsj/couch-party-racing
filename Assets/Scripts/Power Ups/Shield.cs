﻿using UnityEngine;

public class Shield : IKartAbility {

    private GameObject owner;
    private KartAudio ownerAudio;

    private AudioClip shieldUse;
    private bool destroy;
    private bool used;
    private float timer;
    private Color flash;
    private Color normal;

    public Shield(GameObject incomingOwner, KartAudio audio)
    {
        owner = incomingOwner;
        timer = 0.0f;
        destroy = false;
        used = false;
        //shieldUse = Resources.Load<AudioClip>("");
        ownerAudio = audio;
        flash = Color.clear;
        normal = owner.GetComponentInChildren<Renderer>().material.color;
    }

    public void UseItem()
    {
        if (used == false)
        {
            used = true;
            //ownerAudio.playOneOff(shieldUse);
        }
    }

    public override string ToString()
    {
        return "Shield";
    }

    public void Update()
    {
        if(owner.GetComponent<Kart>() != null)
        {
            if (timer < 5.0f && used)
            {
                owner.GetComponent<Kart>().shield_particle.GetComponent<ParticleSystem>().Play();
                owner.GetComponent<Kart>().IsInvulnerable = true;
                timer += Time.deltaTime;
            }
            else if (timer > 5.0f && used)
            {
                owner.GetComponent<Kart>().shield_particle.GetComponent<ParticleSystem>().Stop();
                destroy = true;
            }
        }
        else if(owner.GetComponent<WaypointAI>() != null)
        {
            if (timer < 5.0f && used)
            {
                owner.GetComponent<WaypointAI>().shield_particle.GetComponent<ParticleSystem>().Play();
                owner.GetComponent<WaypointAI>().IsInvulnerable = true;
                timer += Time.deltaTime;
            }
            else if (timer > 5.0f && used)
            {
                owner.GetComponent<WaypointAI>().shield_particle.GetComponent<ParticleSystem>().Stop();
                owner.GetComponent<WaypointAI>().IsInvulnerable = false;
                destroy = true;
            }
        }
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
