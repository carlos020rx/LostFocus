using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAppear : MonoBehaviour
{
    [Header("Animación")]
    public float duration = 0.6f;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;  // empieza invisible
        gameObject.SetActive(false);         // arranca apagado
    }

    public void Play()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        StopAllCoroutines();
        StartCoroutine(Animar());
    }

    IEnumerator Animar()
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);
            float f = curve.Evaluate(k);
            transform.localScale = Vector3.LerpUnclamped(Vector3.zero, originalScale, f);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}