using UnityEngine;

public class NullItem : IKartAbility {

    private GameObject owner;

    public NullItem(GameObject incomingOwner)
    {
        owner = incomingOwner;
    }

    public void UseItem()
    {
        // does nothing
    }

    public override string ToString()
    {
        return "Null";
    }

}
