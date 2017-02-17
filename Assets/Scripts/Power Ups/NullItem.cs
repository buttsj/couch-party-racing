using UnityEngine;

public class NullItem : IKartAbility {

    private GameObject owner;
    private bool destroy;
    private bool used;

    public NullItem(GameObject incomingOwner)
    {
        owner = incomingOwner;
        destroy = false;
        used = false;
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

    public bool IsUsing()
    {
        return used;
    }

    public bool IsUsed()
    {
        return destroy;
    }

}
