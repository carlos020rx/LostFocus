using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cambia entre paneles (UI) con una animación de fade.
/// La entrada por teclado (Q) está separada: ReadKeyboardInput() -> OnQPressed() -> NextPanel().
/// </summary>
public class TVController : MonoBehaviour
{
    [Header("Paneles (en orden)")]
    [Tooltip("Cada panel debe tener un CanvasGroup")]
    public List<CanvasGroup> panels = new List<CanvasGroup>();

    [Header("Tiempos (editable en el Inspector)")]
    [Min(0f)] public float fadeOutDuration = 0.25f;
    [Min(0f)] public float fadeInDuration = 0.25f;
    [Min(0f)] public float holdBetween = 0.00f; // pausa entre fade out e in

    [Header("Opciones")]
    [Tooltip("Si es false, al llegar al último no avanza más.")]
    public bool wrap = true;

    [Header("Curvas de animación")]
    public AnimationCurve easeOut = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve easeIn = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private int current = 0;
    private bool busy = false;
    public int contador = 0;
    public int contador2 = 0;
    void Awake()
    {
        InitializePanels();
    }

    void Update()
    {
        ReadKeyboardInput(); // Lógica de lectura de teclado separada
    }

    /// <summary>
    /// Solo lee entrada. Si se presiona Q, delega a OnQPressed()
    /// </summary>
    public void ReadKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            contador += 1;
            contador2 += 1;
            if (contador2 == 2 && contador<9)
            {
                NextPanel();
                contador2 = 0;
            }
            //OnQPressed();
            if (contador > 8)
            {
                NextPanel();
            }
        }
    }

    /// <summary>
    /// Punto único donde reaccionas a la Q.
    /// Se mantiene separado y solo invoca el cambio de imagen.
    /// </summary>
    //public void OnQPressed()
    //{
    //    NextPanel();
    //}

    /// <summary>
    /// Cambia al siguiente panel con transición.
    /// </summary>
    public void NextPanel()
    {
        if (busy || panels.Count == 0) return;

        int next = current + 1;
        if (next >= panels.Count)
        {
            if (!wrap) return;
            next = 0;
        }

        StartCoroutine(FadeSwap(current, next));
        current = next;
    }

    /// <summary>
    /// Inicializa estados de los paneles (uno visible, resto ocultos).
    /// </summary>
    public void InitializePanels()
    {
        if (panels.Count == 0) return;

        for (int i = 0; i < panels.Count; i++)
        {
            var cg = panels[i];
            if (!cg) continue;

            bool isCurrent = (i == current);
            if (!cg.gameObject.activeSelf) cg.gameObject.SetActive(true);

            cg.alpha = isCurrent ? 1f : 0f;
            cg.interactable = isCurrent;
            cg.blocksRaycasts = isCurrent;
        }
    }

    /// <summary>
    /// Permite ir a un índice específico desde UI/botón si quieres.
    /// </summary>
    public void GoTo(int index)
    {
        if (busy || panels.Count == 0) return;
        index = Mathf.Clamp(index, 0, panels.Count - 1);
        if (index == current) return;

        StartCoroutine(FadeSwap(current, index));
        current = index;
    }

    private IEnumerator FadeSwap(int fromIndex, int toIndex)
    {
        busy = true;

        // Asegura que estén activos
        for (int i = 0; i < panels.Count; i++)
            if (panels[i] && !panels[i].gameObject.activeSelf)
                panels[i].gameObject.SetActive(true);

        var from = panels[fromIndex];
        var to = panels[toIndex];

        // Estados iniciales
        to.alpha = 0f;
        to.interactable = false;
        to.blocksRaycasts = false;

        // Fade Out del actual
        float t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.unscaledDeltaTime; // UI suele ir mejor no escalada
            float k = fadeOutDuration > 0f ? Mathf.Clamp01(t / fadeOutDuration) : 1f;
            float a = 1f - easeOut.Evaluate(k);
            if (from) from.alpha = a;
            yield return null;
        }
        if (from)
        {
            from.alpha = 0f;
            from.interactable = false;
            from.blocksRaycasts = false;
        }

        if (holdBetween > 0f)
            yield return new WaitForSecondsRealtime(holdBetween);

        // Fade In del siguiente
        t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.unscaledDeltaTime;
            float k = fadeInDuration > 0f ? Mathf.Clamp01(t / fadeInDuration) : 1f;
            float a = easeIn.Evaluate(k);
            if (to) to.alpha = a;
            yield return null;
        }
        if (to)
        {
            to.alpha = 1f;
            to.interactable = true;
            to.blocksRaycasts = true;
        }

        busy = false;
    }
}
