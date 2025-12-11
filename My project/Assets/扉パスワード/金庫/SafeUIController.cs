using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 電卓風 UI のコントローラ。Inspectorで UI 要素（数字ボタン・枠テキスト等）を接続すること。
/// Keyboard input は使わず、UI ボタンだけで入力する仕様。
/// </summary>
public class SafeUIController : MonoBehaviour
{
    [Header("表示テキスト (Legacy Text)")]
    public Text[] slotTexts = new Text[3];      // 3枠表示 (A,B,C)
    public Image[] slotHighlights;              // 任意：選択中枠のハイライト（null可）

    [Header("ボタン類")]
    public Button[] digitButtons;               // 0~9 のボタンを順に入れる（Inspector）
    public Button backspaceButton;              // 1文字消す
    public Button clearButton;                  // 選択枠の全消去
    public Button submitButton;                 // 決定
    public Button closeButton;                  // 閉じる

    [Header("その他")]
    public Color highlightColor = new Color(1f, 1f, 0.6f);
    public Color normalColor = Color.white;

    // 開かれるときに渡される
    private string[] correctCodes = new string[3];
    private int maxDigits = 2;
    private Action onSuccess;
    private Action onClose;

    // 内部状態
    private string[] inputs = new string[3] { "", "", "" };
    private int selectedIndex = 0;
    private bool isOpen = false;

    void Awake()
    {
        // ボタンの hookup (Inspectorでも可だが自動で繋がってない場合用)
        if (digitButtons != null)
        {
            for (int i = 0; i < digitButtons.Length && i < 10; i++)
            {
                int d = i; // capture
                digitButtons[i].onClick.AddListener(() => OnDigitPressed(d));
            }
        }

        if (backspaceButton != null) backspaceButton.onClick.AddListener(OnBackspacePressed);
        if (clearButton != null) clearButton.onClick.AddListener(OnClearPressed);
        if (submitButton != null) submitButton.onClick.AddListener(OnSubmitPressed);
        if (closeButton != null) closeButton.onClick.AddListener(OnClosePressed);

        // 初期は非表示想定（親パネル側で制御）
        gameObject.SetActive(true); // panel object is active, but controller can be used even if hidden
    }

    /// <summary>
    /// SafeController から呼んで開く
    /// </summary>
    public void Open(string[] codes, int maxDigitsPerSlot, Action successCallback, Action closeCallback)
    {
        if (codes == null || codes.Length < 3)
            throw new ArgumentException("codes must be length 3");

        correctCodes = new string[3] { codes[0], codes[1], codes[2] };
        maxDigits = Mathf.Max(1, maxDigitsPerSlot);
        onSuccess = successCallback;
        onClose = closeCallback;

        // 初期化
        inputs[0] = inputs[1] = inputs[2] = "";
        selectedIndex = 0;
        isOpen = true;

        // 表示更新
        UpdateAllSlots();

        // 入力中はプレイヤー制御を停止
        SetPlayerControl(false);

        // カーソル出す
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// UI を閉じる（成功・失敗ともに呼ぶ）
    /// </summary>
    public void Close()
    {
        isOpen = false;
        SetPlayerControl(true);

        // カーソルはシーン側で管理したいがここでは非表示に戻す
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        onClose?.Invoke();
    }

    // ---------- ボタンコールバック ----------
    public void OnDigitPressed(int digit)
    {
        if (!isOpen) return;

        if (inputs[selectedIndex].Length >= maxDigits) return;
        inputs[selectedIndex] += digit.ToString();
        UpdateSlot(selectedIndex);
    }

    public void OnBackspacePressed()
    {
        if (!isOpen) return;

        var s = inputs[selectedIndex];
        if (!string.IsNullOrEmpty(s))
        {
            inputs[selectedIndex] = s.Substring(0, s.Length - 1);
            UpdateSlot(selectedIndex);
        }
    }

    public void OnClearPressed()
    {
        if (!isOpen) return;

        inputs[selectedIndex] = "";
        UpdateSlot(selectedIndex);
    }

    public void OnSubmitPressed()
    {
        if (!isOpen) return;

        // 比較（厳密一致）
        bool ok = inputs[0] == correctCodes[0] &&
                  inputs[1] == correctCodes[1] &&
                  inputs[2] == correctCodes[2];

        if (ok)
        {
            // 成功処理
            onSuccess?.Invoke();
            // UI を閉じる
            HideAndClose();
        }
        else
        {
            // 不正解時の簡単演出（例：赤フラッシュ等） — 今はログのみ
            Debug.Log("SafeUIController: パスワード不正解");
        }
    }

    public void OnClosePressed()
    {
        if (!isOpen) return;
        HideAndClose();
    }

    // ユーザーが枠をタップするための公開メソッド（UI Text に Button を重ねて割り当てる想定）
    public void SelectSlot(int index)
    {
        if (!isOpen) return;
        if (index < 0 || index >= 3) return;
        selectedIndex = index;
        UpdateHighlight();
    }

    // ---------- 表示更新 ----------
    private void UpdateAllSlots()
    {
        for (int i = 0; i < 3; i++)
            UpdateSlot(i);

        UpdateHighlight();
    }

    private void UpdateSlot(int idx)
    {
        if (slotTexts != null && idx < slotTexts.Length && slotTexts[idx] != null)
        {
            // 表示は入力文字列そのまま。必要ならマスク（*）に変更可能。
            slotTexts[idx].text = inputs[idx];
        }
    }

    private void UpdateHighlight()
    {
        if (slotHighlights == null) return;
        for (int i = 0; i < slotHighlights.Length; i++)
        {
            if (slotHighlights[i] == null) continue;
            var img = slotHighlights[i];
            img.color = (i == selectedIndex) ? highlightColor : normalColor;
        }
    }

    private void HideAndClose()
    {
        // SafeController がパネル自体を管理している想定なので、ここでは Close() を呼ぶだけ
        // もしパネル自体を非表示にしたいなら親パネルを非アクティブにする等。
        Close();
    }

    // ---------- プレイヤー制御停止 / 再開 ----------
    private void SetPlayerControl(bool enable)
    {
        // PlayerMovement と MouseLook を無効化/有効化する（存在すれば）
        var playerMove = FindFirstObjectByType<PlayerMovement>();
        var mouseLook = FindFirstObjectByType<MouseLook>();

        if (playerMove != null) playerMove.enabled = enable;
        if (mouseLook != null) mouseLook.enabled = enable;

        // PauseMenuを開く等を防ぐ方針なら、必要に応じて PauseMenu を操作してください。
        // （ここでは単純にプレイヤーの移動と視点を無効化しています）
    }

    // 補助：FindFirstObjectByType をエディションにより利用できない場合は
    // GameObject.FindObjectOfType<T>() に読み替えてください。
}
