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

    public ItemData LastSelectedItem { get; private set; }

    // 旧：固定レシピ（必要なら残す）
    private Dictionary<(string, string), ItemData> comboTable = new();

    // 新：インスペクター設定レシピ
    private Dictionary<(string, string), ItemData> dynamicComboTable = new();
    private Dictionary<(string, string), UnityEvent> dynamicEventTable = new();

    // ★追加アイテム（消去型合成用）
    private Dictionary<(string, string), ItemData[]> bonusItemTable = new();

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

    public void AddItem(ItemData item)
    {
        if (inventoryUI != null)
            inventoryUI.AddItem(item);
    }

    public void SetLastSelectedItem(ItemData item)
    {
        LastSelectedItem = item;
    }

    // =============================================================
    // レシピ登録（CombineRecipeEvent.cs から呼ばれる）
    // =============================================================
    public void RegisterCombineEventRecipe(ItemData a, ItemData b, ItemData result, UnityEvent evt, ItemData[] bonusItems = null)
    {
        var key1 = (a.itemName, b.itemName);
        var key2 = (b.itemName, a.itemName);

        dynamicComboTable[key1] = result;
        dynamicComboTable[key2] = result;

        dynamicEventTable[key1] = evt;
        dynamicEventTable[key2] = evt;

        bonusItemTable[key1] = bonusItems;
        bonusItemTable[key2] = bonusItems;

        Debug.Log($"[登録] {a.itemName} + {b.itemName} → {(result == null ? "消去型" : result.itemName)} / 追加アイテム: {(bonusItems == null ? "なし" : bonusItems.Length + "個")}");
    }

    // =============================================================
    // 合成処理（ドラッグ時に呼ばれる）
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
            {
                evt.Invoke();
                Debug.Log("合成イベント実行");
            }

            // ★合成後アイテムなし → 消去 + 追加アイテム処理
            if (result == null)
            {
                slotA.SetItem(null);
                slotB.SetItem(null);
                inventoryUI.RemoveEmptySlots();
                Debug.Log("結果アイテム無し → 消去型合成");

                // ★追加アイテム付与
                if (bonusItemTable.TryGetValue(key, out ItemData[] bonus)
                    && bonus != null && bonus.Length > 0)
                {
                    foreach (var item in bonus)
                    {
                        AddItem(item);
                        Debug.Log($"追加アイテム：{item.itemName}");
                    }
                }
                return;
            }

            // ★通常合成
            slotA.SetItem(null);
            slotB.SetItem(result);
            inventoryUI.RemoveEmptySlots();
            return;
        }

        // ② 旧 comboTable（任意で残す）
        if (comboTable.TryGetValue(key, out ItemData resultItem))
        {
            Debug.Log($"合成成功（旧）: {itemA.itemName} + {itemB.itemName} → {resultItem.itemName}");

            slotA.SetItem(null);
            slotB.SetItem(resultItem);
            inventoryUI.RemoveEmptySlots();
            return;
        }

        Debug.Log("✖ この組み合わせは合成できません");
    }

    public void PickupItem(ItemData item)
    {
        if (item == null) return;
        AddItem(item);
    }
}
