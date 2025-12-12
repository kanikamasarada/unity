using UnityEditorInternal.VersionControl;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float crouchSpeed = 1.5f;  // :太字のプラス記号:追加：しゃがみ時の速度
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    private Rigidbody rb;
    private float mouseX, mouseY;
    private float xRotation = 0f;
    public AudioSource footstepAudio;
    public float speedThreshold = 0.1f;
    [Header("Crouch Settings")]
    public bool isCrouching = false;
    public float crouchHeight = 1.2f;
    public float standHeight = 1.8f;
    public float crouchCameraOffset = 0.8f;
    private CapsuleCollider col;
    [Header("Noise Output for Enemy AI")]
    public float noiseLevel = 1f;       // 歩く時の音量
    public float crouchNoise = 0f;      // しゃがみ時の音（完全にゼロもOK）
    public float CurrentNoise { get; private set; }
    [Header("Head Bob Settings")]
    public float bobFrequency = 1.5f;
    public float bobAmplitude = 0.05f;
    private float bobTimer = 0f;
    private Vector3 initialCameraLocalPos;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        col = GetComponent<CapsuleCollider>();
        LockCursor(true);
        if (cameraTransform != null)
            initialCameraLocalPos = cameraTransform.localPosition;
    }
    void Update()
    {
        HandleCrouch();        // :太字のプラス記号:追加（しゃがみ処理）
        HandleCameraLook();
        HandleMovement();
        HandleFootstepSound();
        HandleHeadBob();
    }
    private void HandleCrouch()
    {
        // :太字のプラス記号: スペースを押している間だけしゃがむ
        if (Input.GetKey(KeyCode.LeftControl))
            isCrouching = true;
        else
            isCrouching = false;
        // 高さ調整
        float targetHeight = isCrouching ? crouchHeight : standHeight;
        col.height = Mathf.Lerp(col.height, targetHeight, Time.deltaTime * 10f);
        // 視点も連動して少し下げる
        if (cameraTransform != null)
        {
            Vector3 camPos = initialCameraLocalPos;
            if (isCrouching) camPos.y -= crouchCameraOffset;
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                camPos,
                Time.deltaTime * 10f
            );
        }
    }
    private void HandleCameraLook()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
        Vector3 velocity = new Vector3(move.x * currentSpeed, rb.linearVelocity.y, move.z * currentSpeed);
        rb.linearVelocity = velocity;
        // 敵AI用の Noise 値
        bool isMoving = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude > speedThreshold;
        CurrentNoise = isMoving ? (isCrouching ? crouchNoise : noiseLevel) : 0;
    }
    private void HandleFootstepSound()
    {
        if (footstepAudio == null) return;
        float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        bool isMoving = horizontalSpeed > speedThreshold;
        if (isCrouching)
        {
            // :太字のプラス記号: しゃがみ中は足音OFF
            if (footstepAudio.isPlaying) footstepAudio.Stop();
            return;
        }
        if (isMoving)
        {
            if (!footstepAudio.isPlaying)
            {
                footstepAudio.pitch = Random.Range(0.95f, 1.05f);
                footstepAudio.Play();
            }
        }
        else
        {
            if (footstepAudio.isPlaying)
                footstepAudio.Stop();
        }
    }
    private void HandleHeadBob()
    {
        if (cameraTransform == null) return;
        float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        bool isMoving = horizontalSpeed > speedThreshold;
        if (isMoving && !isCrouching) // :太字のプラス記号:しゃがみ中はボブを弱める
        {
            bobTimer += Time.deltaTime * bobFrequency * (horizontalSpeed / moveSpeed);
            float bobOffsetY = Mathf.Sin(bobTimer * Mathf.PI * 2f) * bobAmplitude;
            cameraTransform.localPosition = initialCameraLocalPos + new Vector3(0, bobOffsetY, 0);
        }
        else
        {
            bobTimer = 0f;
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                initialCameraLocalPos,
                Time.deltaTime * 5f
            );
        }
    }
    private void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
