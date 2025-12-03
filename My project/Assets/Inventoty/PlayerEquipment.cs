using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public Transform weaponHolder;
    private GameObject currentWeapon;

private ItemData currentItem;

public void Equip(ItemData item)
{
    if (item == currentItem) return;
    currentItem = item;

    foreach (Transform child in weaponHolder)
        Destroy(child.gameObject);

    if (item == null || item.worldPrefab == null) return;

    var obj = Instantiate(item.worldPrefab, weaponHolder);

    obj.transform.localPosition = item.worldPositionOffset;
    obj.transform.localRotation = Quaternion.Euler(item.worldRotationOffset);
    obj.transform.localScale = item.worldScale;
}

private void LateUpdate()
{
    if (currentWeapon != null && currentItem != null)
    {
        currentWeapon.transform.localPosition = currentItem.worldPositionOffset;
        currentWeapon.transform.localRotation = Quaternion.Euler(currentItem.worldRotationOffset);
        currentWeapon.transform.localScale = currentItem.worldScale;
    }
}

}
