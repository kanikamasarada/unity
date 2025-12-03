using UnityEngine;

public class PlayerItemHolder : MonoBehaviour
{
    public static PlayerItemHolder Instance;

    [Header("装備アイテム設定")]
    public Transform playerCameraOrBody;

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
            currentItemObj.transform.position =
                playerCameraOrBody.position +
                playerCameraOrBody.TransformDirection(currentItemData.worldPositionOffset);

            currentItemObj.transform.rotation =
                playerCameraOrBody.rotation *
                Quaternion.Euler(currentItemData.worldRotationOffset);
        }

        if (Input.GetKeyDown(equipKey))
            TryEquipLastSelectedItem();

        if (Input.GetKeyDown(unequipKey))
            UnequipItem();
    }

    public void EquipItem(ItemData item)
    {
        UnequipItem();

        if (item == null || item.worldPrefab == null)
        {
            Debug.LogError("EquipItem: item または worldPrefab が null です");
            return;
        }

        currentItemData = item;
        currentItemObj = Instantiate(item.worldPrefab);

        // 手に持つ位置・回転・スケールの調整
        currentItemObj.transform.position =
            playerCameraOrBody.position +
            playerCameraOrBody.TransformDirection(item.worldPositionOffset);

        currentItemObj.transform.rotation =
            playerCameraOrBody.rotation *
            Quaternion.Euler(item.worldRotationOffset);

        currentItemObj.transform.localScale = item.worldScale;

        // Prefab の Light を有効化（もしあれば）
        var lights = currentItemObj.GetComponentsInChildren<Light>();
        foreach (var l in lights)
            l.enabled = true;

        // Prefab の ParticleSystem を再生（もしあれば）
        var particles = currentItemObj.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particles)
            ps.Play();

        Debug.Log($"装備: {item.itemName}");
    }

    public void UnequipItem()
    {
        if (currentItemObj != null)
            Destroy(currentItemObj);

        currentItemObj = null;
        currentItemData = null;
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
