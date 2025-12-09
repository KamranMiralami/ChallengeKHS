using SFXSystem;
using UnityEngine;

public class Magazine : Equipable
{

    public override bool CanBeUsed()
    {
        return UseCount > 0;
    }

    public override void Equip(EquipLocation owner)
    {
        this.owner = owner;
        base.Equip(owner);
    }

    public override void OnUse(Vector3 direction)
    {
        var used = owner.Owner.ReloadWeapon(UseCount);
        SetUseCount(useCount - used);
        SoundSystemManager.Instance.PlaySFX("reload");
        if (UseCount <= 0)
        {
            InventoryManager.Instance.RemoveItemFromInventory(owner.CurrentItem);
            Destroy(gameObject);
        }
    }

    public override void ResetItem()
    {
    }

    public override void Unequip()
    {
        Destroy(gameObject);
    }
}
