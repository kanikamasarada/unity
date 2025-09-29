using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public Transform slotsParent;
    public GameObject slotPrefab;

    public Image detailIcon;      // 右上の白い画像
    public TMP_Text detailName;   // "raita-"のテキスト
    public TMP_Text detailDesc;   // アイテム説明テキスト

    private void Awake()
    {
        instance = this;
    }

    public void AddItem(Item item)
    {
        foreach (Transform child in slotsParent)
        {
            InventorySlot slot = child.GetComponent<InventorySlot>();
            if (slot != null && slot.icon.sprite == null)
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    public void ShowItemDetail(Item item)
    {
        detailIcon.sprite = item.icon;
        detailName.text = item.itemName;
        detailDesc.text = item.description;
    }
}
