using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private InventorySlotUI thisSlot;
    private InventorySlotUI targetSlot;

    void Awake()
    {
        thisSlot = GetComponent<InventorySlotUI>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ドラッグ開始時にアイコン半透明などにしたいならここに追加
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 省略（視覚的な移動処理）
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // ドロップ位置のUI要素を取得
        if (eventData.pointerCurrentRaycast.gameObject == null)
            return;

        targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlotUI>();
        if (targetSlot == null)
            targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<InventorySlotUI>();

        if (targetSlot == null)
        {
            Debug.Log("❌ ドロップ先がスロットではありません");
            return;
        }

        // 合成処理を安全に呼ぶ
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.TryCombineItems(thisSlot, targetSlot);
        }
        else
        {
            Debug.LogError("❌ InventoryManager.Instance が存在しません");
        }
    }
}
