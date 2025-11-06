using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Transform itemSlotsParent;
    public GameObject itemSlotPrefab;

    private List<InventorySlotUI> slots = new List<InventorySlotUI>();

    void Start()
    {
        RefreshSlots();
    }

    public void AddItem(ItemData item)
    {
        // まず現在のスロットを確認
        foreach (var slot in slots)
        {
            if (!slot.HasItem())
            {
                slot.SetItem(item);
                return;
            }
        }

        // 🟢 新しいスロットを生成して追加
        GameObject newSlotObj = Instantiate(itemSlotPrefab, itemSlotsParent);
        var newSlot = newSlotObj.GetComponent<InventorySlotUI>();
        newSlot.SetItem(item);
        slots.Add(newSlot);
    }

    void RefreshSlots()
    {
        slots.Clear();
        foreach (Transform child in itemSlotsParent)
        {
            var slot = child.GetComponent<InventorySlotUI>();
            if (slot != null && !slots.Contains(slot))
                slots.Add(slot);
        }
    }
}
