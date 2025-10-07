using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance;
    public Transform playerBody;       // プレイヤー本体
    public float mouseSensitivity = 100f;
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (playerBody == null) return;

        // マウス入力のみで回転
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 上下回転（カメラ）
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 左右回転（プレイヤー本体）
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
