using UnityEngine;

public class Oil : IKartAbility {

    private GameObject owner;

    public Oil(GameObject incomingOwner)
    {
        owner = incomingOwner;
    }

	public void UseItem()
    {
        // use Oil
    }

    public override string ToString()
    {
        return "Oil";
    }
}
