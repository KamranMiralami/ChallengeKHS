using System;
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
    public InventoryItem CurrentItem => currentItem;
    public bool IsAcceptable(EquipableType type)
    {
        return type switch
        {
            EquipableType.None => true,
            EquipableType.Gun => acceptableItems.HasFlag(AcceptableItems.Gun),
            EquipableType.Magazine => acceptableItems.HasFlag(AcceptableItems.Mag),
            EquipableType.Rock => acceptableItems.HasFlag(AcceptableItems.Rock),
            EquipableType.Flashlight => acceptableItems.HasFlag(AcceptableItems.Flashlight),
            EquipableType.Hat => acceptableItems.HasFlag(AcceptableItems.Hat),
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
            if (item.type == EquipableType.Flashlight)
            {
                txt.text = item.type.ToString() + " (" + (item.NumberOfUses == 1 ? "On" : "Off") + ")";
            }
            else
            {
                txt.text = item.type.ToString() + " (" + item.NumberOfUses + ")";
            }
            item.OnUseNumberChanged = (val) =>
            {
                if (item.type == EquipableType.Flashlight)
                {
                    txt.text = item.type.ToString() + " (" + (item.NumberOfUses == 1 ? "On" : "Off") + ")";
                }
                else
                {
                    txt.text = item.type.ToString() + " (" + val + ")";
                }
            };
            if (isBodySlot)
            {
                InventoryManager.Instance.EquipItemOnPlayer(locationType, item);
            }
        }
    }

    public virtual InventoryItem UnAssignItem()
    {
        if (currentItem == null) return null;
        InventoryItem itemToReturn = new InventoryItem(currentItem.type, currentItem.NumberOfUses);
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
    public Action<int> OnUseNumberChanged;
    public EquipableType type;
    int numberOfUses;
    public InventoryItem(EquipableType type, int numberOfUses)
    {
        this.type = type;
        this.numberOfUses = numberOfUses;
    }
    public int NumberOfUses
    {
        get { return numberOfUses; }
        set
        {
            numberOfUses = value;
            OnUseNumberChanged?.Invoke(value);
        }
    }
}
[System.Flags]
public enum AcceptableItems
{
    None = 0,
    Gun = 1 << 0,
    Mag = 1 << 1,
    Rock = 1 << 2,
    Flashlight = 1 << 3,
    Hat = 1 << 4,
}
