using UnityEngine;

public class DateObjectBehaviour : MonoBehaviour
{
    public Renderer targetRenderer;
    public DayChangeData[] changes;

    private DayChangeData currentData;
    private Transform player;

    // -----------------------------
    // ★★ 追加機能：インタラクト設定 ★★
    // -----------------------------
    [Header("インタラクト設定（追加機能）")]
    public bool enableInteract = false;

    [Tooltip("Eキーで消去する対象オブジェクト")]
    public GameObject destroyTarget;

    [Tooltip("Eキーで生成するプレハブ（未設定なら消去のみ）")]
    public GameObject spawnPrefab;

    [Tooltip("生成位置（未設定ならこのオブジェクトの位置）")]
    public Transform spawnPoint;

    [Tooltip("インタラクトに必要なアイテム")]
    public ItemData[] requiredItems;

    [Tooltip("Eキーで判定する距離（0なら distanceRange を使用）")]
    public float interactDistance = 0f;
    // -----------------------------


    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;

        if (GameDateManager.Instance != null)
            GameDateManager.Instance.OnDateChanged += CheckDate;

        CheckDate(); // 初回適用（changes を使わない場合は currentData = null のままになる）
    }

    void OnEnable()
    {
        if (GameDateManager.Instance != null)
            GameDateManager.Instance.OnDateChanged += CheckDate;
    }

    void OnDisable()
    {
        if (GameDateManager.Instance != null)
            GameDateManager.Instance.OnDateChanged -= CheckDate;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        // ------------------------------------------
        // ★★ 日付と関係ある部分（既存機能） ★★
        // ------------------------------------------
        if (currentData != null && targetRenderer != null)
        {
            if (dist <= currentData.distanceRange && currentData.distanceTexture != null)
                targetRenderer.material.mainTexture = currentData.distanceTexture;
            else if (currentData.newTexture != null)
                targetRenderer.material.mainTexture = currentData.newTexture;
        }

        // ------------------------------------------
        // ★★ 日付と関係ない「追加機能」 ★★
        // ------------------------------------------
        if (enableInteract)
            CheckInteract(dist);
    }


    // ======================================================
    // 日付が変わった時の処理（変更なし）
    // ======================================================
    void CheckDate()
    {
        currentData = null;

        if (GameDateManager.Instance == null || changes == null) return;

        int day = GameDateManager.Instance.day;

        foreach (var c in changes)
        {
            if (c.day == day)
            {
                currentData = c;

                transform.position = c.newPosition;
                gameObject.SetActive(c.active);

                if (targetRenderer != null && c.newTexture != null)
                    targetRenderer.material.mainTexture = c.newTexture;

                break;
            }
        }
    }


    // ======================================================
    // ★ 追加機能：距離内＆アイテム所持 & Eキー → 消去 / 消去+生成
    // ======================================================
    // ======================================================
    // ★ 追加機能：距離内＆アイテム所持 & Eキー → 消去 / 消去+生成
    // ======================================================
    void CheckInteract(float currentDistance)
    {
        var inv = InventoryManager.Instance;
        if (inv == null) return;

        // 判定距離（設定されてなければ 2m）
        float range =
            (interactDistance > 0f)
            ? interactDistance
            : currentData != null ? currentData.distanceRange : 2f;

        if (currentDistance > range) return;

        // Eキー押し
        if (!Input.GetKeyDown(KeyCode.E)) return;

        // --- 必要アイテムチェック（消費しない）---
        foreach (var item in requiredItems)
        {
            if (!inv.HasItem(item))
            {
                Debug.Log("必要アイテムが不足 → 実行不可: " + item.itemName);
                return;
            }
        }

        // ======== ここから メイン処理 ========

        // --- ターゲット消去（必ず実行）---
        if (destroyTarget != null)
        {
            Destroy(destroyTarget);
            Debug.Log("指定オブジェクトを消去: " + destroyTarget.name);
        }

        // --- 新オブジェクト生成（未指定ならスキップ）---
        if (spawnPrefab != null)
        {
            Vector3 pos = (spawnPoint != null) ? spawnPoint.position : transform.position;
            Instantiate(spawnPrefab, pos, Quaternion.identity);
            Debug.Log("新オブジェクト生成: " + spawnPrefab.name);
        }
    }
}

