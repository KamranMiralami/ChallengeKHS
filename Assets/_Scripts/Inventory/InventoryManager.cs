using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : SingletonBehaviour<InventoryManager>
{
    public Canvas Canvas;
    public Action<bool> OnInventoryStateChanged;
    [HideInInspector] public bool IsOpen;
    [SerializeField] GameObject InventoryParent;
    [SerializeField] List<InventorySlot> inventorySlots;
    [SerializeField] List<InventorySlot> bodySlots;
    [SerializeField] InventoryShadowSlot shadowSlot;
    PlayerController player;


    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    private InventorySlot previouslyClicked;
    private void Start()
    {
        player = GameManager.Instance.CurrentPlayer;
    }
    public void Initialize()
    {
        raycaster = Canvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;

        var gunEquipment = new InventoryItem(Equipable.EquipableType.Gun, 20);
        var magEquipment = new InventoryItem(Equipable.EquipableType.Magazine, 20);
        var rockEquipment = new InventoryItem(Equipable.EquipableType.Rock, 1);
        var flashlightEquipment = new InventoryItem(Equipable.EquipableType.Flashlight, 2);
        var hatEquipment = new InventoryItem(Equipable.EquipableType.Hat, 1);
        inventorySlots[0].AssignItem(gunEquipment);
        inventorySlots[1].AssignItem(magEquipment);
        inventorySlots[2].AssignItem(rockEquipment);
        inventorySlots[3].AssignItem(flashlightEquipment);
        inventorySlots[4].AssignItem(hatEquipment);

        shadowSlot.UnAssignItem();
    }
    public void Open()
    {
        IsOpen = true;
        InventoryParent.SetActive(true);
        Utils.UnLockMouse();
        OnInventoryStateChanged?.Invoke(true);
    }
    public void Close()
    {
        IsOpen = false;
        InventoryParent.SetActive(false);
        Utils.LockMouse();
        OnInventoryStateChanged?.Invoke(false);
    }
    public void EquipItemOnPlayer(EquipLocation.LocationType locationType, InventoryItem item)
    {
        player.EquipItem(locationType, item);
    }
    public void UnEquipItemOnPlayer(EquipLocation.LocationType locationType)
    {
        player.UnEquipItem(locationType);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckItem(PressedOnItem, null);
        }
        if (Input.GetMouseButtonUp(0))
        {
            CheckItem(ReleasedOnItem, ReleasedOnEmpty);
        }
    }
    void PressedOnItem(InventorySlot res)
    {
        if (!res.HasItem) return;
        var item = res.UnAssignItem();
        shadowSlot.AssignItem(item);
        previouslyClicked = res;
    }
    void ReleasedOnItem(InventorySlot res)
    {
        if (!shadowSlot.HasItem) return;
        var item = shadowSlot.UnAssignItem();
        if (!res.IsAcceptable(item.type) && previouslyClicked != null)
        {
            previouslyClicked.AssignItem(item);
            return;
        }
        if (res.HasItem)
        {
            SwapItem(res, item);
            return;
        }
        res.AssignItem(item);
    }
    void SwapItem(InventorySlot res, InventoryItem item)
    {
        var swapItem = res.UnAssignItem();
        if (previouslyClicked.IsAcceptable(swapItem.type))
        {
            previouslyClicked.AssignItem(swapItem);
        }
        else
        {
            foreach (var slot in inventorySlots)
            {
                if (!slot.HasItem)
                {
                    slot.AssignItem(swapItem);
                    break;
                }
            }
        }
        res.AssignItem(item);
    }
    void ReleasedOnEmpty()
    {
        if (shadowSlot.HasItem && previouslyClicked != null)
        {
            var item = shadowSlot.UnAssignItem();
            previouslyClicked.AssignItem(item);
        }
    }

    private void CheckItem(Action<InventorySlot> found, Action notFound)
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("Draggable")) // tag your draggable items
            {
                result.gameObject.TryGetComponent<InventorySlot>(out InventorySlot slot);
                found?.Invoke(slot);
                return;
            }
        }
        notFound?.Invoke();
    }
    public void RemoveItemFromInventory(InventoryItem item)
    {
        foreach (var slot in inventorySlots)
        {
            if(slot.CurrentItem == item)
            {
                slot.UnAssignItem();
                return;
            }
        }
        foreach (var slot in bodySlots)
        {
            if (slot.CurrentItem == item)
            {
                slot.UnAssignItem();
                return;
            }
        }
    }
}
