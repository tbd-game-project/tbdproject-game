using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// TransitionSettingsの設定を使用してフェードアウトを行う
public sealed class FadeController : MonoBehaviour
{
    [SerializeField]
    private Image       fadeImage;

    [SerializeField]
    private CanvasGroup fadeCanvasGroup;

    private void Awake()
    {
        fadeCanvasGroup.alpha = 0.0f;
    }

    public IEnumerator FadeOut(TransitionSettings settings)
    {
        fadeImage.color = settings.FadeColor;

        yield return ChangeAlpha(0.0f, 1.0f, settings.FadeDuration);
    }

    public IEnumerator FadeIn(TransitionSettings settings)
    {
        fadeImage.color = settings.FadeColor;

        yield return ChangeAlpha(1.0f ,0.0f, settings.FadeDuration);
    }

    private IEnumerator ChangeAlpha(float startAlpha, float endAlpha, float duration)
    {
        if (duration <= 0.0f)
        {
            fadeCanvasGroup.alpha = endAlpha;
            yield break;
        }

        float elapsedTime = 0.0f;

        fadeCanvasGroup.alpha = startAlpha;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(elapsedTime / duration);

            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
    }
}