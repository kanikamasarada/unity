using UnityEngine;
using UnityEngine.Events;

public class InteractEventTrigger : MonoBehaviour
{
    [Header("Eキーで実行（必要アイテムを持っている時の初回のみ）")]
    public UnityEvent onInteract;

    [Header("2回目以降のインタラクトイベント")]
    public UnityEvent onInteractAfter;

    [Header("必要アイテムが無い場合に実行されるイベント（初回のみ）")]
    public UnityEvent onMissingItem;

    [Header("実行に必要なアイテム（初回のみ判定）")]
    public ItemData[] requiredItems;

    [Header("初回イベント後に削除されるアイテム")]
    public ItemData[] consumeItems;

    [Header("初回イベント後に追加されるアイテム")]
    public ItemData[] rewardItems;

    [Header("プレイヤーとのインタラクト距離")]
    public float interactDistance = 2f;

    private Transform player;

    // 初回実行済みフラグ
    private bool hasInteractedOnce = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
            Debug.LogError("Player に Tag 'Player' を設定してください");
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > interactDistance) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        var inv = InventoryManager.Instance;
        if (inv == null)
        {
            Debug.LogError("InventoryManager がシーンにありません");
            return;
        }

        // -----------------------------------------------------
        // ⭐ 2回目以降 → 必要アイテムチェックせず、onInteractAfter だけ実行
        // -----------------------------------------------------
        if (hasInteractedOnce)
        {
            Debug.Log("2回目以降 → onInteractAfter 実行");
            onInteractAfter?.Invoke();
            return;
        }

        // -----------------------------------------------------
        // ⭐ 初回 → 必要アイテムチェックを行う
        // -----------------------------------------------------
        foreach (var item in requiredItems)
        {
            if (!inv.HasItem(item))
            {
                Debug.Log("必要アイテムなし → onMissingItem 実行: " + item.itemName);
                onMissingItem?.Invoke();
                return;
            }
        }

        // -----------------------------------------------------
        // ⭐ 初回イベント実行
        // -----------------------------------------------------
        Debug.Log("初回 → onInteract 実行");
        onInteract?.Invoke();

        // フラグ ON → ここからは requiredItems を無視
        hasInteractedOnce = true;

        // 必要アイテム消費
        foreach (var item in consumeItems)
        {
            inv.RemoveItem(item);
        }

        // 報酬アイテム追加
        foreach (var item in rewardItems)
        {
            inv.AddItem(item);
        }

        Debug.Log("初回インタラクト完了 → アイテム更新＆イベント切替完了");
    }
}
