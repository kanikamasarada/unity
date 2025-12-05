using UnityEngine;
public class SoundReactiveEnemy_NoNav : MonoBehaviour
{
    public float moveSpeed = 2f;        // 移動スピード
    public float rotateSpeed = 120f;    // 旋回スピード
    public float wanderChangeInterval = 3f;  // ランダム方向の変更間隔
    public float hearingRange = 15f;    // 足音を聞く範囲
    public float investigateTime = 2f;  // 音の場所で停止する時間
    private Transform player;
    private PlayerMovement playerMov;
    private Vector3 moveDirection;
    private float wanderTimer = 0f;
    private bool investigating = false;
    private Vector3 soundTarget;
    private float investigateTimer = 0f;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;
        playerMov = player.GetComponent<PlayerMovement>();
        PickRandomDirection();
    }
    void Update()
    {
        float noise = playerMov.CurrentNoise;
        float dist = Vector3.Distance(transform.position, player.position);
        // :音量大: 足音を聞く
        if (noise > 0 && dist <= hearingRange)
        {
            investigating = true;
            soundTarget = player.position;
            investigateTimer = 0f;
        }
        // :右向き虫眼鏡: 足音の場所を調べている
        if (investigating)
        {
            InvestigateSound();
            return;
        }
        // :歩く人: ランダム徘徊
        Wander();
    }
    // -------------------------
    // :右向き虫眼鏡: 足音の場所を調べる
    // -------------------------
    void InvestigateSound()
    {
        investigateTimer += Time.deltaTime;
        Vector3 dir = (soundTarget - transform.position).normalized;
        // 回転
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            rotateSpeed * Time.deltaTime
        );
        // 前へ移動
        rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, soundTarget) < 0.8f ||
            investigateTimer > investigateTime)
        {
            investigating = false;
            PickRandomDirection();
        }
    }
    // -------------------------
    // :歩く人: ランダム徘徊
    // -------------------------
    void Wander()
    {
        wanderTimer += Time.deltaTime;
        // 一定時間で方向チェンジ
        if (wanderTimer >= wanderChangeInterval)
        {
            PickRandomDirection();
            wanderTimer = 0f;
        }
        // 方向向いて歩く
        Quaternion targetRot = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            rotateSpeed * Time.deltaTime
        );
        rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
    }
    // -------------------------
    // :サイコロ: ランダム方向を決める
    // -------------------------
    void PickRandomDirection()
    {
        Vector2 rand = Random.insideUnitCircle.normalized;
        moveDirection = new Vector3(rand.x, 0, rand.y);
    }
    // -------------------------
    // :れんが: 壁にぶつかったら方向転換
    // -------------------------
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Wall"))
        {
            // 壁の反対方向へ向く
            Vector3 away = transform.position - col.contacts[0].point;
            away.y = 0;
            moveDirection = away.normalized;
        }
    }
}


