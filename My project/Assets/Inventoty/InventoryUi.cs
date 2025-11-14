using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("スロットをまとめる親")]
    public Transform itemSlotsParent;

    [Header("スロットプレハブ")]
    public GameObject itemSlotPrefab;

    private List<InventorySlotUI> slots = new List<InventorySlotUI>();

    void Awake()
    {
        if (itemSlotPrefab != null && itemSlotPrefab.activeSelf)
            itemSlotPrefab.SetActive(false);
    }

    public void AddItem(ItemData item)
    {
        if (item == null) return;

        foreach (var slot in slots)
        {
            if (!slot.HasItem())
            {
                slot.SetItem(item);
                return;
            }
        }

        if (itemSlotPrefab != null && itemSlotsParent != null)
        {
            GameObject newSlotObj = Instantiate(itemSlotPrefab, itemSlotsParent);
            newSlotObj.SetActive(true);
            var slotUI = newSlotObj.GetComponent<InventorySlotUI>();
            slotUI.SetItem(item);
            slots.Add(slotUI);
        }
        else
        {
            Debug.LogError("❌ itemSlotPrefab または itemSlotsParent が未設定です");
        }
    }
public void RemoveEmptySlots()
{
    List<InventorySlotUI> removeList = new List<InventorySlotUI>();

    foreach (var slot in slots)
    {
        if (!slot.HasItem())
        {
            removeList.Add(slot);
        }
    }

    // 実際に破棄
    foreach (var slot in removeList)
    {
        slots.Remove(slot);
        Destroy(slot.gameObject);
    }
}

    public void ClearAllSlots()
    {
        foreach (var slot in slots)
        {
            slot.SetItem(null);
        }
    }
}
