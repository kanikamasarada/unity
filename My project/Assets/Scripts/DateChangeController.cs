using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DateChangeController : MonoBehaviour
{
    [Header("フェード設定")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("日付テキスト（どちらか使用）")]
    public TextMeshProUGUI dateText_TMP;
    public Text dateText_Legacy;

    private bool firstDayShown = false; // ★追加：ゲーム開始後の初回表示フラグ

    void Start()
    {
        // ★ゲーム開始時に1日目を表示（明るさそのまま）
        StartCoroutine(ShowFirstDayRoutine());
    }

    // ======================================================
    // ★ ゲーム開始後の1回だけ、『1日目』を表示する処理
    // ======================================================
    private IEnumerator ShowFirstDayRoutine()
    {
        if (firstDayShown) yield break;

        firstDayShown = true;

        // 画面フェードアウト（暗転しないように duration = 0）
        yield return StartCoroutine(FadeInstant(1f));

        // UIに1日目表示
        if (GameDateManager.Instance != null)
            GameDateManager.Instance.day = 1;

        UpdateDateText(1);
        EnableDateText(true);

        yield return new WaitForSeconds(1f);

        // フェードイン（ただし明るさは変更しない）
        yield return StartCoroutine(FadeInstant(0f));

        EnableDateText(false);
    }

    // ======================================================
    // ★ Instant用フェード（明るさそのままにするため、時間0で透明度だけ変更）
    // ======================================================
    private IEnumerator FadeInstant(float alpha)
    {
        if (fadeImage == null) yield break;

        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;

        yield return null;
    }

    // ======================================================
    // 通常の日付変更（ベッドで寝た時）暗転あり
    // ======================================================

    public void ChangeDateWithFade()
    {
        if (!CanProceedNextDay()) return; // ★日数チェック追加
        StartCoroutine(FadeAndChangeDateRoutine(null));
    }

    public void ChangeDateWithFade(Action onComplete)
    {
        if (!CanProceedNextDay()) return; // ★日数チェック追加
        StartCoroutine(FadeAndChangeDateRoutine(onComplete));
    }

    public void ChangeDateWithFade(Action afterFadeOut, Action onComplete)
    {
        if (!CanProceedNextDay()) return; // ★日数チェック追加
        StartCoroutine(FadeAndChangeDateRoutine(afterFadeOut, onComplete));
    }


    // ======================================================
    // ★ 次の日に進めて良いか判定する（5日目でストップ）
    // ======================================================
    private bool CanProceedNextDay()
    {
        int current = GameDateManager.Instance?.day ?? 1;

        if (current >= 5)
        {
            Debug.Log("5日目以降は寝れません！");
            return false;
        }

        return true;
    }

    // ======================================================
    // 通常のフェードあり日付変更（ベッド仕様）
    // ======================================================
    private IEnumerator FadeAndChangeDateRoutine(Action onComplete)
    {
        yield return StartCoroutine(Fade(0f, 1f)); // 暗転

        GameDateManager.Instance?.NextDay();
        yield return null;

        int currentDay = GameDateManager.Instance?.day ?? 0;
        UpdateDateText(currentDay);
        EnableDateText(true);

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(Fade(1f, 0f)); // 明るく

        EnableDateText(false);

        onComplete?.Invoke();
    }

    private IEnumerator FadeAndChangeDateRoutine(Action afterFadeOut, Action onComplete)
    {
        yield return StartCoroutine(Fade(0f, 1f)); // 暗転

        afterFadeOut?.Invoke();

        GameDateManager.Instance?.NextDay();
        yield return null;

        int currentDay = GameDateManager.Instance?.day ?? 0;
        UpdateDateText(currentDay);
        EnableDateText(true);

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(Fade(1f, 0f)); // 明るく

        EnableDateText(false);

        onComplete?.Invoke();
    }


    // ======================================================
    // フェード処理（既存のまま）
    // ======================================================
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (fadeImage == null)
        {
            Debug.LogWarning("fadeImage が未設定。フェードスキップ");
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

    // ======================================================
    // UI更新関係
    // ======================================================

    private void UpdateDateText(int day)
    {
        string displayText = day + "日目";

        if (dateText_TMP != null)
            dateText_TMP.text = displayText;

        if (dateText_Legacy != null)
            dateText_Legacy.text = displayText;
    }

    private void EnableDateText(bool enable)
    {
        if (dateText_TMP != null)
            dateText_TMP.enabled = enable;

        if (dateText_Legacy != null)
            dateText_Legacy.enabled = enable;
    }

}
