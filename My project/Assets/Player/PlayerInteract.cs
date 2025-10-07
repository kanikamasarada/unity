using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            // Eキーを押した瞬間だけ反応するように
            if (Input.GetKeyDown(KeyCode.E))
            {
                ItemPickUp pickUp = other.GetComponent<ItemPickUp>();
                if (pickUp != null)
                {
                    InventoryManager.Instance.AddItem(pickUp.itemData);
                    Destroy(other.gameObject);
                }
            }
        }
    }
}
