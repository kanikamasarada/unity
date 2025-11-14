using UnityEngine;
public class PlayerEquipment : MonoBehaviour
{
    public Transform weaponHolder; // 武器を持たせる位置
    private GameObject currentWeapon;
    public void Equip(ItemData item)
    {
        // 既存武器を削除
        if (currentWeapon != null)
            Destroy(currentWeapon);
        if (item == null || item.worldPrefab == null) return;
        // 新しい武器を生成
        currentWeapon = Instantiate(item.worldPrefab, weaponHolder);
        // ----------------------------
        // 装備時の位置調整を反映
        // ----------------------------
        currentWeapon.transform.localPosition = item.worldPositionOffset;
        currentWeapon.transform.localRotation = Quaternion.Euler(item.worldRotationOffset);
        currentWeapon.transform.localScale = item.worldScale;
    }
}