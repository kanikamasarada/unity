using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image iconImage;
    public Button button;
    public ItemData currentItem;

    [Header("UI参照")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescText;
    public Button equipButton;

    void Start()
    {
        if (button != null)
            button.onClick.AddListener(OnClickSlot);
    }

    public void SetItem(ItemData item)
    {
        currentItem = item;
        iconImage.sprite = item.icon;
        iconImage.enabled = true;
    }

    public bool HasItem() => currentItem != null;

    private void OnClickSlot()
    {
        if (currentItem == null) return;
        ItemDetailPanel.Instance.ShowItem(currentItem);

        // 説明更新
        itemNameText.text = currentItem.itemName;
        itemDescText.text = currentItem.description;

        // 装備ボタン更新
        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(() => EquipItem());
    }

    private void EquipItem()
    {
        if (currentItem == null) return;
        Debug.Log($"装備中: {currentItem.itemName}");

        // PlayerItemHolderに通知
        PlayerItemHolder.Instance.EquipItem(currentItem);
    }
}
