using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;       // スロットのアイコン
    private Item item;       // このスロットに入っているアイテム

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;  // アイコン画像をセット
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    // クリックされたら詳細表示更新
    public void OnSlotClicked()
    {
        if (item != null)
        {
            InventoryManager.instance.ShowItemDetail(item);
        }
    }
}
