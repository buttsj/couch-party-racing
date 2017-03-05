using UnityEngine;

public class Marble : IKartAbility
{
    private GameObject marble;
    private GameObject owner;
    private KartAudio ownerAudio;

    private AudioClip marbleUse;
    public GameObject Owner { get; }
    private bool destroy;
    private bool used;

    public Marble(GameObject incomingOwner, KartAudio audio)
    {
        owner = incomingOwner;
        ownerAudio = audio;

        marbleUse = Resources.Load<AudioClip>("Sounds/KartEffects/marbleLaunch");

        destroy = false;
        used = false;
    }

    public void Update(){ }

    public void UseItem()
    {
        if (!used)
        {
            used = true;
            destroy = true;
            initMarble();
            ownerAudio.playOneOff(marbleUse);
        }
    }

    public override string ToString()
    {
        return "Marble";
    }

    public bool IsUsed()
    {
        return destroy;
    }

    public bool IsUsing()
    {
        return used;
    }

    private void initMarble()
    {
        marble = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marble.name = "Marble";
        marble.GetComponent<SphereCollider>().isTrigger = true;
        marble.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        marble.transform.position = new Vector3(owner.transform.position.x, owner.transform.position.y, owner.transform.position.z);
        marble.GetComponentInChildren<Renderer>().material.color = owner.transform.FindChild("Body").GetComponent<Renderer>().material.color;
        marble.AddComponent<Rigidbody>();
        marble.GetComponent<Rigidbody>().useGravity = false;
        marble.transform.rotation = owner.transform.rotation;

        marble.AddComponent<MarbleManager>();
        marble.GetComponent<MarbleManager>().Owner = owner;

        marble.AddComponent<AudioSource>();
    }
}
