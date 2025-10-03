using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Transform slotParent;   // スロットを並べる親 (Grid Layout Group 推奨)
    public GameObject slotPrefab;  // スロットのプレハブ

    public void Refresh(List<ItemData> items)
    {
        // 古いスロットを消す
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        // 新しいスロットを並べる
        foreach (var item in items)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);
            Image icon = slot.GetComponentInChildren<Image>();
            icon.sprite = item.icon;
        }
    }
}
