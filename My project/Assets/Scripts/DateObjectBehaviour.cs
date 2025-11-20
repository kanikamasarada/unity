using UnityEngine;
using System.Collections;

public class DateObjectBehaviour : MonoBehaviour
{
    public Renderer targetRenderer;
    public DayChangeData[] changes;

    private DayChangeData currentData;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;

        if (GameDateManager.Instance != null)
        {
            GameDateManager.Instance.OnDateChanged += CheckDate;
        }

        CheckDate(); // 初回適用
    }

    void OnEnable()
    {
        if (GameDateManager.Instance != null)
            GameDateManager.Instance.OnDateChanged += CheckDate;
    }

    void OnDisable()
    {
        if (GameDateManager.Instance != null)
            GameDateManager.Instance.OnDateChanged -= CheckDate;
    }

    void Update()
    {
        if (currentData == null || targetRenderer == null || player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        // 範囲内 → distanceTexture
        if (dist <= currentData.distanceRange && currentData.distanceTexture != null)
        {
            targetRenderer.material.mainTexture = currentData.distanceTexture;
        }
        // 範囲外 → newTexture
        else if (currentData.newTexture != null)
        {
            targetRenderer.material.mainTexture = currentData.newTexture;
        }
    }

    // ---------------------
    // 日付が変わった時の処理
    // ---------------------
    void CheckDate()
    {
        int day = GameDateManager.Instance.day;
        currentData = null;

        foreach (var c in changes)
        {
            if (c.day == day)
            {
                currentData = c;

                // 位置変更
                transform.position = c.newPosition;

                // オブジェクトON/OFF
                gameObject.SetActive(c.active);

                // 初期テクスチャー（通常の）
                if (targetRenderer != null && c.newTexture != null)
                    targetRenderer.material.mainTexture = c.newTexture;

                break;
            }
        }
    }
}
