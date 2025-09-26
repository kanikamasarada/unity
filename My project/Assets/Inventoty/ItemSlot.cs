using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private Item currentItem;
    public Image iconImage;

    public void SetItem(Item newItem)
    {
        currentItem = newItem;

        if (iconImage != null)
        {
            if (newItem != null)
            {
                iconImage.sprite = newItem.icon;
                iconImage.enabled = true;
            }
            else
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }
        }
    }

    public Item GetItem()
    {
        return currentItem;
    }
}
