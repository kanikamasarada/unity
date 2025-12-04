using UnityEngine;

public class MoveLoopOnPlayer : MonoBehaviour
{
    public string targetTag = "Player"; // 近づきを判定するタグ
    public float triggerDistance = 5f;  // 反応距離
    public float moveDistance = 5f;     // どれだけ動くか
    public float speed = 3f;            // 動く速度

    private bool isPlayerInside = false;
    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + transform.right * moveDistance;  // 横方向に移動
    }

    void Update()
    {
        // ---- プレイヤーの位置を取得 ----
        GameObject player = GameObject.FindGameObjectWithTag(targetTag);
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.transform.position);

        // ---- プレイヤーが近くにいるかどうか ----
        if (dist < triggerDistance)
        {
            isPlayerInside = true;
        }
        else
        {
            isPlayerInside = false;
        }

        // ---- プレイヤーがいる間ループ移動 ----
        if (isPlayerInside)
        {
            // PingPong で行ったり来たり
            float t = Mathf.PingPong(Time.time * speed, 1f);
            transform.position = Vector3.Lerp(startPos, endPos, t);
        }
        else
        {
            // 離れたらスタート位置に戻る
            transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * speed);
        }
    }
}
