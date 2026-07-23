using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;

    public void FadeIn(float time, EaseType ease)
    {
        StartCoroutine(FadeInCoroutine(time, ease));
    }

    public void FadeOut(float time, EaseType ease)
    {
        StartCoroutine(FadeOutCoroutine(time, ease));
    }

    IEnumerator FadeInCoroutine(float time, EaseType ease)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float raw = Mathf.Clamp01(t / time);
            float eased = ApplyEase(ease, raw);

            fadeImage.color = new Color(0, 0, 0, 1 - eased);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    IEnumerator FadeOutCoroutine(float time, EaseType ease)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float raw = Mathf.Clamp01(t / time);
            float eased = ApplyEase(ease, raw);

            fadeImage.color = new Color(0, 0, 0, eased);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);
    }

    float ApplyEase(EaseType type, float t)
    {
        return type switch
        {
            EaseType.Linear => Ease.Linear(t),
            EaseType.InQuad => Ease.InQuad(t),
            EaseType.OutQuad => Ease.OutQuad(t),
            EaseType.InOutQuad => Ease.InOutQuad(t),
            EaseType.InCubic => Ease.InCubic(t),
            EaseType.OutCubic => Ease.OutCubic(t),
            EaseType.InOutCubic => Ease.InOutCubic(t),
            EaseType.InQuart => Ease.InQuart(t),
            EaseType.OutQuart => Ease.OutQuart(t),
            EaseType.InOutQuart => Ease.InOutQuart(t),
            EaseType.InQuint => Ease.InQuint(t),
            EaseType.OutQuint => Ease.OutQuint(t),
            EaseType.InOutQuint => Ease.InOutQuint(t),
            EaseType.EaseOutBack => Ease.EaseOutBack(t),
            EaseType.EaseInBack => Ease.EaseInBack(t),
            EaseType.EaseInOutBack => Ease.EaseInOutBack(t),
            _ => t
        };
    }
}
