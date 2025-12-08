using UnityEngine;

public abstract class Equipable : MonoBehaviour
{
    public enum EquipableType
    {
        None,
        Gun,
        Magazine,
        Flashlight,
        Rock,
        Hat,
    }
    public EquipableType Type;
    [SerializeField] protected Vector3 equipedHandOffset;
    [SerializeField] protected Vector3 equipedHeadOffset;
    protected int useCount;
    protected EquipLocation owner;
    public virtual void Equip(EquipLocation owner)
    {
        this.owner = owner;
        switch (owner.Type)
        {
            case EquipLocation.LocationType.LeftHand:
                transform.localPosition += equipedHandOffset;
                break;
            case EquipLocation.LocationType.RightHand:
                var offset = new Vector3(-equipedHandOffset.x, equipedHandOffset.y, equipedHandOffset.z);
                transform.localPosition += offset;
                break;
            case EquipLocation.LocationType.Head:
                transform.localPosition += equipedHeadOffset;
                break;
            default:
                break;
        }
    }
    public abstract void Unequip();
    public abstract void OnUse(Vector3 direction);
    public abstract bool CanBeUsed();
    public abstract void ResetItem();
    public virtual void SetUseCount(int count)
    {
        count = Mathf.Clamp(count, 0, int.MaxValue);
        useCount = count;
        owner.CurrentItem.NumberOfUses = count;
    }
    public virtual int UseCount => useCount;
}
