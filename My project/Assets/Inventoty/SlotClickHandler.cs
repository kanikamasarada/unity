using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SlotClickHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("参照")]
    public Image slotImage;             // スロット内の画像
    public GameObject iconPrefab;       // 生成するアイコンのプレハブ

    [Header("アイテムごとの表示先")]
    public Transform kagiTarget;        // 鍵の表示位置
    public Transform karaageTarget;     // 唐揚げくんの表示位置
    public Transform bedTarget;         // ベッドの表示位置
    // 必要に応じて増やせる

    // アイテム識別用（Slot にセットされる想定）
    public ItemData currentItem;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (slotImage == null || slotImage.sprite == null || currentItem == null)
            return;

        // 表示先を決める
        Transform targetPos = GetTargetTransform(currentItem.itemName);

        if (targetPos == null)
        {
            Debug.LogWarning($"[{currentItem.itemName}] に対応する表示先が設定されていません");
            return;
        }

        // アイコンを生成
        GameObject icon = Instantiate(iconPrefab, targetPos.position, Quaternion.identity, targetPos);
        icon.GetComponent<Image>().sprite = slotImage.sprite;
    }

    private Transform GetTargetTransform(string itemName)
    {
        switch (itemName)
        {
            case "鍵":
            case "Key":
                return kagiTarget;
            case "からあげくん":
            case "KaraageKun":
                return karaageTarget;
            case "ベッド":
            case "Bed":
                return bedTarget;
            default:
                return null;
        }
    }
}
