using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [Header("UI参照")]
    public Image iconImage;

    private ItemData currentItem;

    void Start()
    {
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnClick);
        }
    }

    public bool HasItem() => currentItem != null;
    public ItemData GetCurrentItem() => currentItem;

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

        ItemDetailPanel.Instance?.ShowItem(currentItem);
        InventoryManager.Instance?.SetLastSelectedItem(currentItem);
    }
}
