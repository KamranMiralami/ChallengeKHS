using UnityEngine;
using static Equipable;

public class Hand : EquipLocation
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
            case EquipableType.Gun:
                var itemInHandPrefab = ResourceManager.Instance.GetItemPrefab(EquipableType.Gun);
                itemInHand = Instantiate(itemInHandPrefab, transform);
                itemInHand.Equip(this);
                itemInHand.SetUseCount(item.NumberOfUses);
                break;
            case EquipableType.Magazine:
                var magazinePrefab = ResourceManager.Instance.GetItemPrefab(EquipableType.Magazine);
                itemInHand = Instantiate(magazinePrefab, transform);
                itemInHand.Equip(this);
                itemInHand.SetUseCount(item.NumberOfUses);
                break;
            case EquipableType.Rock:
                var rockPrefab = ResourceManager.Instance.GetItemPrefab(EquipableType.Rock);
                itemInHand = Instantiate(rockPrefab, transform);
                itemInHand.Equip(this);
                itemInHand.SetUseCount(item.NumberOfUses);
                break;
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
        if (!HasItem()) return;
        if (!itemInHand.CanBeUsed()) return;
        switch(itemInHand.Type)
        {
            case EquipableType.Gun:
                UseWithDirection();
                break;
            case EquipableType.Magazine:
                ItemInHand.OnUse(Vector3.zero);
                break;
            case EquipableType.Rock:
                UseWithDirection();
                break;
            case EquipableType.Flashlight:
                ItemInHand.OnUse(Vector3.zero);
                break;
            case EquipableType.Hat:
                break;
            default:
                throw new System.NotImplementedException();
        }
    }
    public void ResetItem()
    {
        if (!HasItem()) return;
        itemInHand.ResetItem();
    }
    void UseWithDirection()
    {
        Vector2 screenPoint = GameManager.Instance.Crosshair.position;
        Ray ray = owner.Camera.ScreenPointToRay(screenPoint);
        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit, 1000f) ? hit.point : ray.GetPoint(100f);
        itemInHand.OnUse(targetPoint);
    }
}
