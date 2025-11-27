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

    // ============================================================
    // アイテム追加
    // ============================================================
    public void AddItem(ItemData item)
    {
        if (item == null) return;

        // 空きスロットに入れる
        foreach (var slot in slots)
        {
            if (!slot.HasItem())
            {
                slot.SetItem(item);
                return;
            }
        }

        // 空きがなければ新規スロット生成
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

    // ============================================================
    // 空のスロットを削除（合成後に実行）
    // ============================================================
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

    // ============================================================
    // 指定アイテムを削除（InventoryManager → UI）
    // ============================================================
    public void RemoveItem(ItemData item)
    {
        foreach (var slot in slots)
        {
            if (slot.HasItem() && slot.GetCurrentItem() == item)
            {
                slot.SetItem(null);
                RemoveEmptySlots();
                return;
            }
        }
    }

    // ============================================================
    // 全スロットを指定アイテムリストで再構築（同期）
    // ============================================================
    public void Refresh(List<ItemData> allItems)
    {
        // すべて削除
        foreach (var slot in slots)
            Destroy(slot.gameObject);

        slots.Clear();

        // 現在のアイテムリストで作り直し
        foreach (var item in allItems)
            AddItem(item);
    }

    // ============================================================
    // すべて空にする（使わなくてもOK）
    // ============================================================
    public void ClearAllSlots()
    {
        foreach (var slot in slots)
        {
            slot.SetItem(null);
        }
        RemoveEmptySlots();
    }

    // （任意）外部アクセス用
    public List<InventorySlotUI> GetAllSlots() => slots;
}
