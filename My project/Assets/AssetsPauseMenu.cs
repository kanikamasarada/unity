using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    void Start()
    {
        // ===== 音量スライダー =====
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

        // ===== 感度スライダー =====
        if (sensitivitySlider != null && CameraControl.Instance != null)
        {
            sensitivitySlider.minValue = 0f;
            sensitivitySlider.maxValue = 250f;
            sensitivitySlider.value = CameraControl.Instance.mouseSensitivity;
            UpdateSensitivityText(sensitivitySlider.value);
            sensitivitySlider.onValueChanged.AddListener(v =>
            {
                CameraControl.Instance.mouseSensitivity = v;
                UpdateSensitivityText(v);
            });
        }

        // ===== 画面モード初期化 =====
        if (screenModeGroup != null)
        {
            fullscreenToggle.group = screenModeGroup;
            windowedToggle.group = screenModeGroup;
            borderlessToggle.group = screenModeGroup;
        }

        // デフォルトはフルスクリーン
        SetFullScreen();
        fullscreenToggle.isOn = true;
        windowedToggle.isOn = false;
        borderlessToggle.isOn = false;

        fullscreenToggle.onValueChanged.AddListener(on => { if (on) SetFullScreen(); });
        windowedToggle.onValueChanged.AddListener(on => { if (on) SetWindowed(); });
        borderlessToggle.onValueChanged.AddListener(on => { if (on) SetBorderless(); });

        // ===== パネル初期化 =====
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
    }

    void Update()
    {
        // ===== Escキー処理 =====
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

        // ===== Tabキーでインベントリ開閉（ポーズ中は無効） =====
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isPaused && inventoryPanel != null)
            {
                isInventoryOpen = !isInventoryOpen;
                inventoryPanel.SetActive(isInventoryOpen);
            }
        }
    }

    // ===== Pause / Resume =====
    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ===== Inventory =====
    private void CloseInventory()
    {
        if (inventoryPanel != null)
        {
            isInventoryOpen = false;
            inventoryPanel.SetActive(false);
        }
    }

    // ===== Settings Panel =====
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

    // ===== How To Play Panel =====
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

    // ===== Quit =====
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ===== Screen Mode =====
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

    // ===== スライダー値更新 =====
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
