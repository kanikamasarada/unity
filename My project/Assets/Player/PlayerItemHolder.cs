using UnityEngine;

public class PlayerItemHolder : MonoBehaviour
{
    public static PlayerItemHolder Instance;
    private GameObject currentItem;

    public Vector3 screenOffset = new Vector3(0.8f, 0.5f, 2f);

    private ItemData currentItemData;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (currentItem != null && Camera.main != null)
        {
            // カメラの右端に固定
            Vector3 screenPos = new Vector3(
                Screen.width * screenOffset.x,
                Screen.height * screenOffset.y,
                screenOffset.z
            );

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            // 個別オフセットも反映
            worldPos += Camera.main.transform.TransformDirection(currentItemData.worldPositionOffset);

            currentItem.transform.position = worldPos;

            // カメラ方向に向けつつ個別回転を追加
            Quaternion lookRot = Quaternion.LookRotation(Camera.main.transform.forward);
            currentItem.transform.rotation = lookRot * Quaternion.Euler(currentItemData.worldRotationOffset);
        }
    }

    public void EquipItem(ItemData item)
    {
        if (currentItem != null)
            Destroy(currentItem);

        if (item.worldPrefab == null)
        {
            Debug.LogWarning($"'{item.itemName}' に worldPrefab が設定されていません。");
            return;
        }

        currentItemData = item;
        currentItem = Instantiate(item.worldPrefab);
        Debug.Log($"{item.itemName} を装備（右端固定・個別角度適用）");
    }

    public void UnequipItem()
    {
        if (currentItem != null)
            Destroy(currentItem);
    }
}
