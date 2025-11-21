using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DateChangeController : MonoBehaviour
{
    [Header("フェード設定")]
    public Image fadeImage;           // フェード用Image（キャンバス上の黒いImage）
    public float fadeDuration = 1f;   // フェード時間

    [Header("日付テキスト（どちらか使用）")]
    public TextMeshProUGUI dateText_TMP;
    public Text dateText_Legacy;

    // -----------------------
    // public API
    // -----------------------

    // 引数なしで呼ぶ場合（コールバック無し）
    public void ChangeDateWithFade()
    {
        StartCoroutine(FadeAndChangeDateRoutine(null));
        Debug.Log($"Fog変更関数が実行！ 現在:{RenderSettings.fogDensity}");

    }

    // コールバック付きで呼ぶ場合
    public void ChangeDateWithFade(Action onComplete)
    {
        StartCoroutine(FadeAndChangeDateRoutine(onComplete));
    }

    // -----------------------
    // 実装コルーチン（共通）
    // -----------------------
    private IEnumerator FadeAndChangeDateRoutine(Action onComplete)
    {
        // フェードアウト（画面暗転）
        yield return StartCoroutine(Fade(0f, 1f));

        // 日付を進める（GameDateManager側で処理）
        if (GameDateManager.Instance != null)
        {
            GameDateManager.Instance.NextDay();
        }
        else
        {
            Debug.LogWarning("GameDateManager.Instance が見つかりません（NextDay をスキップ）");
        }

        // 日付イベントが各オブジェクトに行き渡るよう 1 フレーム待つ
        yield return null;

        // 日付テキスト更新＆表示
        int currentDay = (GameDateManager.Instance != null) ? GameDateManager.Instance.day : 0;
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

        // 日付表示を短時間キープ（必要なら変更）
        yield return new WaitForSeconds(1f);

        // フェードイン（画面明るく）
        yield return StartCoroutine(Fade(1f, 0f));

        // テキストを非表示に戻す
        if (dateText_TMP != null) dateText_TMP.enabled = false;
        if (dateText_Legacy != null) dateText_Legacy.enabled = false;

        // 最後にユーザー側のコールバックを呼ぶ（あれば）
        onComplete?.Invoke();
    }

    // -----------------------
    // フェード処理
    // -----------------------
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (fadeImage == null)
        {
            Debug.LogWarning("fadeImage がアサインされていません。フェード処理をスキップします。");
            yield break;
        }

        float t = 0f;
        Color c = fadeImage.color;
        c.a = startAlpha;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t / fadeDuration);
            c.a = alpha;
            fadeImage.color = c;
            yield return null;
        }

        c.a = endAlpha;
        fadeImage.color = c;
    }
    // コールバック付きで呼ぶ場合
public void ChangeDateWithFade(Action afterFadeOut, Action onComplete)
{
    StartCoroutine(FadeAndChangeDateRoutine(afterFadeOut, onComplete));
}

private IEnumerator FadeAndChangeDateRoutine(Action afterFadeOut, Action onComplete)
{
    // フェードアウト（画面暗転）
    yield return StartCoroutine(Fade(0f, 1f));

    // ★ フェードアウト直後コールバック（Fogとかここで変える）
    afterFadeOut?.Invoke();

    // 日付変更
    GameDateManager.Instance?.NextDay();
    yield return null;

    // UI更新
    int currentDay = GameDateManager.Instance?.day ?? 0;
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

    yield return new WaitForSeconds(1f);

    // フェードイン
    yield return StartCoroutine(Fade(1f, 0f));

    // テキスト非表示
    if (dateText_TMP != null) dateText_TMP.enabled = false;
    if (dateText_Legacy != null) dateText_Legacy.enabled = false;

    // 最後のコールバック
    onComplete?.Invoke();
}

}
