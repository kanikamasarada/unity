using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    private ItemData currentItem;

    public void SetItem(ItemData item)
    {
        currentItem = item;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public bool HasItem()
    {
        return currentItem != null;
    }

    public void ClearItem()
    {
        currentItem = null;
        icon.enabled = false;
        icon.sprite = null;
    }
}
