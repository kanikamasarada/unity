using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailPanel : MonoBehaviour
{
    public static ItemDetailPanel Instance;

    [Header("UI参照")]
    public Image itemIcon;
    public TextMeshProUGUI itemName_TMP;
    public TextMeshProUGUI itemDesc_TMP;
    public Text itemName_Legacy;
    public Text itemDesc_Legacy;
    public Button equipButton;

    private ItemData selectedItem;

    void Awake()
    {
        Instance = this;
    }

    public void ShowItem(ItemData item)
    {
        selectedItem = item;

        if (itemIcon != null)
            itemIcon.sprite = item.icon;

        if (itemName_TMP != null)
            itemName_TMP.text = item.itemName;
        if (itemDesc_TMP != null)
            itemDesc_TMP.text = item.description;

        if (itemName_Legacy != null)
            itemName_Legacy.text = item.itemName;
        if (itemDesc_Legacy != null)
            itemDesc_Legacy.text = item.description;

        gameObject.SetActive(true);

        // 装備ボタンのイベント登録
        if (equipButton != null)
        {
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(EquipItem);
        }
    }

    private void EquipItem()
    {
        if (selectedItem == null) return;

        Debug.Log($"装備中: {selectedItem.itemName}");
        PlayerItemHolder.Instance?.EquipItem(selectedItem);
    }
}
