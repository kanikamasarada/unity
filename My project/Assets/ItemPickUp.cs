using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item; // インスペクタで設定（Swordとか）

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 新しい推奨方法
            InventoryManager inv = Object.FindFirstObjectByType<InventoryManager>();
            if (inv != null)
            {
                inv.AddItem(item);
                Destroy(gameObject); // 拾ったアイテムを消す
            }
        }
    }
}
