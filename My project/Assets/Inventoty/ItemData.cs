using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;     // アイテム名
    public Sprite icon;         // アイテム画像
    [TextArea]
    public string description;  // アイテム説明
}
