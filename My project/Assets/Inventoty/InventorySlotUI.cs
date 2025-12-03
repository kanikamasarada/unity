using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image iconImage;
    public ItemData currentItem;

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

    public void SetItem(ItemData item)
    {
        currentItem = item;

        if (iconImage != null)
        {
            if(item != null)
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

    public ItemData GetCurrentItem() => currentItem;

private void OnClick()
{
    if(currentItem == null) return;

    // UI だけ反映
    ItemDetailPanel.Instance?.ShowItem(currentItem);
    InventoryManager.Instance?.SetLastSelectedItem(currentItem);

    // 装備はここで一括管理
    EquipmentUI.Instance?.SetEquippedItem(currentItem);
    PlayerEquipmentUIConnector.Instance?.EquipFromUI(currentItem);
}
}
