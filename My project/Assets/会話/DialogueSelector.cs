using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラ側で「画面中心付近で会話できる DialogueTrigger」を毎フレーム選定するコンポーネント。
/// - メインカメラにアタッチしてください（または検出したいカメラ）。
/// - 選定結果は static CurrentTarget で参照可能。
/// </summary>
public class DialogueSelector : MonoBehaviour
{
    [Header("検出設定")]
    [Tooltip("会話可能な最大距離")]
    public float maxDistance = 3f;

    [Tooltip("画面中心からの許容半径（正規化ビュー座標、0..0.5）")]
    [Range(0.01f, 0.5f)]
    public float viewportRadius = 0.12f;

    [Tooltip("カメラ正面からどれくらいの角度まで許容するか（度）")]
    [Range(0f, 90f)]
    public float maxAngleDeg = 60f;

    [Tooltip("検出するレイヤー（会話対象が乗っているレイヤーをセット）")]
    public LayerMask detectionMask = ~0;

    [Tooltip("複数ターゲットがあったときの優先ルール (true=距離優先, false=画面中心優先)")]
    public bool preferCloser = false;

    // 現在見ている（選定された） DialogueTrigger
    public static DialogueTrigger CurrentTarget { get; private set; }

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>() ?? Camera.main;
        if (cam == null)
            Debug.LogWarning("[DialogueSelector] カメラが見つかりません。MainCamera をシーンに置いてください。");
    }

    void Update()
    {
        UpdateCurrentTarget();
    }

    /// <summary>
    /// 毎フレーム、カメラ中心近傍で最適な DialogueTrigger を選ぶ。
    /// アルゴリズム：
    ///  1) OverlapSphere で近傍のコライダを集める（maxDistance）
    ///  2) 各候補について DialogueTrigger があれば、前方か（dot）／ビュー内か（Viewport z>0）／角度が小さいかを確認
    ///  3) ビュー中心距離（0.5,0.5）やワールド距離を用いて最良候補を選択
    /// </summary>
    void UpdateCurrentTarget()
    {
        CurrentTarget = null;

        if (cam == null) return;

        // OverlapSphere で近くのコライダを集める
        Collider[] hits = Physics.OverlapSphere(cam.transform.position, maxDistance, detectionMask);

        if (hits == null || hits.Length == 0) return;

        Vector2 center = new Vector2(0.5f, 0.5f); // ビューポート中心
        float bestScore = float.MaxValue;
        DialogueTrigger bestTrigger = null;

        foreach (var col in hits)
        {
            if (col == null) continue;

            DialogueTrigger trg = col.GetComponentInParent<DialogueTrigger>(); // コライダが子にある場合も拾う
            if (trg == null) continue;

            // トリガーが会話を拒否中ならスキップ
            if (!trg.enabled || trg.ignore) continue;

            // プレイヤー（カメラ）からターゲット位置へのベクトル
            Vector3 dir = (trg.transform.position - cam.transform.position);
            float dist = dir.magnitude;

            if (dist > maxDistance) continue;

            Vector3 dirNorm = dir.normalized;

            // カメラ正面との角度チェック
            float angle = Vector3.Angle(cam.transform.forward, dirNorm);
            if (angle > maxAngleDeg) continue;

            // ビューポート座標へ投影
            Vector3 vp = cam.WorldToViewportPoint(trg.transform.position);

            // 後ろに回っている（z <= 0）は除外
            if (vp.z <= 0f) continue;

            // ビュー中心からの距離（正規化）
            Vector2 vp2 = new Vector2(vp.x, vp.y);
            float centerDist = Vector2.Distance(vp2, center);

            // 中心からの距離が閾値より大きければ除外
            if (centerDist > viewportRadius) continue;

            // スコア化：小さいほど良い
            // preferCloser==true の場合は距離優先、そうでなければ画面中心優先
            float score = preferCloser ? dist : centerDist;
            // 微調整で両方を考慮する（副次スコア）
            score += (preferCloser ? centerDist * 0.1f : dist * 0.1f);

            if (score < bestScore)
            {
                bestScore = score;
                bestTrigger = trg;
            }
        }

        CurrentTarget = bestTrigger;
    }

    // デバッグ用ギズモ
    void OnDrawGizmosSelected()
    {
        if (cam == null) cam = GetComponent<Camera>() ?? Camera.main;
        if (cam == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(cam.transform.position, maxDistance);

        // ビューポート半径の可視化（ワールド空間で近似線）
#if UNITY_EDITOR
        // draw a cone outline approximating viewportRadius at maxDistance
        Vector3 center = cam.transform.position + cam.transform.forward * maxDistance;
        // compute a world offset corresponding to viewportRadius at that distance
        Vector3 right = cam.transform.right * (viewportRadius * 2f * maxDistance * Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad * 0.5f));
        Gizmos.color = new Color(1f, 0.8f, 0f, 0.3f);
        Gizmos.DrawWireSphere(center, viewportRadius * maxDistance * 0.5f);
#endif
    }
}
