using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Transform slotParent;   // スロットの親 (ItemSlotsPanel)
    private ItemSlot[] slots;

    void Start()
    {
        slots = slotParent.GetComponentsInChildren<ItemSlot>();
    }

    // アイテムを追加
    public void AddItem(Item newItem)
    {
        foreach (var slot in slots)
        {
            // 空きスロットに追加
            if (slot.GetItem() == null)
            {
                slot.SetItem(newItem);
                return;
            }
        }
        Debug.Log("インベントリがいっぱい");
    }

    // アイテムを取り出す（例：インデックス指定）
    public void RemoveItem(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            slots[index].SetItem(null);
        }
    }

    internal void AddItem(Interactable item)
    {
        throw new NotImplementedException();
    }
}
