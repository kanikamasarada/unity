using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManagger : MonoBehaviour
{
    public Transform slotParent;
    public TMP_Text itemNameText;          // ← 修正
    public TMP_Text itemDescriptionText;
    public Image itemIcon;

    private Item[] items;

    void Start()
    {
        items = new Item[slotParent.childCount];
        RefreshUI();
    }

    public void AddItem(Item newItem)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = newItem;
                break;
            }
        }
        RefreshUI();
    }

    public void RemoveIten(int index)
    {
        items[index] = null;
        RefreshUI();
    }

    void RefreshUI()
    {
        for (int i = 0; i < slotParent.childCount; i++)
        {
            var slot = slotParent.GetChild(i);
            var icon = slot.GetComponentInChildren<Image>();

            if (items[i] != null)
                icon.sprite = items[i].icon;
            else
                icon.sprite = null;
        }
    }

    public void ShowItemInfo(int index)
    {
        if (items[index] == null) return;

        itemNameText.text = items[index].itemName;        // ← 修正
        itemDescriptionText.text = items[index].description;
        itemIcon.sprite = items[index].icon;
    }
}
