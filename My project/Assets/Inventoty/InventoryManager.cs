using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI参照")]
    public InventoryUI inventoryUI;
    public PauseMenu pauseMenu;

    private bool isOpen = false;
    public bool IsOpen => isOpen;

    private PlayerMovement playerMovement; // ← プレイヤー動作スクリプト参照

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // PlayerMovement を探す
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    void Start()
    {
        if (inventoryUI != null)
            inventoryUI.gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (pauseMenu != null && pauseMenu.IsPaused)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;

        if (inventoryUI != null)
            inventoryUI.gameObject.SetActive(isOpen);

        if (isOpen)
        {
            // 🔹 開いたとき：完全停止
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (playerMovement != null) playerMovement.enabled = false;

            Debug.Log("🟡 Inventory 開いた → 完全停止");
        }
        else
        {
            // 🔹 閉じたとき：再開
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (playerMovement != null) playerMovement.enabled = true;

            Debug.Log("🟢 Inventory 閉じた → 再開");

            if (ItemDetailPanel.Instance != null)
                ItemDetailPanel.Instance.Hide();
        }
    }

    public void AddItem(ItemData item)
    {
        if (inventoryUI != null)
        {
            inventoryUI.AddItem(item);
        }
        else
        {
            Debug.LogWarning("⚠ InventoryUI が未設定！");
        }
    }
}
