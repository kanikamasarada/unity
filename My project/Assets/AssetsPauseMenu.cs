using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // ★追加：シーン切り替えに必要

public class PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject inventoryPanel;

    [Header("UI - Sliders")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Text volumeValueText;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityValueText;

    [Header("Screen Mode Toggles")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle windowedToggle;
    [SerializeField] private Toggle borderlessToggle;
    [SerializeField] private ToggleGroup screenModeGroup;

    private bool isPaused = false;
    private bool isInventoryOpen = false;

    public bool IsPaused => isPaused;

    private void Start()
    {
        // === 音量スライダー ===
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 100f;
            volumeSlider.value = AudioListener.volume * 100f;
            UpdateVolumeText(volumeSlider.value);
            volumeSlider.onValueChanged.AddListener(v =>
            {
                AudioListener.volume = v / 100f;
                UpdateVolumeText(v);
            });
        }

        // === 感度スライダー ===
        var mouseLook = MouseLook.Instance ?? FindFirstObjectByType<MouseLook>();
        if (sensitivitySlider != null && mouseLook != null)
        {
            sensitivitySlider.minValue = 0f;
            sensitivitySlider.maxValue = 300f;
            sensitivitySlider.value = mouseLook.mouseSensitivity;
            UpdateSensitivityText(sensitivitySlider.value);
            sensitivitySlider.onValueChanged.AddListener(v =>
            {
                var allMouseLooks = FindObjectsByType<MouseLook>(FindObjectsSortMode.None);
                foreach (var ml in allMouseLooks)
                {
                    ml.mouseSensitivity = v;
                }
                UpdateSensitivityText(v);
            });
        }
        else
        {
            Debug.LogWarning("MouseLook コンポーネントが見つかりません。感度スライダーは無効です。");
        }

        // === 画面モード初期化 ===
        if (screenModeGroup != null)
        {
            fullscreenToggle.group = screenModeGroup;
            windowedToggle.group = screenModeGroup;
            borderlessToggle.group = screenModeGroup;
        }

        SetFullScreen();
        fullscreenToggle.isOn = true;
        windowedToggle.isOn = false;
        borderlessToggle.isOn = false;

        fullscreenToggle.onValueChanged.AddListener(on => { if (on) SetFullScreen(); });
        windowedToggle.onValueChanged.AddListener(on => { if (on) SetWindowed(); });
        borderlessToggle.onValueChanged.AddListener(on => { if (on) SetBorderless(); });

        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInventoryOpen)
            {
                CloseInventory();
                PauseGame();
            }
            else if (!isPaused)
            {
                PauseGame();
            }
            else if (settingsPanel.activeSelf)
            {
                CloseSettings();
            }
            else if (howToPlayPanel.activeSelf)
            {
                CloseHowToPlay();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    // === 一時停止 ===
    public void PauseGame()
    {
        var allMouseLooks = FindObjectsByType<MouseLook>(FindObjectsSortMode.None);
        foreach (var ml in allMouseLooks)
        {
            ml.isPaused = true;
        }

        // ★追加: PlayerMovementも停止
        var playerMove = FindFirstObjectByType<PlayerMovement>();
        if (playerMove != null)
        {
            playerMove.enabled = false;
        }

        pauseMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // === 再開 ===
    public void ResumeGame()
    {
        var allMouseLooks = FindObjectsByType<MouseLook>(FindObjectsSortMode.None);
        foreach (var ml in allMouseLooks)
        {
            ml.isPaused = false;
        }

        // ★追加: PlayerMovementを再開
        var playerMove = FindFirstObjectByType<PlayerMovement>();
        if (playerMove != null)
        {
            playerMove.enabled = true;
        }

        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CloseInventory()
    {
        if (inventoryPanel != null)
        {
            isInventoryOpen = false;
            inventoryPanel.SetActive(false);
        }
    }

    public void OpenSettings()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void OpenHowToPlay()
    {
        pauseMenuPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    // === ★変更箇所：ゲーム終了 → タイトルシーンへ戻る ===
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // タイトルシーン名をここで指定（例: "TitleScene"）
        SceneManager.LoadScene("TitleScene");
    }

    private void SetFullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        Screen.fullScreen = true;
    }

    private void SetWindowed()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.fullScreen = false;
    }

    private void SetBorderless()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.fullScreen = true;
    }

    private void UpdateVolumeText(float value)
    {
        if (volumeValueText != null)
            volumeValueText.text = $"{Mathf.RoundToInt(value)}";
    }

    private void UpdateSensitivityText(float value)
    {
        if (sensitivityValueText != null)
            sensitivityValueText.text = $"{Mathf.RoundToInt(value)}";
    }
}
