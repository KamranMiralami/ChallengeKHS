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
    public InventoryItem CurrentItem;
    protected PlayerController owner;
    protected Equipable itemInHand;
    [SerializeField] LocationType type;
    public LocationType Type => type;
    public void Initialize(PlayerController owner)
    {
        this.owner = owner;
    }
    public abstract void UseItem();
    public abstract void EquipItem(InventoryItem item);
    public abstract void UnEquipItem();
    public bool HasItem()
    {
        return itemInHand != null;
    }
    public PlayerController Owner => owner;
    public Equipable ItemInHand => itemInHand;
}
