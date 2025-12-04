using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(InventorySlotUI))]
public class InventorySlotDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private InventorySlotUI thisSlot;
    private Vector3 originalPosition;

    public Image iconImage;

    private Canvas canvas;

    private void Awake()
    {
        thisSlot = GetComponent<InventorySlotUI>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
            Debug.LogError("Canvas が見つかりません (InventorySlotDrag)");

        if (iconImage == null && thisSlot != null)
            iconImage = thisSlot.iconImage;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (thisSlot == null || thisSlot.currentItem == null) return;

        originalPosition = iconImage.rectTransform.localPosition;

        // ドラッグ中は最前面に
        iconImage.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (thisSlot.currentItem == null) return;

        // Overlay ならそのままスクリーン座標をローカル座標へ変換するだけで OK
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            null, // Overlay → Camera は null
            out localPos
        );

        iconImage.rectTransform.localPosition = localPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (thisSlot.currentItem == null) return;

        GameObject targetObj = eventData.pointerCurrentRaycast.gameObject;
        if (targetObj != null)
        {
            InventorySlotUI targetSlot = targetObj.GetComponent<InventorySlotUI>();
            if (targetSlot == null)
                targetSlot = targetObj.GetComponentInParent<InventorySlotUI>();

            if (targetSlot != null && targetSlot != thisSlot)
            {
                InventoryManager.Instance?.TryCombineItems(thisSlot, targetSlot);
            }
        }

        iconImage.rectTransform.localPosition = originalPosition;
    }
}
