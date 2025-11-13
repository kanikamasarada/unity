/*using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Canvas canvas;
    private Image iconImage;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        iconImage = GetComponent<Image>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (iconImage == null || iconImage.sprite == null) return;

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
        transform.SetParent(originalParent, true);
        canvasGroup.blocksRaycasts = true;

        // ドロップ対象を取得
        GameObject targetObj = eventData.pointerCurrentRaycast.gameObject;
        if (targetObj == null) return;

        var targetSlot = targetObj.GetComponent<InventorySlotUI>();
        var thisSlot = GetComponent<InventorySlotUI>();

        if (targetSlot != null && thisSlot != null && targetSlot != thisSlot)
        {
            InventoryManager.Instance?.TryCombineItems(thisSlot, targetSlot);
        }

        transform.localPosition = Vector3.zero;
    }
}
*/