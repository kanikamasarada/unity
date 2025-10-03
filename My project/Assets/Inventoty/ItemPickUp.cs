using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData item;  // このアイテムが持つデータ

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            InventoryManager.Instance.AddItem(item);
            Destroy(gameObject); // フィールドから消す
        }
    }
}
