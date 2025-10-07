using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventoryUI inventoryUI;
    public PauseMenu pauseMenu;  // ← PauseMenuスクリプト参照
    private bool isOpen = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        // PauseMenu が開いてたら（isPaused = true）Tab 無効化
        if (pauseMenu != null && pauseMenu.IsPaused)
            return;

        // Tabキーでインベントリ開閉
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryUI.gameObject.SetActive(isOpen);

        // 開いた時、マウスカーソルを出す
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void AddItem(ItemData item)
    {
        if (inventoryUI != null)
            inventoryUI.AddItem(item);
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
