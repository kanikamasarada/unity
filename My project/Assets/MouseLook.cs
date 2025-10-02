using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // シングルトン参照
    public static CameraControl Instance { get; private set; }

    [Range(0f, 250f)]
    public float mouseSensitivity = 50f;

    private float xRotation = 0f;
    private Transform playerBody;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerBody = transform.parent; // 親をプレイヤー本体に
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
