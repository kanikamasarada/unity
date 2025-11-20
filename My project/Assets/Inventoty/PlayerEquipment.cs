using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public Transform weaponHolder;
    private GameObject currentWeapon;

    public void Equip(ItemData item)
    {
        // 既存の武器を完全に削除
        foreach (Transform child in weaponHolder)
        {
            DestroyImmediate(child.gameObject);
        }

        currentWeapon = null;

        if (item == null || item.worldPrefab == null) return;

        // 新しい武器を生成
        currentWeapon = Instantiate(item.worldPrefab, weaponHolder);

        // 調整
        currentWeapon.transform.localPosition = item.worldPositionOffset;
        currentWeapon.transform.localRotation = Quaternion.Euler(item.worldRotationOffset);
        currentWeapon.transform.localScale = item.worldScale;
    }
}
