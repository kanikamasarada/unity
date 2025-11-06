using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [Header("アイテム情報")]
    public Image iconImage;
    public Button button;
    public ItemData currentItem;

    [Header("UI参照（どちらか使用）")]
    public TextMeshProUGUI itemNameText_TMP;
    public TextMeshProUGUI itemDescText_TMP;
    public Text itemNameText_Legacy;
    public Text itemDescText_Legacy;

    void Start()
    {
        if (button != null)
            button.onClick.AddListener(OnClickSlot);
    }

public void SetItem(ItemData item)
{
    if (item == null) return;

    // すでにアイテムが入っているスロットなら何もしない
    if (currentItem != null) return;

    currentItem = item;

    if (iconImage != null)
    {
        iconImage.sprite = item.icon;
        iconImage.enabled = true;
    }
}

    public bool HasItem() => currentItem != null;

    private void OnClickSlot()
    {
        if (currentItem == null) return;

        // 選択中のアイテムをItemDetailPanelに送る
        if (ItemDetailPanel.Instance != null)
            ItemDetailPanel.Instance.ShowItem(currentItem);

        // --- 名前表示 ---
        if (itemNameText_TMP != null)
            itemNameText_TMP.text = currentItem.itemName;
        if (itemNameText_Legacy != null)
            itemNameText_Legacy.text = currentItem.itemName;

        // --- 説明表示 ---
        if (itemDescText_TMP != null)
            itemDescText_TMP.text = currentItem.description;
        if (itemDescText_Legacy != null)
            itemDescText_Legacy.text = currentItem.description;

       if (SelectedItemDisplay.Instance != null)
        SelectedItemDisplay.Instance.ShowItem(currentItem);
    }
}
