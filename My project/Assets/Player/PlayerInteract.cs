using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 3f;
    public Camera cam;

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                Debug.Log("Hit: " + hit.collider.name);

                ItemPickUp pickUp = hit.collider.GetComponent<ItemPickUp>();
                if (pickUp != null)
                {
                    Debug.Log("Item Picked Up: " + pickUp.item.itemName);
                    InventoryManager.instance.AddItem(pickUp.item);

                    // アイテム本体だけ消す
                    Destroy(pickUp.gameObject);
                }
            }
        }
    }
}
