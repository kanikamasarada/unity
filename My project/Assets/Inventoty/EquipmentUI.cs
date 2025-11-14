using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public static EquipmentUI Instance;

    [Header("装備アイテム表示")]
    public Image equippedIcon;

    private ItemData equippedItem;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetEquippedItem(ItemData item)
    {
        equippedItem = item;

        if (equippedIcon != null)
        {
            if (item != null)
            {
                equippedIcon.sprite = item.icon;
                equippedIcon.enabled = true;
            }
            else
            {
                equippedIcon.sprite = null;
                equippedIcon.enabled = false;
            }
        }
    }

    public ItemData GetEquippedItem() => equippedItem;
}
