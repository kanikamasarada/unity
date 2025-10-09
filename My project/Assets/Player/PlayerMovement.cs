using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform;

    private Rigidbody rb;
    private float mouseX, mouseY;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 回転の暴走防止
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // すり抜け防止
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // マウス操作
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        // 移動入力
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 velocity = new Vector3(move.x * moveSpeed, rb.linearVelocity.y, move.z * moveSpeed);
        rb.linearVelocity = velocity;
    }
}
