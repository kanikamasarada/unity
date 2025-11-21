using UnityEngine;
using UnityEngine.UI;

public class SleepUI : MonoBehaviour
{
    public GameObject panel;
    public Button sleepButton;
    public Button cancelButton;
    public DateChangeController dateChanger;

    [Header("寝た後に Fog density を増やす量")]
    public float fogDensityIncrease = 0.9f;  // ← これで濃くなる。必要なら0.02や0.03に

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
    Debug.Log("睡眠ボタンが押された！");

    // ① フェードアウト後に Fog 増加
    // ② フェード全部終わった後に呼ばれる処理（今は使わないので null）
    dateChanger.ChangeDateWithFade(
        afterFadeOut: IncreaseFogDensity, 
        onComplete: null
    );

    Hide();
}
    private void OnCancelClicked()
    {
        Hide();
    }

    // フェード後に実行
    private void OnFadeComplete()
    {
        IncreaseFogDensity();
    }

    private void IncreaseFogDensity()
    {
        if (!RenderSettings.fog)
        {
        Debug.LogWarning("Fog が OFF のため density は変更されません！");
        return;
        }

    float density = RenderSettings.fogDensity;

        density += fogDensityIncrease;

    // 上限を 0.3 に変更（必要ならもっと上げてもOK）
        density = Mathf.Clamp(density, 0f, 1f);

    RenderSettings.fogDensity = density;

    Debug.Log($"Fog density 増加 → 現在: {density}");
}
}
