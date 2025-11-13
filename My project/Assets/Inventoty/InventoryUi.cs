using UnityEngine;
using System.Collections.Generic;
public class InventoryUI : MonoBehaviour
{
    public Transform itemSlotsParent;
    public GameObject itemSlotPrefab;
    private List<InventorySlotUI> slots = new List<InventorySlotUI>();
    void Start()
    {
        if (itemSlotPrefab.activeSelf)
            itemSlotPrefab.SetActive(false);
    }
    public void AddItem(ItemData item)
    {
        // 新しいスロットを生成
        GameObject newSlot = Instantiate(itemSlotPrefab, itemSlotsParent);
        newSlot.SetActive(true);
        var slotUI = newSlot.GetComponent<InventorySlotUI>();
        slotUI.SetItem(item);
        slots.Add(slotUI);
    }
}