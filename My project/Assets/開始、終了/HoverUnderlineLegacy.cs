using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class HoverUnderlineLegacy : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("対象テキスト（レガシー Text）")]
    public Text targetText;

    [Header("下線用 Image")]
    public Image underlineImage;

    [Header("アニメーション設定")]
    public float animSpeed = 6f;
    public float underlineHeight = 2f;
    public float underlineOffset = 5f;

    private RectTransform underlineRect;
    private float targetWidth;
    private float maxWidth;

    void Start()
    {
        if (targetText == null)
        {
            Debug.LogError($"{name}: targetText が設定されていません。");
            return;
        }

        if (underlineImage == null)
        {
            Debug.LogError($"{name}: underlineImage が設定されていません。");
            return;
        }

        underlineRect = underlineImage.GetComponent<RectTransform>();

        // 下線をテキストの幅に合わせて初期化
        LayoutRebuilder.ForceRebuildLayoutImmediate(targetText.GetComponent<RectTransform>());
        maxWidth = targetText.preferredWidth;
        underlineRect.sizeDelta = new Vector2(0f, underlineHeight);


        targetWidth = 0f;
    }

    void Update()
    {
        if (underlineRect == null) return;

        float currentWidth = underlineRect.sizeDelta.x;
        float desiredWidth = Mathf.Lerp(currentWidth, targetWidth * maxWidth, Time.deltaTime * animSpeed);
        underlineRect.sizeDelta = new Vector2(desiredWidth, underlineHeight);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetWidth = 1f; // 全開
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetWidth = 0f; // 閉じる
    }
}
