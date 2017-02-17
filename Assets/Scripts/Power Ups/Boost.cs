using UnityEngine;

public class Boost : IKartAbility {

    private GameObject owner;
    private AudioClip boostUse;
    private float boostValue;
    private bool used;
    private bool destroy;

    public Boost(GameObject incomingOwner)
    {
        boostValue = 50.0f;
        used = false;
        destroy = false;
        owner = incomingOwner;
        boostUse = Resources.Load<AudioClip>("Sounds/KartEffects/Boost_Sound");
    }

    public void UseItem()
    {
        if (used == false)
        {
            GameObject.Find("Music Manager HUD").GetComponent<AudioSource>().PlayOneShot(boostUse);
            used = true;
        }
    }

    public override string ToString()
    {
        return "Boost";
    }

    public void Update()
    {
        if (used)
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
    }

    public bool IsUsed()
    {
        return destroy;
    }

}
