using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        // カウントダウン UI は最初は非表示
        if (countdownUI != null)
            countdownUI.SetActive(false);

        // 終了パネルも非表示
        if (finishPanel != null)
            finishPanel.SetActive(false);

        // タイトルへ戻るボタン
        if (toTitleButton != null)
            toTitleButton.onClick.AddListener(OnClickReturnTitle);

        // 赤フェード初期化
        if (redOverlay != null)
        {
            Color c = redOverlay.color;
            c.a = 0f;
            redOverlay.color = c;
        }

        // カウントダウン自動スタート
        StartCountdown();
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

        // ここ！！パネルを確実に表示
        if (finishPanel != null)
            finishPanel.SetActive(true);

        isRunning = false;

        // カウントダウン UI を消したい場合はコメント解除
        // if (countdownUI != null)
        //     countdownUI.SetActive(false);
    }

    private void OnClickReturnTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
