using UnityEngine;

public class NullItem : IKartAbility {

    private GameObject owner;
    private bool destroy;

    public NullItem(GameObject incomingOwner)
    {
        owner = incomingOwner;
        destroy = false;
    }

    public void UseItem()
    {
        // does nothing
    }

    public override string ToString()
    {
        return "Null";
    }

    public void Update()
    {

    }

    public bool IsUsed()
    {
        return destroy;
    }

}
