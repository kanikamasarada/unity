using UnityEngine;
using System.Collections.Generic;

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

    // ---------------------------------------------
    // ▼ ここはあなたの元のコード（そのまま）
    // ---------------------------------------------
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

    // ---------------------------------------------
    // ▼ ここから下が「追記」部分（合成機能）
    // ---------------------------------------------
    private Dictionary<(string, string), ItemData> comboTable = new();

    void Start()
    {
        // 合成ルール登録（例）
        // Resources/Items/StrongHammer.asset を作ったら有効にできる
        comboTable.Add(("kanaduti", "kagi"), Resources.Load<ItemData>("Items/StrongHammer"));
    }

    public void TryCombineItems(InventorySlotUI slotA, InventorySlotUI slotB)
    {
        var itemA = GetPrivateItem(slotA);
        var itemB = GetPrivateItem(slotB);

        if (itemA == null || itemB == null) return;

        if (comboTable.TryGetValue((itemA.itemName, itemB.itemName), out var resultItem) ||
            comboTable.TryGetValue((itemB.itemName, itemA.itemName), out resultItem))
        {
            // 合成成功
            Debug.Log($"✅ 合成成功！ → {resultItem.itemName}");

            // 元アイテム削除・合成結果に置き換え
            slotA.SetItem(null);
            slotB.SetItem(resultItem);
        }
        else
        {
            Debug.Log("❌ 合成できませんでした");
        }
    }

    /// <summary>
    /// InventorySlotUI の private な currentItem を安全に取得
    /// </summary>
    private ItemData GetPrivateItem(InventorySlotUI slot)
    {
        var field = typeof(InventorySlotUI).GetField("currentItem",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return field?.GetValue(slot) as ItemData;
    }
}
