using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [Header("UI参照")]
    public Image iconImage;
    private ItemData currentItem;

    void Start()
    {
        // ボタンがついていればクリックイベントを登録
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnClick);
        }
    }

    public bool HasItem() => currentItem != null;

    public void SetItem(ItemData item)
    {
        currentItem = item;

        if (iconImage != null)
        {
            if (item != null)
            {
                iconImage.sprite = item.icon;
                iconImage.enabled = true;
            }
            else
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }
        }
    }

    private void OnClick()
    {
        if (currentItem == null) return;

        // 右側の詳細パネル表示
        ItemDetailPanel.Instance?.ShowItem(currentItem);
    }
}
