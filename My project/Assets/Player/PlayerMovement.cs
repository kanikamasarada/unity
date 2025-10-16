using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;

    private Rigidbody rb;
    private float mouseX, mouseY;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        LockCursor(true); // ゲーム開始時はロック
    }

    void Update()
    {
        // ポーズメニューやスリープUIが開いてたら操作しない
        var pauseMenu = FindFirstObjectByType<PauseMenu>();
        var sleepUI = FindFirstObjectByType<SleepUI>();

        bool uiActive = 
            (pauseMenu != null && pauseMenu.IsPaused) ||
            (sleepUI != null && sleepUI.panel.activeSelf);

        if (uiActive)
        {
            LockCursor(false); // UIが開いてる間はカーソル解放
            return;
        }
        else
        {
            LockCursor(true); // 通常プレイ中はロック
        }

        // --- カーソルロック後の操作 ---
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 velocity = new Vector3(move.x * moveSpeed, rb.linearVelocity.y, move.z * moveSpeed);
        rb.linearVelocity = velocity;
    }

    private void LockCursor(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
