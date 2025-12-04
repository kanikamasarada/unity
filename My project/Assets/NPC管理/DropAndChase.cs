using UnityEngine;

public class DropAndChase : MonoBehaviour
{
    public string playerTag = "Player";
    public float chaseSpeed = 2f;      // プレイヤーを追う速度
    public float groundCheckDistance = 0f; // 地面判定の距離

    private Rigidbody rb;
    private Transform player;
    private bool hasLanded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;

        // 落下させるために最初は重力ON
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    void FixedUpdate()
    {
        // 1️⃣ 地面に着いたかチェック（Raycast）
        if (!hasLanded)
        {
            if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance))
            {
                hasLanded = true;
                rb.isKinematic = true;   // 物理を止める（倒れたり転がったりしない）
                rb.useGravity = false;
            }
            return;
        }

        // 2️⃣ 地面に着いたらプレイヤーを追う
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // 上下は追わない（水平移動）

            transform.position += direction * chaseSpeed * Time.deltaTime;

            // プレイヤーの方向を向く
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    Time.deltaTime * 5f
                );
        }
    }
}
