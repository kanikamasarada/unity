using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject inventoryPanel; // インベントリパネル

    [Header("UI")]
    [SerializeField] private Slider volumeSlider;

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
        // 音量初期化
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 100f;
            volumeSlider.value = AudioListener.volume * 100f;
            volumeSlider.onValueChanged.AddListener(v => AudioListener.volume = v / 100f);
        }

        // ToggleGroup 設定
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

        // トグル変更イベント
        fullscreenToggle.onValueChanged.AddListener(on => { if (on) SetFullScreen(); });
        windowedToggle.onValueChanged.AddListener(on => { if (on) SetWindowed(); });
        borderlessToggle.onValueChanged.AddListener(on => { if (on) SetBorderless(); });

        // パネル初期状態
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }

    void Update()
    {
        // Escキー
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // インベントリ開いていたら閉じてメニュー表示
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

        // Tabキーでインベントリ開閉（ポーズ中は無効）
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isPaused) // メニュー中は無効
            {
                if (inventoryPanel != null)
                {
                    isInventoryOpen = !isInventoryOpen;
                    inventoryPanel.SetActive(isInventoryOpen);
                }
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

    // ===== Settings =====
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

    // ===== How To Play =====
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
}
