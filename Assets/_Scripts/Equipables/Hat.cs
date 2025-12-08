using UnityEngine;

public class Hat : Equipable
{
    public override bool CanBeUsed()
    {
        return UseCount > 0;
    }

    public override void Equip(EquipLocation owner)
    {
        base.Equip(owner);
    }

    public override void OnUse(Vector3 direction)
    {
    }

    public override void ResetItem()
    {
    }

    public override void Unequip()
    {
        Destroy(gameObject);
    }
}
