using UnityEngine;

public class Spark : IKartAbility {

    private GameObject owner;
    private bool destroy;
    private bool used;
    private float timer;

    public Spark(GameObject incomingOwner)
    {
        owner = incomingOwner;
        destroy = false;
        used = false;
        timer = 0.0f;
    }

    public void UseItem()
    {
        // use Spark
        if (used == false)
        {
            owner.transform.Find("electricity").gameObject.SetActive(true);
            used = true;
        }
    }
    
    public override string ToString()
    {
        return "Spark";
    }

    public void Update()
    {
        if (used && timer < 5.0f)
        {
            timer = timer + Time.deltaTime;
        }
        else if (used && timer > 5.0f)
        {
            owner.transform.Find("electricity").gameObject.SetActive(false);
            destroy = true;
        }
    }

    public bool IsUsed()
    {
        return destroy;
    }
}
