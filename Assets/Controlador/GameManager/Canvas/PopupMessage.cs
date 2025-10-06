using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class PopupMessage : MonoBehaviour
{
    [Header("Animación")]
    [SerializeField] private float appearDuration = 0.25f;
    [SerializeField] private float disappearDuration = 0.18f;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);


    public float DisappearDuration => disappearDuration;
    public bool IsVisible => isVisible;

    [Header("Pulso")]
    [SerializeField] private float pulseScale = 1.08f;
    [SerializeField] private float pulseSpeed = 1.3f;

    private RectTransform rect;
    private Coroutine playRoutine;
    private Coroutine pulseRoutine;
    private Vector3 originalScale;
    private bool isVisible;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale == Vector3.zero ? Vector3.one : rect.localScale;

        

        // Estado inicial oculto
        rect.localScale = Vector3.zero;
        
        gameObject.SetActive(false);
    }

    public void Show(float lifetimeSeconds = 5f, bool pulse = false)
    {
        if (playRoutine != null) StopCoroutine(playRoutine);
        if (pulseRoutine != null) StopCoroutine(pulseRoutine);

        gameObject.SetActive(true);
        playRoutine = StartCoroutine(ShowRoutine(lifetimeSeconds, pulse));
    }

    public void HideImmediate()
    {
        if (playRoutine != null) StopCoroutine(playRoutine);
        if (pulseRoutine != null) StopCoroutine(pulseRoutine);
        rect.localScale = Vector3.zero;
        
        gameObject.SetActive(false);
        isVisible = false;
    }

    public void HideAnimated()
    {
        if (!isVisible) return;
        if (playRoutine != null) StopCoroutine(playRoutine);
        if (pulseRoutine != null) StopCoroutine(pulseRoutine);
        playRoutine = StartCoroutine(HideRoutine());
    }

    private IEnumerator ShowRoutine(float lifetimeSeconds, bool pulse)
    {
        isVisible = true;

        // Aparecer (escala 0 -> 1 con curva)
        float t = 0f;
        rect.localScale = Vector3.zero;
       

        while (t < appearDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / appearDuration);
            float s = scaleCurve.Evaluate(k);
            rect.localScale = Vector3.LerpUnclamped(Vector3.zero, originalScale, s);
            
            yield return null;
        }

        rect.localScale = originalScale;
        

        // Pulso (opcional)
        if (pulse)
            pulseRoutine = StartCoroutine(PulseLoop());

        // Vida útil
        yield return new WaitForSeconds(lifetimeSeconds);

        // Ocultar
        if (pulseRoutine != null) StopCoroutine(pulseRoutine);
        pulseRoutine = null;
        yield return HideRoutine();
    }

    private IEnumerator HideRoutine()
    {
        float t = 0f;
        Vector3 start = rect.localScale;

        while (t < disappearDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / disappearDuration);
            float s = 1f - k;
            rect.localScale = Vector3.LerpUnclamped(start, Vector3.zero, k);
            
            yield return null;
        }

        rect.localScale = Vector3.zero;
        
        gameObject.SetActive(false);
        isVisible = false;
        playRoutine = null;
    }

    private IEnumerator PulseLoop()
    {
        // Bucle de escala 1 -> pulseScale -> 1
        while (true)
        {
            // Crecer
            float t = 0f;
            while (t < 1f / pulseSpeed)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t * pulseSpeed);
                rect.localScale = Vector3.Lerp(originalScale, originalScale * pulseScale, k);
                yield return null;
            }
            // Encoger
            t = 0f;
            while (t < 1f / pulseSpeed)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t * pulseSpeed);
                rect.localScale = Vector3.Lerp(originalScale * pulseScale, originalScale, k);
                yield return null;
            }
        }
    }
}