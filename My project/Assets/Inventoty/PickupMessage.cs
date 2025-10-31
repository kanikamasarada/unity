using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PickupMessage : MonoBehaviour
{
    public static PickupMessage Instance; // シングルトン

    [Header("テキスト設定（どちらか使用）")]
    public TextMeshProUGUI messageText_TMP;  // TextMeshPro 用
    public Text messageText_Legacy;          // レガシーUI用

    [Header("表示時間設定")]
    public float displayTime = 2f;

    private float timer = 0f;

    private void Awake()
    {
        Instance = this;

        // 初期状態では非表示にする
        if (messageText_TMP != null)
            messageText_TMP.gameObject.SetActive(false);

        if (messageText_Legacy != null)
            messageText_Legacy.gameObject.SetActive(false);
    }

    private void Update()
    {
        bool isActiveTMP = messageText_TMP != null && messageText_TMP.gameObject.activeSelf;
        bool isActiveLegacy = messageText_Legacy != null && messageText_Legacy.gameObject.activeSelf;

        if (isActiveTMP || isActiveLegacy)
        {
            timer += Time.deltaTime;
            if (timer > displayTime)
            {
                if (messageText_TMP != null)
                    messageText_TMP.gameObject.SetActive(false);

                if (messageText_Legacy != null)
                    messageText_Legacy.gameObject.SetActive(false);
            }
        }
    }

    public void ShowMessage(string itemName)
    {
        string displayText = $"{itemName}を手に入れた";
        timer = 0f;

        if (messageText_TMP != null)
        {
            messageText_TMP.text = displayText;
            messageText_TMP.gameObject.SetActive(true);
        }

        if (messageText_Legacy != null)
        {
            messageText_Legacy.text = displayText;
            messageText_Legacy.gameObject.SetActive(true);
        }
    }
}
