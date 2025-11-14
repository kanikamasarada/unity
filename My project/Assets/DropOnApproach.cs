using UnityEngine;

public class DropOnApproach : MonoBehaviour
{
    public float triggerDistance = 3f; // 落とす距離
    private Rigidbody rb;
    private GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // 最初は落ちないようにする
        player = GameObject.FindWithTag("Player"); // Playerにタグをつけておく
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= triggerDistance)
        {
            rb.isKinematic = false; // Playerが近づいたら落ちる
        }
    }
}