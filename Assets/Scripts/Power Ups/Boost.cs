using UnityEngine;

public class Boost : IKartAbility {

    private GameObject owner;
    private KartAudio ownerAudio;
    private AudioClip boostUse;
    private float boostValue;
    private bool used;
    private bool destroy;

    public Boost(GameObject incomingOwner, KartAudio audio)
    {
        boostValue = 50.0f;
        used = false;
        destroy = false;
        owner = incomingOwner;
        boostUse = Resources.Load<AudioClip>("Sounds/KartEffects/Boost_Sound");
        ownerAudio = audio;
    }

    public void UseItem()
    {
        if (used == false)
        {
            ownerAudio.playOneOff(boostUse);
            used = true;
        }
    }

    public override string ToString()
    {
        return "Boost";
    }

    public void Update()
    {
        if (used && owner.GetComponent<Kart>() != null)
        {
            owner.GetComponent<Kart>().green_arrow.GetComponent<ParticleSystem>().Play();
            if (owner.GetComponent<Kart>().Boost < 100.0f)
            {
                owner.GetComponent<Kart>().Boost += 1.0f;
                boostValue -= 1.0f;
            }
            else
            {
                boostValue = 0.0f;
                destroy = true;
            }
            if (boostValue == 0.0f)
            {
                owner.GetComponent<Kart>().green_arrow.GetComponent<ParticleSystem>().Stop();
                destroy = true;
            }
        }
        else if(used && owner.GetComponent<WaypointAI>() != null)
        {
            if (owner.GetComponent<WaypointAI>().Boost < 100.0f)
            {
                owner.GetComponent<WaypointAI>().Boost += 1.0f;
                boostValue -= 1.0f;
            }
            else
            {
                boostValue = 0.0f;
                destroy = true;
            }
            if (boostValue == 0.0f)
            {
                destroy = true;
            }
        }

    }

    public bool IsUsing()
    {
        return used;
    }

    public bool IsUsed()
    {
        return destroy;
    }

}
