using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public static MouseLook Instance;
    public float mouseSensitivity = 100f;
    public Transform playerBody; // ← これが Player
    float xRotation = 0f;

    public bool isPaused = false; // ← PauseMenuから制御される

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
        // 一時停止・インベントリ中は動かさない
        if (isPaused || InventoryActive()) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 上下回転：カメラ
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 左右回転：プレイヤー本体
        if (playerBody != null)
            playerBody.Rotate(Vector3.up * mouseX);
    }

    bool InventoryActive()
    {
        return InventoryManager.Instance != null && InventoryManager.Instance.IsOpen();
    }
}
