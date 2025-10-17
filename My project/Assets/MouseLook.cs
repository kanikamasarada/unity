using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public static MouseLook Instance;
    public Transform playerBody;
    public float mouseSensitivity = 300f;

    private float xRotation = 0f;
    [HideInInspector] public bool isPaused = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // ❌ 削除：ここでカーソルロックするとUI操作できなくなる
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    void Update()
    {
        // UI中・ポーズ中は視点を動かさない
        var pauseMenu = FindFirstObjectByType<PauseMenu>();
        var sleepUI = FindFirstObjectByType<SleepUI>();

        bool uiActive =
            (pauseMenu != null && pauseMenu.IsPaused) ||
            (sleepUI != null && sleepUI.panel.activeSelf);

        if (uiActive || isPaused) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
