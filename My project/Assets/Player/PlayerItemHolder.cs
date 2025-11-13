using UnityEngine;
public class PlayerItemHolder : MonoBehaviour
{
    public static PlayerItemHolder Instance;
    [Header("装備アイテム設定")]
    public Transform playerCameraOrBody;
    public Vector3 holdOffset = new Vector3(0.5f, -0.3f, 1.0f);
    public Vector3 rotationOffset = Vector3.zero;
    public Vector3 scale = Vector3.one;
    [Header("キー設定")]
    public KeyCode equipKey = KeyCode.E;
    public KeyCode unequipKey = KeyCode.Q;
    private GameObject currentItemObj;
    private ItemData currentItemData;
    void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        if (currentItemObj != null && playerCameraOrBody != null)
        {
            currentItemObj.transform.position = playerCameraOrBody.position +
                                                playerCameraOrBody.TransformDirection(holdOffset);
            currentItemObj.transform.rotation = playerCameraOrBody.rotation * Quaternion.Euler(rotationOffset);
        }
        if (Input.GetKeyDown(equipKey))
        {
            TryEquipLastSelectedItem();
        }
        if (Input.GetKeyDown(unequipKey))
        {
            UnequipItem();
        }
    }
    public void EquipItem(ItemData item)
    {
        UnequipItem();
        if (item == null || item.worldPrefab == null)
        {
            Debug.LogWarning("装備失敗: itemまたはprefabがnull");
            return;
        }
        currentItemData = item;
        currentItemObj = Instantiate(item.worldPrefab);
        currentItemObj.transform.localScale = scale;
        Debug.Log($"装備: {item.itemName}");
    }
    public void UnequipItem()
    {
        if (currentItemObj != null)
        {
            Destroy(currentItemObj);
            currentItemObj = null;
        }
        currentItemData = null;
        Debug.Log("素手に戻った");
    }
    private void TryEquipLastSelectedItem()
    {
        if (currentItemData != null)
        {
            Debug.Log($"{currentItemData.itemName} はすでに装備中");
            return;
        }
        if (InventoryManager.Instance != null && InventoryManager.Instance.LastSelectedItem != null)
        {
            EquipItem(InventoryManager.Instance.LastSelectedItem);
        }
        else
        {
            Debug.Log("装備できるアイテムが選択されていません");
        }
    }
    public bool HasEquipped() => currentItemData != null;
}





