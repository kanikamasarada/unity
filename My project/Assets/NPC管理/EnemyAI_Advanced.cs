using UnityEngine;

public class EnemyAI_Advanced : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 1.5f;
    public float chaseSpeed = 6f;
    public float lostChaseSpeed = 3f;
    public float turnSpeed = 5f;

    [Header("Detection")]
    public float viewDistance = 12f;
    public float viewAngle = 60f;
    public float loseSightTime = 3f;
    public LayerMask obstacleMask;

    [Header("Wander Settings")]
    public float wanderRadius = 10f;     // ランダムで選ぶ範囲
    public float pointReachDistance = 1f;
    public float obstacleCheckDistance = 1f;

    private Transform player;
    private Vector3 wanderTarget;        // 次に向かう目的地
    private float loseTimer;
    private bool isChasing = false;
    private Vector3 lastSeenPos;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        PickNewWanderPoint();
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = dist < viewDistance && IsPlayerInSight();

        if (canSeePlayer)
        {
            isChasing = true;
            loseTimer = 0;
            lastSeenPos = player.position;
        }
        else if (isChasing)
        {
            loseTimer += Time.deltaTime;
            if (loseTimer > loseSightTime)
                isChasing = false;
        }

        if (isChasing)
            ChasePlayer();
        else
            Wander();
    }

    // プレイヤー視認チェック
    bool IsPlayerInSight()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > viewAngle) return false;

        if (Physics.Raycast(transform.position + Vector3.up, dir, out RaycastHit hit, viewDistance))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }

    // ---------------------
    // ▶ ランダム地点へ徘徊
    // ---------------------
    void Wander()
    {
        Vector3 dir = (wanderTarget - transform.position).normalized;

        if (IsObstacleAhead())
        {
            AvoidObstacle();   // 壁回避
            return;
        }

        // ターゲットに近づく
        RotateTowards(dir);
        transform.position += dir * walkSpeed * Time.deltaTime;

        // 目的地に着いたら次を決める
        if (Vector3.Distance(transform.position, wanderTarget) < pointReachDistance)
        {
            PickNewWanderPoint();
        }
    }

    // ランダムな地点を選ぶ
    void PickNewWanderPoint()
    {
        Vector2 rand = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(
            transform.position.x + rand.x,
            transform.position.y,
            transform.position.z + rand.y
        );
    }

    // ---------------------
    // ▶ 追跡
    // ---------------------
    void ChasePlayer()
    {
        Vector3 dir;
        float speed = chaseSpeed;

        if (IsPlayerInSight())
        {
            dir = (player.position - transform.position).normalized;
            lastSeenPos = player.position;
        }
        else
        {
            dir = (lastSeenPos - transform.position).normalized;
            speed = lostChaseSpeed;
        }

        if (IsObstacleAhead())
            AvoidObstacle();

        RotateTowards(dir);
        transform.position += dir * speed * Time.deltaTime;
    }

    // 壁判定
    bool IsObstacleAhead()
    {
        return Physics.Raycast(
            transform.position + Vector3.up * 0.5f,
            transform.forward,
            obstacleCheckDistance,
            obstacleMask
        );
    }

    // 壁回避
    void AvoidObstacle()
    {
        float turn = Random.value < 0.5f ? 90f : -90f;
        transform.Rotate(0, turn, 0);
        PickNewWanderPoint();   // 新しい地点も決めておく
    }

    // スムーズに回転
    void RotateTowards(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        Quaternion target = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * turnSpeed);
    }
}
