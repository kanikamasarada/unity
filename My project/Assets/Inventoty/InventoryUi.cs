using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Transform itemSlotsParent;  // アイテムスロットを並べる親オブジェクト
    public GameObject itemSlotPrefab;  // アイテムスロットのプレハブ

    private List<InventorySlotUI> slots = new List<InventorySlotUI>();

    void Start()
    {
        // すでにUIにスロットがある場合はリストに追加
        foreach (Transform child in itemSlotsParent)
        {
            var slot = child.GetComponent<InventorySlotUI>();
            if (slot != null)
                slots.Add(slot);
        }
    }

    // InventoryManager から呼び出される
    public void AddItem(ItemData item)
    {
        // 空きスロットを探して追加
        foreach (var slot in slots)
        {
            if (!slot.HasItem())
            {
                slot.SetItem(item);
                return;
            }
        }

        // 空きがなければ新規スロットを生成
        GameObject newSlotObj = Instantiate(itemSlotPrefab, itemSlotsParent);
        var newSlot = newSlotObj.GetComponent<InventorySlotUI>();
        newSlot.SetItem(item);
        slots.Add(newSlot);
    }
}
