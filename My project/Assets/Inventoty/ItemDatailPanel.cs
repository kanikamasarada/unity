using UnityEngine;
using UnityEngine.UI;
public class ItemDetailPanel : MonoBehaviour
{
    public static ItemDetailPanel Instance;
    [Header("UI参照")]
    public GameObject panelRoot;
    public Image itemIcon;
    public Text itemName_Legacy;
    public Text itemDesc_Legacy;
    public Button equipButton;
    private ItemData currentItem;
    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    public void ShowItem(ItemData item)
    {
        if (item == null) return;
        currentItem = item;
        if (itemIcon != null) itemIcon.sprite = item.icon;
        if (itemName_Legacy != null) itemName_Legacy.text = item.itemName;
        if (itemDesc_Legacy != null) itemDesc_Legacy.text = item.description;
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
