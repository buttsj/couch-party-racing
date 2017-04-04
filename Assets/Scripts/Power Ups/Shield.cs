using UnityEngine;

public class Shield : IKartAbility {

    private GameObject owner;
    private KartAudio ownerAudio;

    private AudioClip shieldUse;
    private bool destroy;
    private bool used;
    private float timer;
    private Color flash;
    private Color normal;
    private bool switcher;

    public Shield(GameObject incomingOwner, KartAudio audio)
    {
        switcher = true;
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
        if (timer < 10.0f && used)
        {
            if (switcher)
                owner.GetComponentInChildren<Renderer>().material.color = flash;
            else
                owner.GetComponentInChildren<Renderer>().material.color = normal;

            switcher = !switcher;
            owner.GetComponent<Kart>().IsInvulnerable = true;
            timer += Time.deltaTime;
        }
        else if (timer > 10.0f && used)
        {
            owner.GetComponentInChildren<Renderer>().material.color = normal;
            owner.GetComponent<Kart>().IsInvulnerable = false;
            destroy = true;
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
