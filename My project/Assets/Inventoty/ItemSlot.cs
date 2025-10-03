using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    private Item currentItem;
    public Image iconImage;
    public TMP_Text nameText;   // アイテム名表示用（オプション）
    public TMP_Text descText;   // 説明表示用（オプション）

    public void SetItem(Item newItem)
    {
        currentItem = newItem;

        if (iconImage != null)
        {
            if (newItem != null)
            {
                iconImage.sprite = newItem.icon;
                iconImage.enabled = true;

                if (nameText != null) nameText.text = newItem.itemName;
                if (descText != null) descText.text = newItem.description;
            }
            else
            {
                iconImage.sprite = null;
                iconImage.enabled = false;

                if (nameText != null) nameText.text = "";
                if (descText != null) descText.text = "";
            }
        }
    }

    public Item GetItem()
    {
        return currentItem;
    }
}
