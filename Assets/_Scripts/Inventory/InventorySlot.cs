using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static Equipable;
using static EquipLocation;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] bool isBodySlot;
    [SerializeField] LocationType locationType;
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] AcceptableItems acceptableItems;
    InventoryItem currentItem;
    public bool HasItem => currentItem != null;
    public bool IsAcceptable(EquipableType type)
    {
        return type switch
        {
            EquipableType.None => true,
            EquipableType.Gun => acceptableItems.HasFlag(AcceptableItems.Gun),
            _ => false,
        };
    }
    public virtual void AssignItem(InventoryItem item)
    {
        if (!IsAcceptable(item.type)) return;
        currentItem = item;
        if (item.type == EquipableType.None)
        {
            currentItem = null;
            txt.text = " ";
        }
        else
        {
            txt.text = item.type.ToString();
            if (isBodySlot)
            {
                InventoryManager.Instance.EquipItemOnPlayer(locationType, item.type, item.numberOfUses);
            }
        }
    }
    public virtual InventoryItem UnAssignItem()
    {
        if (currentItem == null) return null;
        InventoryItem itemToReturn = new InventoryItem(currentItem.type, currentItem.numberOfUses);
        currentItem = null;
        txt.text = " ";
        if (isBodySlot)
        {
            InventoryManager.Instance.UnEquipItemOnPlayer(locationType);
        }
        return itemToReturn;
    }
}
[System.Serializable]
public class InventoryItem
{
    public EquipableType type;
    public int numberOfUses;
    public InventoryItem(EquipableType type, int numberOfUses)
    {
        this.type = type;
        this.numberOfUses = numberOfUses;
    }
}
[System.Flags]
public enum AcceptableItems
{
    None = 0,
    Gun = 1 << 0,   // 1
}
