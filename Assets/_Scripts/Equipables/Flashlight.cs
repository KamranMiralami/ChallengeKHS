using UnityEngine;

public class Flashlight : Equipable
{
    [SerializeField] Light lightSource;
    bool used;
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
        if(used) return;
        used = true;
        SetUseCount(UseCount == 2? 1:2);
    }

    public override void ResetItem()
    {
        used = false;
    }

    public override void Unequip()
    {
        Destroy(gameObject);
    }
    public override void SetUseCount(int count)
    {
        lightSource.enabled = count == 1;
        base.SetUseCount(count);
    }
}
