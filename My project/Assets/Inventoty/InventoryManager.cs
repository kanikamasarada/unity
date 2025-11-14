using System.Collections.Generic;
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
    // 合成テーブル
    private Dictionary<(string, string), ItemData> comboTable = new();
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        // 合成ルール登録
        comboTable.Add(("kanaduti", "kagi"), Resources.Load<ItemData>("Items/StrongHammer"));
    }
    void Update()
    {
        if (pauseMenu != null && pauseMenu.IsPaused) return;
        if (Input.GetKeyDown(KeyCode.Tab)) ToggleInventory();
    }
    public void ToggleInventory()
    {
        isOpen = !isOpen;
        if (inventoryUI != null) inventoryUI.gameObject.SetActive(isOpen);
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
        if (inventoryUI != null) inventoryUI.AddItem(item);
    }
    public void SetLastSelectedItem(ItemData item)
    {
        LastSelectedItem = item;
    }
    // -----------------------
    // 合成処理
    // -----------------------
    public void TryCombineItems(InventorySlotUI slotA, InventorySlotUI slotB)
    {
        if (slotA == null || slotB == null) return;
        ItemData itemA = slotA.GetCurrentItem();
        ItemData itemB = slotB.GetCurrentItem();
        if (itemA == null || itemB == null) return;
        if (comboTable.TryGetValue((itemA.itemName, itemB.itemName), out ItemData resultItem) ||
            comboTable.TryGetValue((itemB.itemName, itemA.itemName), out resultItem))
        {
            Debug.Log($":チェックマーク_緑: 合成成功: {itemA.itemName} + {itemB.itemName} → {resultItem.itemName}");
            slotA.SetItem(null);
            slotB.SetItem(resultItem);
            inventoryUI.RemoveEmptySlots();
            // 見た目の武器を更新
            var playerEquip = FindObjectOfType<PlayerEquipment>();
            if (playerEquip != null)
            {
                playerEquip.Equip(resultItem);
                Debug.Log($":斧: プレイヤーが {resultItem.itemName} を装備しました");
            }
        }
        else
        {
            Debug.Log(":x: この組み合わせは合成できません");
        }
    }
    public void PickupItem(ItemData item)
    {
        if (item == null) return;
        if (inventoryUI != null) inventoryUI.AddItem(item);
    }
}
