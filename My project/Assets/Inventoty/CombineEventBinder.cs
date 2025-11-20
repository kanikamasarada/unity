using UnityEngine;
using UnityEngine.Events;

public class CombineRecipeEvent : MonoBehaviour
{
    [System.Serializable]
    public class CombineRecipe
    {
        public ItemData itemA;
        public ItemData itemB;

        [Header("合成後アイテム（空 = 消去のみ & イベント発動 + 追加アイテム適用）")]
        public ItemData resultItem;

        [Header("追加アイテム（resultItem が null の場合のみ使用）")]
        public ItemData[] bonusItems;

        [Header("合成成功時に実行するイベント")]
        public UnityEvent onCombined;
    }

    public CombineRecipe[] recipes;

    void Start()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager が存在しません");
            return;
        }

        foreach (var r in recipes)
        {
            if (r.itemA == null || r.itemB == null)
            {
                Debug.LogWarning("レシピの設定が不完全です（itemA または itemB が未設定）");
                continue;
            }

            InventoryManager.Instance.RegisterCombineEventRecipe(
                r.itemA,
                r.itemB,
                r.resultItem,
                r.onCombined,
                r.bonusItems // ★ここ追加
            );
        }
    }
}
