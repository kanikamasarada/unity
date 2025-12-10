using System;
using UnityEngine;

public class SlideDoor : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(3f, 0f, 0f);
    public float speed = 2f;
    public float triggerDistance = 3.5f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isOpen = false;
    private bool isMoving = false;

    private Transform player;

    public AudioSource sound_effect_open;
    public AudioSource sound_effect_close;

    [NonSerialized] public bool isLocked = false; // 外部からロック

    public float autoCloseDelay = 2f; // ★ 自動で閉めるまでの時間（任意で調整可）
    private bool autoCloseStarted = false;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;

        player = GameObject.FindWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Playerタグが付いたオブジェクトが見つかりません！");
        }
    }

    void Update()
    {
        if (isLocked) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Eキーによる開閉
        if (!isMoving && distance < triggerDistance && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = true;
            isMoving = true;

            if (sound_effect_open != null)
            {
                sound_effect_open.pitch = 1.3f;
                sound_effect_open.Play();
            }

            // ★自動閉鎖コルーチン開始（1回だけ）
            if (!autoCloseStarted)
            {
                StartCoroutine(AutoCloseDoor());
                autoCloseStarted = true;
            }
        }

        // ドア移動
        Vector3 targetPos = isOpen ? openPosition : closedPosition;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (transform.position == targetPos)
        {
            isMoving = false;
        }
    }

    // ★ 2秒待って自動で閉まる処理
    private System.Collections.IEnumerator AutoCloseDoor()
    {
        yield return new WaitForSeconds(autoCloseDelay);

        isOpen = false;     // 閉める
        isMoving = true;

        if (sound_effect_close != null)
            sound_effect_close.Play();

        autoCloseStarted = false; // 次の開閉のためにリセット
    }
}
