using UnityEngine;
using TMPro;

public class PickupMessage : MonoBehaviour
{
    public static PickupMessage Instance; // シングルトン用
    public TextMeshProUGUI messageText;
    public float displayTime = 2f;

    private float timer = 0f;

    private void Awake()
    {
        Instance = this; // シングルトンとして登録

        // アイテムを取る前は非表示にする
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (messageText != null && messageText.gameObject.activeSelf)
        {
            timer += Time.deltaTime;
            if (timer > displayTime)
            {
                messageText.gameObject.SetActive(false);
            }
        }
    }

    public void ShowMessage(string itemName)
    {
        if (messageText == null) return;

        messageText.text = $"{itemName} を手に入れた";
        messageText.gameObject.SetActive(true);
        timer = 0f;
    }
}
