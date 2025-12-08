using UnityEngine;
using static Equipable;

public class Hand : EquipLocation
{
    public override void EquipItem(EquipableType itemType, int numberOfUses)
    {
        if (HasItem())
        {
            UnEquipItem();
        }
        switch(itemType)
        {
            case EquipableType.Gun:
                var itemInHandPrefab = ResourceManager.Instance.GetItemPrefab(EquipableType.Gun);
                itemInHand = Instantiate(itemInHandPrefab, transform);
                if(itemInHand is Gun gun)
                {
                    gun.BulletCount = numberOfUses;
                }
                itemInHand.Equip(owner);
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
    }

    public override void UseItem()
    {
        if (!HasItem()) return;
        if (!itemInHand.CanBeUsed()) return;
        switch(itemInHand.Type)
        {
            case EquipableType.Gun:
                UsedGun();
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
    void UsedGun()
    {
        Vector2 screenPoint = GameManager.Instance.Crosshair.position;
        Ray ray = owner.Camera.ScreenPointToRay(screenPoint);
        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit, 1000f) ? hit.point : ray.GetPoint(100f);
        itemInHand.OnUse(targetPoint);
    }
}
