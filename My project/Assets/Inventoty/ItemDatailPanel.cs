using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailPanel : MonoBehaviour
{
    public static ItemDetailPanel Instance;

    [Header("UI参照")]
    public GameObject panelRoot; // ← 詳細パネルの親
    public Image itemIcon;
    public TextMeshProUGUI itemName_TMP;
    public TextMeshProUGUI itemDesc_TMP;
    public Button equipButton;

    private ItemData currentItem;

void Awake()
{
    Instance = this;
    gameObject.SetActive(false); // ← panelRoot削除
}

public void ShowItem(ItemData item)
{
    if (item == null) return;
    currentItem = item;

    if (itemIcon != null) itemIcon.sprite = item.icon;
    if (itemName_TMP != null) itemName_TMP.text = item.itemName;
    if (itemDesc_TMP != null) itemDesc_TMP.text = item.description;

    gameObject.SetActive(true);

    if (equipButton != null)
    {
        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(EquipItem);
    }
}

public void Hide()
{
    gameObject.SetActive(false);
}

    private void EquipItem()
    {
        if (currentItem == null) return;

        Debug.Log($"装備中: {currentItem.itemName}");
        PlayerItemHolder.Instance?.EquipItem(currentItem);
    }
}
