using UnityEngine;

public class TargetInfo : MonoBehaviour
{
    [Header("UI表示情報")]
    public string displayName = "？？？";      // 表示する名前
    [TextArea(1, 3)]
    public string hintText = "";               // ヒント（例：話しかける、拾うなど）

    [Header("オプション（例：体力バーなどに拡張予定）")]
    public float health = 1.0f;                // 将来的な体力バーなどに利用可
}
