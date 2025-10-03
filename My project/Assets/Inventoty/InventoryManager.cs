using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public InventoryUI inventoryUI;

    private bool isOpen = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        // Tabキーで開閉
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            inventoryUI.gameObject.SetActive(isOpen);
        }
    }

    public void AddItem(ItemData item)
    {
        inventoryUI.AddItem(item);
    }
}
