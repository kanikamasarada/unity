using UnityEngine;

public class PlayerEquipmentUIConnector : MonoBehaviour
{
    public static PlayerEquipmentUIConnector Instance;
    private PlayerEquipment playerEquip;

    void Awake()
    {
        Instance = this;
        playerEquip = FindFirstObjectByType<PlayerEquipment>();
    }

    public void EquipFromUI(ItemData item)
    {
        // 同じもの再装備防止
        if (playerEquip == null) return;

        playerEquip.Equip(item);
    }
}
