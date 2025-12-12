using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 3f; // ← 前は1fとかなら広げる
    public LayerMask itemLayer;
    public Camera playerCamera;
    public AudioSource get_audio;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, itemLayer))
        {
            var itemPickup = hit.collider.GetComponent<ItemPickUp>();
            if (itemPickup != null)
            {
                get_audio.Play();
                itemPickup.PickUp();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCamera == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactRange);
    }
}
