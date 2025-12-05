using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventCountdown : MonoBehaviour
{
    [Header("カウントダウン設定")]
    public float countdownTime = 10f;
    private float timer;

    [Header("UI 要素")]
    public GameObject countdownUI;
    public Text countdownText;
    public GameObject finishPanel;  // ← これを 0 の時に表示する
    public Button toTitleButton;

    [Header("サウンド")]
    public AudioSource audioSource;
    public AudioClip startSE;
    public AudioClip finishSE;

    [Header("赤フェード演出")]
    public Image redOverlay;
    public float maxRedAlpha = 0.5f;

    [Header("警告演出（点滅）")]
    public bool enableBlink = true;
    public float blinkStartTime = 3f;
    public float blinkSpeed = 6f;
    public float blinkExtraAlpha = 0.2f;

    private bool isFinished = false;
    private bool isRunning = false;

    void Start()
    {
        if (countdownUI != null)
            countdownUI.SetActive(false);

        if (finishPanel != null)
            finishPanel.SetActive(false);

        if (toTitleButton != null)
            toTitleButton.onClick.AddListener(OnClickReturnTitle);

        if (redOverlay != null)
        {
            Color c = redOverlay.color;
            c.a = 0f;
            redOverlay.color = c;
        }
    }

    public void StartCountdown()
    {
        timer = countdownTime;
        isRunning = true;
        isFinished = false;

        if (countdownUI != null)
            countdownUI.SetActive(true);

        if (startSE != null && audioSource != null)
            audioSource.PlayOneShot(startSE);
    }

    void Update()
    {
        if (!isRunning || isFinished) return;

        timer -= Time.deltaTime;
        if (timer < 0) timer = 0;

        if (countdownText != null)
            countdownText.text = Mathf.Ceil(timer).ToString();

        UpdateRedOverlay();

        if (timer <= 0)
        {
            isFinished = true;
            OnCountdownFinished();
        }
    }

    private void UpdateRedOverlay()
    {
        if (redOverlay == null) return;

        float baseAlpha = Mathf.Lerp(0f, maxRedAlpha, 1 - (timer / countdownTime));
        float finalAlpha = baseAlpha;

        if (enableBlink && timer <= blinkStartTime && timer > 0)
        {
            float blink = (Mathf.Sin(Time.time * blinkSpeed) + 1) / 2f;
            finalAlpha = baseAlpha + blink * blinkExtraAlpha;
            finalAlpha = Mathf.Clamp(finalAlpha, 0f, 1f);
        }

        Color c = redOverlay.color;
        c.a = finalAlpha;
        redOverlay.color = c;
    }

    private void OnCountdownFinished()
    {
        if (finishSE != null && audioSource != null)
            audioSource.PlayOneShot(finishSE);

        // ----------- パネル表示 ----------- //
        if (finishPanel != null)
            finishPanel.SetActive(true);

        // ----------- カウントダウン UI を非表示 ----------- //
        if (countdownUI != null)
            countdownUI.SetActive(false);

        // ----------- ★操作停止（ゲームオーバー） ----------- //
        PauseGameControls();

        // ----------- カーソル表示 ----------- //
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isRunning = false;
    }

    /// <summary>
    /// ★ MouseLook, PlayerMovement, PauseMenu を無効化する（あなたの提示した処理に合わせた）
    /// </summary>
    private void PauseGameControls()
    {
        // MouseLook 停止
        var allMouseLooks = FindObjectsByType<MouseLook>(FindObjectsSortMode.None);
        foreach (var ml in allMouseLooks)
        {
            ml.isPaused = true;
        }

        // PlayerMovement 停止
        var playerMove = FindFirstObjectByType<PlayerMovement>();
        if (playerMove != null)
        {
            playerMove.enabled = false;
        }

        // PauseMenu (ESC メニュー) 停止
        var pauseMenu = FindFirstObjectByType<PauseMenu>();
        if (pauseMenu != null)
        {
            pauseMenu.enabled = false;
        }
    }

    private void OnClickReturnTitle()
    {
        // ESC メニューを戻す必要があればここで解除しても良い
        SceneManager.LoadScene("TitleScene");
    }
}
