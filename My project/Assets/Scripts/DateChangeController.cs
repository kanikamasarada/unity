using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DateChangeController : MonoBehaviour
{
    public Image fadeImage;           // 黒いImage
    public TextMeshProUGUI dateText;  // 日付表示Text
    public float fadeDuration = 1f;   // フェード時間

    // 寝るボタンから呼ばれる
    public void ChangeDateWithFade()
    {
        StartCoroutine(FadeAndChangeDateRoutine());
    }

    private IEnumerator FadeAndChangeDateRoutine()
    {
        // フェードアウト
        yield return StartCoroutine(Fade(0f, 1f));

        // 日付更新（OnDateChanged 発火 → DateObjectBehaviour 変化）
        GameDateManager.Instance.NextDay();

        // 日付表示更新
        int currentDay = GameDateManager.Instance.day;
        dateText.text = currentDay + "日目";
        dateText.enabled = true;

        // 1秒表示
        yield return new WaitForSeconds(1f);

        // フェードイン
        yield return StartCoroutine(Fade(1f, 0f));

        dateText.enabled = false;
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
