using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingPanel : MonoBehaviour
{
    [Header("Colores del parpadeo (Alpha fijo en 50%)")]
    [SerializeField]
    private List<Color> colors = new List<Color>() {
        new Color(1f, 0f, 0f, 0.5f), // Rojo 50%
        new Color(0f, 1f, 0f, 0.5f), // Verde 50%
        new Color(0f, 0f, 1f, 0.5f)  // Azul 50%
    };
    //velocidades
    [Header("Velocidad (tiempo por transición)")]
    [SerializeField] private float colorDuration = 1f;
    [Header("Fade Out final")]
    [SerializeField] private float fadeOutDuration = 0.5f;

    private Image panelImage;
    private Coroutine blinkRoutine;
    private Coroutine autoOffRoutine;
    private void Awake()
    {
        panelImage = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Empieza a parpadear indefinidamente
    /// </summary>
    public void StartBlink()
    {
        if (colors.Count < 2)
        {
            Debug.LogWarning("Necesitas al menos 2 colores para parpadear.");
            return;
        }

        if (blinkRoutine != null) StopCoroutine(blinkRoutine);
        if (autoOffRoutine != null) StopCoroutine(autoOffRoutine);

        gameObject.SetActive(true);
        blinkRoutine = StartCoroutine(BlinkLoop());
    }

    /// <summary>
    /// Empieza a parpadear y se apaga tras 'durationSeconds' con fade out
    /// </summary>
    public void StartBlink(float durationSeconds)
    {
        StartBlink();
        autoOffRoutine = StartCoroutine(AutoOff(durationSeconds));
    }

    public void StopBlink()
    {
        if (blinkRoutine != null) StopCoroutine(blinkRoutine);
        if (autoOffRoutine != null) StopCoroutine(autoOffRoutine);
        blinkRoutine = null;
        autoOffRoutine = null;
        gameObject.SetActive(false);
    }

    private IEnumerator BlinkLoop()
    {
        int index = 0;
        while (true)
        {
            Color start = colors[index];
            Color end = colors[(index + 1) % colors.Count];
            float t = 0f;

            while (t < colorDuration)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / colorDuration);
                panelImage.color = Color.Lerp(start, end, k);
                yield return null;
            }

            index = (index + 1) % colors.Count;
        }
    }

    private IEnumerator AutoOff(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        // hacer fade out suave
        yield return FadeOut();
        StopBlink();
    }

    private IEnumerator FadeOut()
    {
        Color initial = panelImage.color;
        float startAlpha = initial.a;
        float t = 0f;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fadeOutDuration);
            float newAlpha = Mathf.Lerp(startAlpha, 0f, k);
            panelImage.color = new Color(initial.r, initial.g, initial.b, newAlpha);
            yield return null;
        }

        // asegurar alpha 0
        panelImage.color = new Color(initial.r, initial.g, initial.b, 0f);
    }
}