using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera playerCam;
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    public InventoryManager inventory;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("E pressed"); // ← デバッグ確認用

            Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                Debug.Log("Hit: " + hit.collider.name);

                var interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null && interactable.item != null)
                {
                    inventory.AddItem(interactable.item);
                    interactable.OnInteract();
                }
            }
        }
    }
}
