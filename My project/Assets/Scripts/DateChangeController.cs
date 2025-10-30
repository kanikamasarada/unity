using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DateChangeController : MonoBehaviour
{
    [Header("フェード設定")]
    public Image fadeImage;           // フェード用Image
    public float fadeDuration = 1f;   // フェード時間

    [Header("日付テキスト（どちらか使用）")]
    public TextMeshProUGUI dateText_TMP;  // TextMeshPro 用
    public Text dateText_Legacy;          // レガシーUI用

    // 日付変更を開始（ボタンなどから呼ぶ）
    public void ChangeDateWithFade()
    {
        StartCoroutine(FadeAndChangeDateRoutine());
    }

    private IEnumerator FadeAndChangeDateRoutine()
    {
        // フェードアウト
        yield return StartCoroutine(Fade(0f, 1f));

        // 日付を進める
        GameDateManager.Instance.NextDay();

        // 現在の日付を取得して表示
        int currentDay = GameDateManager.Instance.day;
        string displayText = currentDay + "日目";

        if (dateText_TMP != null)
        {
            dateText_TMP.text = displayText;
            dateText_TMP.enabled = true;
        }

        if (dateText_Legacy != null)
        {
            dateText_Legacy.text = displayText;
            dateText_Legacy.enabled = true;
        }

        // 1秒表示
        yield return new WaitForSeconds(1f);

        // フェードイン
        yield return StartCoroutine(Fade(1f, 0f));

        if (dateText_TMP != null)
            dateText_TMP.enabled = false;

        if (dateText_Legacy != null)
            dateText_Legacy.enabled = false;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, endAlpha, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = endAlpha;
        fadeImage.color = c;
    }
}
