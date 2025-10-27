using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData itemData; // ScriptableObject のデータを設定するだけ
   public void PickUp()
{
    InventoryManager.Instance.AddItem(itemData);

    if (PickupMessage.Instance != null)
        PickupMessage.Instance.ShowMessage(itemData.itemName); // ← 追加

    Destroy(gameObject);
}



}
