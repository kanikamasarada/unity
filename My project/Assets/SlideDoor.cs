using UnityEngine;

public class SlideDoor : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(3f, 0f, 0f); // 開くときの移動量
    public float speed = 2f;                             // 開閉速度
    public float triggerDistance = 3.5f;                   // ドアが反応する距離

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;

    private Transform player;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;

        // "Player"タグを持つオブジェクトを取得
        player = GameObject.FindWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Playerタグが付いたオブジェクトが見つかりません！");
        }
    }

    void Update()
    {
        if (player == null) return;

        // プレイヤーとの距離をチェック
        float distance = Vector3.Distance(transform.position, player.position);

        // 距離内ならEキーで開閉
        if (distance < triggerDistance && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;

            Debug.Log("ドアを開けた！");

        }
    }
}

