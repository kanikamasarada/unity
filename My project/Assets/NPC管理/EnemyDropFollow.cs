using UnityEngine;

public class EnemyDropFollow : MonoBehaviour
{
    public float followSpeed = 3f;
    public float activateFollowDelay = 3f;
    public string playerTag = "Player";
    public string groundTag = "Yuka";

    private Transform player;
    private Rigidbody rb;

    private bool isFalling = false;
    private bool isGrounded = false;
    private bool isFollowing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag(playerTag).transform;

        rb.useGravity = false;
    }

    public void StartDrop()
    {
        isFalling = true;
        rb.useGravity = true;
    }

    void Update()
    {
        if (isFollowing && player != null)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * followSpeed * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(groundTag) && !isGrounded)
        {
            isGrounded = true;
            isFalling = false;

            Invoke(nameof(StartFollow), activateFollowDelay);
        }
    }

    void StartFollow()
    {
        isFollowing = true;
    }
}
