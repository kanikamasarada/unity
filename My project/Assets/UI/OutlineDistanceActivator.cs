using UnityEngine;

[RequireComponent(typeof(Outline))]
public class OutlineDistanceActivator : MonoBehaviour
{
    [Header("プレイヤー設定")]
    [Tooltip("プレイヤーを探すときのタグ名（通常は MainCamera または Player）")]
    public string playerTag = "MainCamera";

    [Header("発光距離設定")]
    [Tooltip("この距離以内に入ったら発光する")]
    public float activateDistance = 3f;

    [Header("発光色設定")]
    public Color glowColor = Color.cyan;

    private Transform player;
    private Outline outline;
    private bool isActive = false;

    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false; // 最初はOFF

        // プレイヤーを探す
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning($"プレイヤー（タグ '{playerTag}'）が見つかりません。");
    }

    void Update()
    {
        if (player == null || outline == null)
            return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= activateDistance && !isActive)
        {
            EnableOutline();
        }
        else if (dist > activateDistance && isActive)
        {
            DisableOutline();
        }
    }

    private void EnableOutline()
    {
        outline.enabled = true;
        outline.OutlineColor = glowColor;
        outline.OutlineWidth = 4f;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        isActive = true;
    }

    private void DisableOutline()
    {
        outline.enabled = false;
        isActive = false;
    }
}
