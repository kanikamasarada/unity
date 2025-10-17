using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventoryUI inventoryUI;
    public PauseMenu pauseMenu;
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
        // PauseMenu が開いていたら Tab 無効化
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
        if (inventoryUI != null)
            inventoryUI.gameObject.SetActive(isOpen);

        UpdateGamePauseState();
    }

    private void UpdateGamePauseState()
    {
        bool shouldPause = isOpen || (pauseMenu != null && pauseMenu.IsPaused);

        // マウスカーソル
        Cursor.visible = shouldPause;
        Cursor.lockState = shouldPause ? CursorLockMode.None : CursorLockMode.Locked;

        // 世界停止
        Time.timeScale = shouldPause ? 0f : 1f;

        // プレイヤー移動停止
        var playerMove = FindFirstObjectByType<PlayerMovement>();
        if (playerMove != null)
            playerMove.enabled = !shouldPause;

        var rb = FindFirstObjectByType<Rigidbody>();
        if (rb != null)
            rb.isKinematic = shouldPause;
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
