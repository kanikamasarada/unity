using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(InventorySlotUI))]
public class InventorySlotDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private InventorySlotUI thisSlot;
    private RectTransform rect;
    private Vector3 originalPosition;

    public Image iconImage;

    void Awake()
    {
        thisSlot = GetComponent<InventorySlotUI>();
        rect = GetComponent<RectTransform>();

        if (iconImage == null && thisSlot != null)
            iconImage = thisSlot.iconImage;

        if (iconImage == null)
            Debug.LogError("InventorySlotDrag: iconImage が見つかりません！");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (thisSlot == null || thisSlot.currentItem == null) return;

        originalPosition = iconImage.transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (thisSlot.currentItem == null) return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        iconImage.rectTransform.localPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (thisSlot.currentItem == null) return;

        // ドロップ先スロットを取得
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

        // 合成されなかった場合は元の位置に戻す
        iconImage.transform.localPosition = originalPosition;
    }
}
