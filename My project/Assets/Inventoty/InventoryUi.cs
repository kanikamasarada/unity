using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("スロットをまとめる親")]
    public Transform itemSlotsParent;

    [Header("スロットプレハブ（最初に非表示で置いておく）")]
    public GameObject itemSlotPrefab;

    private List<InventorySlotUI> slots = new List<InventorySlotUI>();

    void Awake()
    {
        // プレハブを非表示にする
        if (itemSlotPrefab != null)
            itemSlotPrefab.SetActive(false);

        // 🔥 ここが重要：親の子供ぶんだけスロットを作成して並べる
        InitializeFixedSlots();
    }

    private void InitializeFixedSlots()
    {
        if (itemSlotsParent == null || itemSlotPrefab == null)
        {
            Debug.LogError("❌ itemSlotPrefab または itemSlotsParent が未設定");
            return;
        }

        // すでに子がある場合 → 削除（念のため）
        foreach (Transform child in itemSlotsParent)
        {
            if (child != itemSlotPrefab.transform)
                Destroy(child.gameObject);
        }

        // 🔥 例えば 12 マス作りたい場合 → 子供の数で調整可能
        int slotCount = 12;

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, itemSlotsParent);
            slotObj.SetActive(true);

            var slotUI = slotObj.GetComponent<InventorySlotUI>();
            slotUI.SetItem(null);
            slots.Add(slotUI);
        }
    }

    // ---------- アイテムを追加（固定スロット版） ----------
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

        Debug.Log("⚠ インベントリがいっぱいです");
    }
    public void Refresh(List<ItemData> allItems)
    {
        // まず全部クリア
        foreach (var slot in slots)
            slot.SetItem(null);

        // アイテムを順番に配置
        for (int i = 0; i < allItems.Count && i < slots.Count; i++)
        {
            slots[i].SetItem(allItems[i]);
        }
    }


    // ---------- 全クリア ----------
    public void ClearAllSlots()
    {
        foreach (var slot in slots)
        {
            slot.SetItem(null);
        }
    }
    // 空スロットをリストの後ろに回して並び替える
public void RemoveEmptySlots()
{
    // ★ドラッグ中は実行しない
    if (InventoryManager.Instance != null && InventoryManager.Instance.IsDragging)
        return;

    if (slots == null || slots.Count == 0) return;

    List<ItemData> items = new List<ItemData>();

    foreach (var slot in slots)
    {
        ItemData item = slot.GetCurrentItem();
        if (item != null)
            items.Add(item);
    }

    foreach (var slot in slots)
    {
        slot.SetItem(null);
    }

    for (int i = 0; i < items.Count; i++)
    {
        slots[i].SetItem(items[i]);
    }
}


}
