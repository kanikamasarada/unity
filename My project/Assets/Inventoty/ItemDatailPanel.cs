using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailPanel : MonoBehaviour
{
    public static ItemDetailPanel Instance;

    [Header("UI 参照")]
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescText;
    public Button equipButton;

    private ItemData currentItem;

    void Awake()
    {
        Instance = this;
        if (equipButton != null)
            equipButton.onClick.AddListener(OnEquipButton);
    }

    public void ShowItem(ItemData item)
    {
        currentItem = item;
        itemIcon.sprite = item.icon;
        itemIcon.enabled = true;
        itemNameText.text = item.itemName;
        itemDescText.text = item.description;
        gameObject.SetActive(true);
    }

    void OnEquipButton()
    {
        if (currentItem == null) return;
        PlayerItemHolder.Instance.EquipItem(currentItem);
    }
}
