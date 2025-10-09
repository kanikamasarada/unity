using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public static MouseLook Instance;
    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    float yRotation = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // インベントリやポーズ中は止める
        if (PauseMenuActive() || InventoryActive()) return;

        // マウス入力
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // カメラの上下
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // カメラの左右
        yRotation += mouseX;

        // カメラを回転
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    bool PauseMenuActive()
    {
    var menu = FindFirstObjectByType<PauseMenu>();
    return menu != null && menu.IsPaused;
    }

    bool InventoryActive()
    {
        return InventoryManager.Instance != null && InventoryManager.Instance.IsOpen();
    }
}
