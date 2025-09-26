using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Transform slotParent;   // InventoryPanel の子にある ItemSlot 群
    private ItemSlot[] slots;

    void Start()
    {
        slots = slotParent.GetComponentsInChildren<ItemSlot>();
    }

    public void AddItem(Item newItem)
    {
        foreach (var slot in slots)
        {
            if (slot.GetItem() == null)
            {
                slot.SetItem(newItem);
                Debug.Log(newItem.itemName + " を追加しました");
                return;
            }
        }
        Debug.Log("インベントリがいっぱい！");
    }
}
