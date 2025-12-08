using UnityEngine;

public abstract class Equipable : MonoBehaviour
{
    public enum EquipableType
    {
        None,
        Gun,
    }
    public EquipableType Type;
    protected PlayerController owner;
    public abstract void Equip(PlayerController owner);
    public abstract void Unequip();
    public abstract void OnUse(Vector3 direction);
    public abstract bool CanBeUsed();
    public abstract void ResetItem();
}
