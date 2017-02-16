using UnityEngine;

public class Spark : IKartAbility {

    private GameObject owner;

	public Spark(GameObject incomingOwner)
    {
        owner = incomingOwner;
    }

    public void UseItem()
    {

    }
    
    public override string ToString()
    {
        return "Spark";
    }
}
