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

    void Update()
    {
        // Inventoryが開いている場合は視点操作を止める
        if (InventoryManager.Instance != null && InventoryManager.Instance.IsOpen())
            return;

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
