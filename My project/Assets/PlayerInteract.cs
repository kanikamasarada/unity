using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera playerCam;
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    public InventoryManager inventory;

    void Update()   // ✅ 引数なし
    {
        if (Input.GetKeyDown(interactKey))
        {
            Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                var item = hit.collider.GetComponent<Interactable>();
                if (item != null)
                {
                    inventory.AddItem(item);
                    item.OnInteract();
                }
            }
        }
    }
}
