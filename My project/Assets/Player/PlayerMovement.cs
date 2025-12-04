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

    public AudioSource footstepAudio;
    public float speedThreshold = 0.1f; // これより速く動いたら歩いてる扱い

    [Header("Head Bob Settings")]
    public float bobFrequency = 1.5f;   // 歩く速さに合わせて揺れる速度
    public float bobAmplitude = 0.05f;  // 揺れる大きさ
    private float bobTimer = 0f;
    private Vector3 initialCameraLocalPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        LockCursor(true); // ゲーム開始時はロック

        if (cameraTransform != null)
            initialCameraLocalPos = cameraTransform.localPosition;

    }

    private void HandleHeadBob()
    {
        if (cameraTransform == null) return;

        // 水平方向の速度のみを見る
        float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        bool isMoving = horizontalSpeed > speedThreshold;

        if (isMoving)
        {
            bobTimer += Time.deltaTime * bobFrequency * (horizontalSpeed / moveSpeed); // 速さに応じて変化
            float bobOffsetY = Mathf.Sin(bobTimer * Mathf.PI * 2f) * bobAmplitude;
            cameraTransform.localPosition = initialCameraLocalPos + new Vector3(0f, bobOffsetY, 0f);
        }
        else
        {
            // 止まったら元位置に戻す
            bobTimer = 0f;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, initialCameraLocalPos, Time.deltaTime * 5f);
        }
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
            footstepAudio.Stop();
            if (footstepAudio != null && footstepAudio.isPlaying)
                footstepAudio.Stop();
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
        HandleFootstepSound();
        HandleHeadBob();
    }

    private void HandleFootstepSound()
    {
        if (footstepAudio == null) return;

        // 水平方向の速度のみを見る (上向きジャンプで再生されないように)
        float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;

        bool isMoving = horizontalSpeed > speedThreshold;
        footstepAudio.pitch = 1.2f; // 20%速く再生

        if (isMoving)
        {
            if (!footstepAudio.isPlaying)
            {
                // 少しランダムにピッチを揺らして足音の違和感を減らす
                footstepAudio.pitch = Random.Range(0.95f + 0.3f, 1.05f + 0.3f);
                footstepAudio.Play();
            }
        }
        else
        {
            if (footstepAudio.isPlaying)
                footstepAudio.Stop();
        }
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