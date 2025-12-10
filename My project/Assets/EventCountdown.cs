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
    public GameObject finishPanel;
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
        // ★ ドア全ロック
        var doors = FindObjectsByType<SlideDoor>(FindObjectsSortMode.None);
        foreach (var d in doors)
        {
            d.isLocked = true;
        }

        // ★ 敵に落下命令を送る
        TriggerEnemiesFall();

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

        if (finishPanel != null)
            finishPanel.SetActive(true);

        if (countdownUI != null)
            countdownUI.SetActive(false);

        PauseGameControls();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isRunning = false;
    }

    private void PauseGameControls()
    {
        var MouseLook = FindObjectsByType<MouseLook>(FindObjectsSortMode.None);
        foreach (var ml in MouseLook)
            ml.enabled = false;

        var PlayerMovement = FindFirstObjectByType<PlayerMovement>();
        if (PlayerMovement != null)
            PlayerMovement.enabled = false;

        var pauseMenu = FindFirstObjectByType<PauseMenu>();
        if (pauseMenu != null)
            pauseMenu.enabled = false;

        var InventoryManager = FindFirstObjectByType<InventoryManager>();
        if (InventoryManager != null)
            InventoryManager.enabled = false;

        var InventoryUI = FindFirstObjectByType<InventoryUI>();
        if (InventoryUI != null)
            InventoryUI.enabled = false;
    }

    private void OnClickReturnTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // ★ 敵全員に落下命令
    private void TriggerEnemiesFall()
    {
        var enemies = FindObjectsByType<EnemyDropFollow>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            
        }
    }
}
