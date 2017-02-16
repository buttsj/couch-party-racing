using UnityEngine;

public class Oil : IKartAbility {

    private GameObject owner;
    private bool destroy;

    public Oil(GameObject incomingOwner)
    {
        owner = incomingOwner;
        destroy = false;
    }

	public void UseItem()
    {
        // use Oil
        destroy = true;
    }

    public override string ToString()
    {
        return "Oil";
    }

    public void Update()
    {

    }

    public bool IsUsed()
    {
        return destroy;
    }
}
