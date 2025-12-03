using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [Header("UI参照")]
    public InventoryUI inventoryUI;
    public PauseMenu pauseMenu;
    private bool isOpen = false;
    public bool IsOpen => isOpen;

    public bool IsDragging { get; set; }

    public ItemData LastSelectedItem { get; private set; }
    // 旧：固定レシピ
    private Dictionary<(string, string), ItemData> comboTable = new();
    // 新：インスペクター設定レシピ
    private Dictionary<(string, string), ItemData> dynamicComboTable = new();
    private Dictionary<(string, string), UnityEvent> dynamicEventTable = new();
    // ★追加アイテム
    private Dictionary<(string, string), ItemData[]> bonusItemTable = new();
    // ★所持アイテムリスト
    private List<ItemData> itemList = new();
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Update()
    {
        if (pauseMenu != null && pauseMenu.IsPaused) return;
        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleInventory();
    }
    public void ToggleInventory()
    {
        isOpen = !isOpen;
        if (inventoryUI != null)
            inventoryUI.gameObject.SetActive(isOpen);
        UpdateGamePauseState();
        if (!isOpen && ItemDetailPanel.Instance != null)
            ItemDetailPanel.Instance.Hide();
    }
    private void UpdateGamePauseState()
    {
        bool shouldPause = isOpen || (pauseMenu != null && pauseMenu.IsPaused);
        Cursor.visible = shouldPause;
        Cursor.lockState = shouldPause ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = shouldPause ? 0f : 1f;
        var playerMove = FindFirstObjectByType<PlayerMovement>();
        if (playerMove != null) playerMove.enabled = !shouldPause;
        var rb = FindFirstObjectByType<Rigidbody>();
        if (rb != null) rb.isKinematic = shouldPause;
    }
    // =============================================================
    // 所持アイテムの管理
    // =============================================================
    /// 所持アイテムを追加
    public void AddItem(ItemData item)
    {
        if (item == null) return;
        itemList.Add(item);
        if (inventoryUI != null)
            inventoryUI.AddItem(item);
    }
    /// 複数追加（インタラクトの報酬用）
    public void AddItems(ItemData[] items)
    {
        if (items == null) return;
        foreach (var item in items)
            AddItem(item);
    }
    /// アイテム所持判定
    public bool HasItem(ItemData item)
    {
        return itemList.Contains(item);
    }
    /// 複数アイテム所持判定（全部必要）
    public bool HasItems(ItemData[] items)
    {
        if (items == null) return true;
        foreach (var item in items)
        {
            if (!itemList.Contains(item))
                return false;
        }
        return true;
    }
    /// アイテム削除
    public void RemoveItem(ItemData item)
    {
        if (item == null) return;
        if (itemList.Contains(item))
            itemList.Remove(item);
        if (inventoryUI != null)
            inventoryUI.Refresh(itemList);
    }
    /// 複数削除
    public void RemoveItems(ItemData[] items)
    {
        if (items == null) return;
        foreach (var item in items)
            RemoveItem(item);
    }
    public void SetLastSelectedItem(ItemData item)
    {
        LastSelectedItem = item;
    }
    // =============================================================
    // レシピ登録（CombineRecipeEvent.cs から呼ばれる）
    // =============================================================
    public void RegisterCombineEventRecipe(
        ItemData a,
        ItemData b,
        ItemData result,
        UnityEvent evt,
        ItemData[] bonusItems = null)
    {
        var key1 = (a.itemName, b.itemName);
        var key2 = (b.itemName, a.itemName);
        dynamicComboTable[key1] = result;
        dynamicComboTable[key2] = result;
        dynamicEventTable[key1] = evt;
        dynamicEventTable[key2] = evt;
        bonusItemTable[key1] = bonusItems;
        bonusItemTable[key2] = bonusItems;
        Debug.Log($"[登録] {a.itemName} + {b.itemName} → {(result == null ? "消去型" : result.itemName)}");
    }
    // =============================================================
    // 合成処理
    // =============================================================
    public void TryCombineItems(InventorySlotUI slotA, InventorySlotUI slotB)
    {
        if (slotA == null || slotB == null) return;
        ItemData itemA = slotA.GetCurrentItem();
        ItemData itemB = slotB.GetCurrentItem();
        if (itemA == null || itemB == null) return;
        var key = (itemA.itemName, itemB.itemName);
        // ① インスペクター設定レシピ
        if (dynamicComboTable.TryGetValue(key, out ItemData result))
        {
            Debug.Log($"合成成功（インスペクター設定）: {itemA.itemName} + {itemB.itemName}");
            if (dynamicEventTable.TryGetValue(key, out UnityEvent evt))
                evt.Invoke();
            // ★消去型合成
            if (result == null)
            {
                slotA.SetItem(null);
                slotB.SetItem(null);
                inventoryUI.RemoveEmptySlots();
                if (bonusItemTable.TryGetValue(key, out ItemData[] bonus)
                    && bonus != null && bonus.Length > 0)
                {
                    AddItems(bonus);
                }
                return;
            }
            // ★通常合成
            slotA.SetItem(null);
            slotB.SetItem(result);
            inventoryUI.RemoveEmptySlots();
            return;
        }
        // ② 旧レシピ
        if (comboTable.TryGetValue(key, out ItemData resultItem))
        {
            slotA.SetItem(null);
            slotB.SetItem(resultItem);
            inventoryUI.RemoveEmptySlots();
            return;
        }
        Debug.Log(":x_黒太字: この組み合わせは合成できません");
    }
    public void PickupItem(ItemData item)
    {
        if (item == null) return;
        AddItem(item);
    }
    // =============================================================
    // インタラクト用追加メソッド
    // =============================================================
    /// 必要アイテム1つチェック→あれば消費
    public bool TryConsumeItem(ItemData item)
    {
        if (item == null) return false;
        if (!HasItem(item))
            return false;
        RemoveItem(item);
        return true;
    }
    /// 必要アイテム複数チェック→全部持っていれば一括消費
    public bool TryConsumeItems(ItemData[] items)
    {
        if (items == null || items.Length == 0) return true;
        foreach (var item in items)
        {
            if (!itemList.Contains(item))
                return false;
        }
        foreach (var item in items)
            RemoveItem(item);
        return true;
    }
    /// 所持アイテム一覧取得
    public List<ItemData> GetAllItems()
    {
        return new List<ItemData>(itemList);
    }
}