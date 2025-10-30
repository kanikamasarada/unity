using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailPanel : MonoBehaviour
{
    public static ItemDetailPanel Instance;

    [Header("UI 参照")]
    public Image itemIcon;

    [Header("テキスト（どちらか使用）")]
    public TextMeshProUGUI itemNameText_TMP;
    public TextMeshProUGUI itemDescText_TMP;
    public Text itemNameText_Legacy;
    public Text itemDescText_Legacy;

    [Header("ボタン設定")]
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
        if (item == null) return;

        currentItem = item;

        // アイコン表示
        if (itemIcon != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.enabled = true;
        }

        // テキスト更新（TMP → Legacy の順に確認）
        if (itemNameText_TMP != null)
            itemNameText_TMP.text = item.itemName;
        if (itemDescText_TMP != null)
            itemDescText_TMP.text = item.description;

        if (itemNameText_Legacy != null)
            itemNameText_Legacy.text = item.itemName;
        if (itemDescText_Legacy != null)
            itemDescText_Legacy.text = item.description;

        gameObject.SetActive(true);
    }

    void OnEquipButton()
    {
        if (currentItem == null) return;

        if (PlayerItemHolder.Instance != null)
        {
            PlayerItemHolder.Instance.EquipItem(currentItem);
        }
        else
        {
            Debug.LogWarning("PlayerItemHolder.Instance が見つかりません。");
        }
    }
}
