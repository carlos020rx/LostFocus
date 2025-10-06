using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverlayController : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private float fadeDuration = 0.3f;

    [Tooltip("Usa CanvasGroup para el fade (recomendado). Si lo desactivas, se hace sobre el alpha del Image pero sin tocar el RGB).")]
    [SerializeField] private bool useCanvasGroup = true;

    [Tooltip("Opacidad máxima efectiva del overlay (0..1). No cambia el color del Image, solo la transparencia final.")]
    [Range(0f, 1f)]
    [SerializeField] private float maxAlpha = 0.47f;

    private Image overlayImage;
    private CanvasGroup canvasGroup;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        overlayImage = GetComponent<Image>();

        if (useCanvasGroup)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0f; // arrancar invisible
        }
        else
        {
            // No tocar RGB. Tampoco forzar alpha aquí para no pisar lo que pongas en Inspector.
            // El fade ajustará solo el alpha en runtime.
        }

        gameObject.SetActive(false);
    }

    public void Show()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        fadeRoutine = StartCoroutine(FadeTo(1f));
    }

    public void Hide()
    {
        if (!gameObject.activeInHierarchy) return;
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeTo(0f, disableOnEnd: true));
    }

    private IEnumerator FadeTo(float target, bool disableOnEnd = false)
    {
        float start = useCanvasGroup
            ? canvasGroup.alpha
            : overlayImage.color.a;

        float end = target; // 0 o 1

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fadeDuration);
            float a = Mathf.Lerp(start, end, k) * maxAlpha;

            if (useCanvasGroup)
            {
                canvasGroup.alpha = a; // multiplica opacidad sin tocar color
            }
            else
            {
                // Solo el alpha del Image, preservando RGB
                Color c = overlayImage.color;
                overlayImage.color = new Color(c.r, c.g, c.b, a);
            }

            yield return null;
        }

        // Asegurar valor final exacto
        if (useCanvasGroup)
        {
            canvasGroup.alpha = end * maxAlpha;
        }
        else
        {
            Color c = overlayImage.color;
            overlayImage.color = new Color(c.r, c.g, c.b, end * maxAlpha);
        }

        if (disableOnEnd && end == 0f)
            gameObject.SetActive(false);

        fadeRoutine = null;
    }
}