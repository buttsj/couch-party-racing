using UnityEngine;

public class Magnet : IKartAbility {

    private GameObject owner;
    private KartAudio ownerAudio;
    private AudioClip magnetUse;
    private bool destroy;
    private bool used;
    private float timer;

    public Magnet(GameObject incomingOwner, KartAudio audio)
    {
        owner = incomingOwner;
        destroy = false;
        used = false;
        timer = 0.0f;
        //magnetUse = Resources.Load<AudioClip>("");
        ownerAudio = audio;
    }

    public void UseItem()
    {
        // use Magnet
        if (used == false)
        {
            //owner.transform.Find("").gameObject.SetActive(true);
            used = true;
            //ownerAudio.playOneOff(magnetUse);
        }
    }

    public override string ToString()
    {
        return "Magnet";
    }

    public void Update()
    {
        // update magnet
        if (used && timer < 5.0f)
        {
            timer = timer + Time.deltaTime;
        }
        else if (used && timer > 5.0f)
        {
            //owner.transform.Find("").gameObject.SetActive(false);
            destroy = true;
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
