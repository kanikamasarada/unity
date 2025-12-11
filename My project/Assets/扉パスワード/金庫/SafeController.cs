using UnityEngine;

/// <summary>
/// 金庫本体に付ける（距離判定・EキーでUIを開く）。UIはInspectorで割り当てる。
/// 成功時に指定のオブジェクトをDestroyする。
/// </summary>
public class SafeController : MonoBehaviour
{
    [Header("インタラクト設定")]
    public float interactDistance = 2f;
    public KeyCode openKey = KeyCode.E;

    [Header("UI (Inspectorで割当て)")]
    public GameObject safeUIPanel;           // SafeUIController を持つパネル
    public SafeUIController safeUIController; // パネルに付けたスクリプトへの参照

    [Header("金庫解除後の挙動")]
    public GameObject destroyOnSuccess;      // 成功時に消すオブジェクト（null可）

    [Header("パスワード (3枠)。Inspectorで設定してください)")]
    public string codeA = "12";
    public string codeB = "34";
    public string codeC = "56";
    public int maxDigitsPerSlot = 2;

    private Transform player;
    private bool uiOpen = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (safeUIPanel != null)
            safeUIPanel.SetActive(false);

        if (safeUIController == null && safeUIPanel != null)
            safeUIController = safeUIPanel.GetComponent<SafeUIController>();

        if (safeUIController == null)
            Debug.LogWarning("[SafeController] safeUIController がアサインされていません。Inspectorで UI パネルをセットしてください。");
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > interactDistance) return;

        if (Input.GetKeyDown(openKey) && !uiOpen)
        {
            OpenSafeUI();
        }
    }

    private void OpenSafeUI()
    {
        if (safeUIPanel == null || safeUIController == null)
        {
            Debug.LogWarning("[SafeController] UI が設定されていないため開けません。");
            return;
        }

        // UIを渡して開く
        string[] codes = new string[3] { codeA, codeB, codeC };
        safeUIController.Open(codes, maxDigitsPerSlot, OnSafeSuccess, OnSafeClosedByUser);

        // 表示
        safeUIPanel.SetActive(true);
        uiOpen = true;
    }

    // 成功時コールバック（UIから呼ばれる）
    private void OnSafeSuccess()
    {
        Debug.Log("[SafeController] パスワード正解！ 成功処理を実行します。");

        if (destroyOnSuccess != null)
        {
            Destroy(destroyOnSuccess);
            Debug.Log("[SafeController] 指定オブジェクトを削除しました: " + destroyOnSuccess.name);
        }

        CloseUIInternal();
    }

    // ユーザーが閉じた時（失敗または閉じるボタン）に呼ばれる
    private void OnSafeClosedByUser()
    {
        CloseUIInternal();
    }

    private void CloseUIInternal()
    {
        if (safeUIPanel != null)
            safeUIPanel.SetActive(false);

        uiOpen = false;
    }
}
