using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(InventorySlotUI))]
public class InventorySlotDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private InventorySlotUI thisSlot;
    private Image iconImage;

    void Awake()
    {
        // スロット取得
        thisSlot = GetComponent<InventorySlotUI>();
        if (thisSlot == null)
            Debug.LogError("InventorySlotUI がこのオブジェクトにありません！");

        // Image取得
        iconImage = GetComponent<Image>();
        if (iconImage == null)
            iconImage = GetComponentInChildren<Image>();
        if (iconImage == null)
            Debug.LogError("InventorySlotDrag の Image が見つかりません！");

        // Canvas取得
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            Debug.LogError("InventorySlotDrag が Canvas の子になっていません！");

        // CanvasGroup追加
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (thisSlot == null || iconImage == null || iconImage.sprite == null || canvas == null)
            return;

        originalParent = transform.parent;
        transform.SetParent(canvas.transform, true);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (iconImage == null) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (thisSlot == null)
        {
            transform.localPosition = Vector3.zero;
            return;
        }

        transform.SetParent(originalParent, true);
        canvasGroup.blocksRaycasts = true;

        // ドロップ先取得
        GameObject targetObj = eventData.pointerCurrentRaycast.gameObject;
        if (targetObj == null)
        {
            transform.localPosition = Vector3.zero;
            return;
        }

        // InventorySlotUI を探す（子も含む）
        InventorySlotUI targetSlot = targetObj.GetComponent<InventorySlotUI>();
        if (targetSlot == null)
            targetSlot = targetObj.GetComponentInParent<InventorySlotUI>();

        if (targetSlot != null && targetSlot != thisSlot)
        {
            InventoryManager.Instance?.TryCombineItems(thisSlot, targetSlot);
        }

        // 常に元の位置に戻す
        transform.localPosition = Vector3.zero;
    }
}
