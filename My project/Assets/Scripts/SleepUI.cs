using UnityEngine;
using UnityEngine.UI;

public class SleepUI : MonoBehaviour
{
    public GameObject panel;
    public Button sleepButton;
    public Button cancelButton;
    public DateChangeController dateChanger;

    void Start()
    {
        panel.SetActive(false);

        sleepButton.onClick.AddListener(OnSleepClicked);
        cancelButton.onClick.AddListener(OnCancelClicked);
    }

    public void Show()
    {
        panel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Hide()
    {
        panel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnSleepClicked()
    {
        Debug.Log("�Q��{�^���������ꂽ�I");
        // ���t�ύX + �t�F�[�h + �I�u�W�F�N�g�ω����܂Ƃ߂ČĂ�
        dateChanger.ChangeDateWithFade();
        

        Hide();
    }

    private void OnCancelClicked()
    {
        Hide();
    }
}
