using UnityEngine;
using static Equipable;

public abstract class EquipLocation : MonoBehaviour
{
    public enum LocationType
    {
        None,
        LeftHand,
        RightHand,
        Head
    }
    protected PlayerController owner;
    protected Equipable itemInHand;
    public void Initialize(PlayerController owner)
    {
        this.owner = owner;
    }
    public abstract void UseItem();
    public abstract void EquipItem(EquipableType itemType, int numberOfUses);
    public abstract void UnEquipItem();
    public bool HasItem()
    {
        return itemInHand != null;
    }
    public Equipable ItemInHand => itemInHand;
}
