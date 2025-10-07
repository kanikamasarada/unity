using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData itemData;  // このオブジェクトが持つアイテム情報

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            // アイテムをインベントリに追加
            InventoryManager.Instance.AddItem(itemData);

            // 拾ったら消す
            Destroy(gameObject);
        }
    }
}
