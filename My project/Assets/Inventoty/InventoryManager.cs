using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI参照")]
    public InventoryUI inventoryUI;
    public PauseMenu pauseMenu;

    private bool isOpen = false;
    public bool IsOpen => isOpen;

    public ItemData LastSelectedItem { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        // ポーズ中はインベントリ開閉を無効化
        if (pauseMenu != null && pauseMenu.IsPaused)
            return;

        // Tabでインベントリ開閉
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

        // 閉じたら詳細パネル非表示
        if (!isOpen && ItemDetailPanel.Instance != null)
            ItemDetailPanel.Instance.Hide();
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

        // Rigidbody停止
        var rb = FindFirstObjectByType<Rigidbody>();
        if (rb != null)
            rb.isKinematic = shouldPause;
    }

    public void AddItem(ItemData item)
    {
        if (inventoryUI != null)
            inventoryUI.AddItem(item);
    }

    public void SetLastSelectedItem(ItemData item)
    {
        LastSelectedItem = item;
    }
}
