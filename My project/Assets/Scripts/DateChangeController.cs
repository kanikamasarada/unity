using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DateChangeController : MonoBehaviour
{
   public Image fadeImage;           // ����Image
    public TextMeshProUGUI dateText;  // ���t�\��Text
    public float fadeDuration = 1f;   // �t�F�[�h����

    // �Q��{�^������Ă΂��
    public void ChangeDateWithFade()
    {
        StartCoroutine(FadeAndChangeDateRoutine());
    }

    private IEnumerator FadeAndChangeDateRoutine()
    {
        // �t�F�[�h�A�E�g
        yield return StartCoroutine(Fade(0f, 1f));

        // ���t�X�V�iOnDateChanged ���� �� DateObjectBehaviour �ω��j
        GameDateManager.Instance.NextDay();

        // ���t�\���X�V
        int currentDay = GameDateManager.Instance.day;
        dateText.text = currentDay + "����";
        dateText.enabled = true;

        // 1�b�\��
        yield return new WaitForSeconds(1f);

        // �t�F�[�h�C��
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
