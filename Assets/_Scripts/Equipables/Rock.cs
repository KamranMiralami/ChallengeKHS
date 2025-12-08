using UnityEngine;

public class Rock : Equipable
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float force = 500f;
    bool isThrown;
    public override bool CanBeUsed()
    {
        return UseCount > 0;
    }

    public override void Equip(EquipLocation owner)
    {
        this.owner = owner;
        rb.isKinematic = true;
        base.Equip(owner);
    }

    public override void OnUse(Vector3 direction)
    {
        isThrown = true;
        transform.parent = null;
        rb.isKinematic = false;
        rb.AddForce(-direction * force);
        SetUseCount(0);
        InventoryManager.Instance.RemoveItemFromInventory(owner.CurrentItem);
    }

    public override void ResetItem()
    {
    }

    public override void Unequip()
    {
        if (isThrown)
        {
            transform.parent = null;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
