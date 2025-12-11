using UnityEngine;
using UnityEngine.UI;
public class PuzzleNumberCheck : MonoBehaviour
{
    [Header("UI")]
    public GameObject puzzlePanel;
    public InputField inputA;
    public InputField inputB;
    public InputField inputC;
    public InputField inputD;
    [Header("Correct Numbers")]
    public string correctA = "12";
    public string correctB = "34";
    public string correctC = "56";
    public string correctD = "00";
    [Header("Shelf Move")]
    public Transform targetObject;
    public Vector3 moveOffset = new Vector3(3f, 0, 0);
    public float moveSpeed = 2f;
    [Header("Interaction")]
    public Transform player;
    public float interactDistance = 2f;
    // 内部状態
    private bool isSolved = false;
    private bool isPanelOpen = false;
    private Vector3 originalPos;
    private Vector3 targetPos;
    // 戻す処理
    private bool hasReturned = false; // 一回戻ったかどうか
    private bool isReturning = false; // 戻り中かどうか
    private float returnDelay = 10f;  // 10秒待つ
    void Start()
    {
        originalPos = targetObject.position;
        targetPos = originalPos + moveOffset;
        puzzlePanel.SetActive(false);
    }
    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        // 距離内で E キー → UI 開閉
        if (dist <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (!isPanelOpen)
                OpenPanel();
            else
                ClosePanel();
        }
        // 正解 → 前へ移動
        if (isSolved && !hasReturned)
        {
            targetObject.position = Vector3.Lerp(
                targetObject.position,
                targetPos,
                Time.deltaTime * moveSpeed
            );
        }
        // 戻り処理
        UpdateReturning();
    }
    // ======================
    // UI 開く
    // ======================
    public void OpenPanel()
    {
        isPanelOpen = true;
        puzzlePanel.SetActive(true);
        PauseGameControls();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // ======================
    // UI 閉じる
    // ======================
    public void ClosePanel()
    {
        isPanelOpen = false;
        puzzlePanel.SetActive(false);
        ResumeGameControls();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // ======================
    // パスワード判定
    // ======================
    public void CheckNumbers()
    {
        if (hasReturned)   // 一度戻った後は無効
        {
            ClosePanel();
            return;
        }
        if (inputA.text == correctA &&
            inputB.text == correctB &&
            inputC.text == correctC &&
            inputD.text == correctD)
        {
            isSolved = true;
            // 10秒後に戻す
            Invoke("StartReturn", returnDelay);
        }
        ClosePanel();  // 決定押したらUI閉じる
    }
    // ======================
    // 戻す処理
    // ======================
    private void StartReturn()
    {
        isReturning = true;
        isSolved = false;
    }
    private void UpdateReturning()
    {
        if (!isReturning) return;
        targetObject.position = Vector3.Lerp(
            targetObject.position,
            originalPos,
            Time.deltaTime * moveSpeed
        );
        if (Vector3.Distance(targetObject.position, originalPos) < 0.01f)
        {
            isReturning = false;
            hasReturned = true;
        }
    }
    // ======================
    // ゲーム操作停止（完全版）
    // ======================
    private void PauseGameControls()
    {
        // カメラ回転停止
        foreach (var ml in FindObjectsByType<MouseLook>(FindObjectsSortMode.None))
            ml.enabled = false;
        // 移動停止
        var playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.enabled = false;
        // PauseMenu停止
        var pauseMenu = FindFirstObjectByType<PauseMenu>();
        if (pauseMenu != null)
            pauseMenu.enabled = false;
        // インベントリ操作停止
        var inventoryManager = FindFirstObjectByType<InventoryManager>();
        if (inventoryManager != null)
            inventoryManager.enabled = false;
        // インベントリUI 停止
        var inventoryUI = FindFirstObjectByType<InventoryUI>();
        if (inventoryUI != null)
            inventoryUI.enabled = false;
    }
    // ======================
    // ゲーム操作再開
    // ======================
    private void ResumeGameControls()
    {
        foreach (var ml in FindObjectsByType<MouseLook>(FindObjectsSortMode.None))
            ml.enabled = true;
        var playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.enabled = true;
        var pauseMenu = FindFirstObjectByType<PauseMenu>();
        if (pauseMenu != null)
            pauseMenu.enabled = true;
        var inventoryManager = FindFirstObjectByType<InventoryManager>();
        if (inventoryManager != null)
            inventoryManager.enabled = true;
        var inventoryUI = FindFirstObjectByType<InventoryUI>();
        if (inventoryUI != null)
            inventoryUI.enabled = true;
    }
}