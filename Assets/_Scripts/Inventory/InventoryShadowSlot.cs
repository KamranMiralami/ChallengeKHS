using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryShadowSlot : InventorySlot 
{ 
    [SerializeField] RectTransform rectTransform;
    public override void AssignItem(InventoryItem item)
    {
        rectTransform.position = Input.mousePosition;
        gameObject.SetActive(true);
        base.AssignItem(item);
    }
    public override InventoryItem UnAssignItem()
    {
        gameObject.SetActive(false);
        return base.UnAssignItem();
    }
    private void Update()
    {
        rectTransform.position = Input.mousePosition;
    }
}
