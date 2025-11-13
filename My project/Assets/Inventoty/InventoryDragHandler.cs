/*using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDragHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var draggedObj = eventData.pointerDrag;
        if (draggedObj == null) return;

        var fromSlot = draggedObj.GetComponent<InventorySlotUI>();
        var toSlot = eventData.pointerEnter?.GetComponent<InventorySlotUI>();

        if (fromSlot != null && toSlot != null && fromSlot != toSlot)
        {
            InventoryManager.Instance?.TryCombineItems(fromSlot, toSlot);
        }
    }
}
*/