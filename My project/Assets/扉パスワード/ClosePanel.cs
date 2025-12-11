using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public GameObject panel;

    public void Close()
    {
        panel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
