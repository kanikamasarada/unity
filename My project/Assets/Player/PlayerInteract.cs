using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item") && Input.GetKeyDown(KeyCode.E))
        {
            ItemPickUp pickUp = other.GetComponent<ItemPickUp>();
            if (pickUp != null)
            {
                InventoryManager.Instance.AddItem(pickUp.item);
                Destroy(other.gameObject);
            }
        }
    }
}
