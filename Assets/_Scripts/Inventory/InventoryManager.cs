using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : SingletonBehaviour<InventoryManager>
{
    public Canvas Canvas;
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
        inventorySlots[0].AssignItem(gunEquipment);

        shadowSlot.UnAssignItem();
    }
    public void Open()
    {
        IsOpen = true;
        InventoryParent.SetActive(true);
    }
    public void Close()
    {
        IsOpen = false;
        InventoryParent.SetActive(false);
    }
    public void EquipItemOnPlayer(EquipLocation.LocationType locationType, Equipable.EquipableType currentItem, int numberOfUses)
    {
        player.EquipItem(locationType, currentItem, numberOfUses);
    }
    public void UnEquipItemOnPlayer(EquipLocation.LocationType locationType)
    {
        player.UnEquipItem(locationType);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckItem(res =>
            {
                if(!res.HasItem) return;
                var item = res.UnAssignItem();
                shadowSlot.AssignItem(item);
                previouslyClicked = res;
            }, null);
        }
        if (Input.GetMouseButtonUp(0))
        {
            CheckItem(res =>
            {
                if (!shadowSlot.HasItem) return;
                var item = shadowSlot.UnAssignItem();
                if (!res.IsAcceptable(item.type) && previouslyClicked != null)
                {
                    previouslyClicked.AssignItem(item);
                }
                res.AssignItem(item);
            },() =>
            {
                if (shadowSlot.HasItem && previouslyClicked != null)
                {
                    var item = shadowSlot.UnAssignItem();
                    previouslyClicked.AssignItem(item);
                }
            });
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
}
