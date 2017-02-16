using UnityEngine;

public class Spark : IKartAbility {

    private GameObject owner;
    private bool destroy;

    public Spark(GameObject incomingOwner)
    {
        owner = incomingOwner;
        destroy = false;
    }

    public void UseItem()
    {
        // use Spark
        destroy = true;
    }
    
    public override string ToString()
    {
        return "Spark";
    }

    public void Update()
    {

    }

    public bool IsUsed()
    {
        return destroy;
    }
}
