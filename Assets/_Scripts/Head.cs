using UnityEngine;
using static Equipable;

public class Head : EquipLocation
{
    public override void EquipItem(InventoryItem item)
    {
        if (HasItem())
        {
            UnEquipItem();
        }
        CurrentItem = item;
        switch (item.type)
        {
            case EquipableType.Flashlight:
                var flashlightPrefab = ResourceManager.Instance.GetItemPrefab(EquipableType.Flashlight);
                itemInHand = Instantiate(flashlightPrefab, transform);
                itemInHand.Equip(this);
                itemInHand.SetUseCount(item.NumberOfUses);
                break;
            case EquipableType.Hat:
                var hatPrefab = ResourceManager.Instance.GetItemPrefab(EquipableType.Hat);
                itemInHand = Instantiate(hatPrefab, transform);
                itemInHand.Equip(this);
                itemInHand.SetUseCount(item.NumberOfUses);
                break;
            default:
                UnEquipItem();
                break;
        }
    }

    public override void UnEquipItem()
    {
        itemInHand.Unequip();
        itemInHand = null;
        CurrentItem = null;
    }

    public override void UseItem()
    {
        // head is not usable
        throw new System.NotImplementedException();
    }
}
