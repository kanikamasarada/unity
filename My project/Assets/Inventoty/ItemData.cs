using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [Header("ライト設定")]
    public float lightRange = 4f;
    public float lightIntensity = 2f;
    public GameObject flamePrefab;

    public Color lightColor = Color.white;
    [TextArea]
    public string description;
    public GameObject worldPrefab;

    [Header("装備時の見た目設定")]
    public Vector3 worldRotationOffset = Vector3.zero; // 回転調整
    public Vector3 worldPositionOffset = Vector3.zero; // 位置調整
    public Vector3 worldScale = Vector3.one;           // ← 新しく追加：装備時スケール
}
