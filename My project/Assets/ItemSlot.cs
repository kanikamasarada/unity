using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image background;   // スロット枠（白い四角）
    public Image icon;         // アイコンImage

    private Item currentItem;

    // アイテムをセット
    public void SetItem(Item item)
    {
        currentItem = item;

        if (item != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }

    // クリックで取り出すとかに使える
    public Item GetItem()
    {
        return currentItem;
    }
}
