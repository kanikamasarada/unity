using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private PauseMenu pauseMenu;

    private bool isOpen = false;
    public bool IsOpen => isOpen;       // ★ 状態取得用

    void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }

    void Update()
    {
        // PauseMenu中はTabキー無効
        if (pauseMenu != null && pauseMenu.IsPaused)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            if (inventoryPanel != null)
                inventoryPanel.SetActive(isOpen);
        }
    }

    // ★ PauseMenuから呼び出して強制的に閉じる
    public void ForceClose()
    {
        isOpen = false;
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }
}
