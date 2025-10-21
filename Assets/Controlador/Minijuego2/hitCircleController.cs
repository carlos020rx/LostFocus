using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HitCircleController : MonoBehaviour
{
    [Header("Referencias")]
    public RectTransform outerCircle;
    public RectTransform innerCircle;
    public TextMeshProUGUI feedbackText;

    [Header("Ajustes")]
    public float shrinkDuration = 2f;
    public float startScale = 2f;
    public float endScale = 1f;

    [Header("Rango de acierto (en porcentaje de tiempo)")]
    [Range(0f, 1f)] public float perfectStart = 0.85f;
    [Range(0f, 1f)] public float perfectEnd = 0.95f;

    [Header("Ajustes de Dificultad")]
    public bool enableMovement = false;  // ✅ Activa movimiento de toda la instancia
    public float moveRadius = 100f;
    public float moveSpeed = 1.5f;

    private float timer;
    private bool wasPressed = false;

    public GameObject fallo, acierto;

    private Image outerCircleImage;
    private Color originalColor;

    // Movimiento
    private RectTransform rectTransform;
    private Vector2 basePosition;
    private Vector2 randomOffset;

    void Start()
    {
        timer = 0f;
        feedbackText.text = "";

        rectTransform = GetComponent<RectTransform>(); // ✅ Se moverá este transform
        outerCircle.localScale = Vector3.one * startScale;

        // Guardar color original
        outerCircleImage = outerCircle.GetComponent<Image>();
        if (outerCircleImage != null)
            originalColor = outerCircleImage.color;

        // Guardar posición inicial de todo el prefab
        basePosition = rectTransform.anchoredPosition;
        randomOffset = GetRandomOffset();
    }

    void Update()
    {
        if (outerCircle == null) return;

        timer += Time.deltaTime;
        float progress = timer / shrinkDuration;
        float currentScale = Mathf.Lerp(startScale, endScale, progress);
        outerCircle.localScale = Vector3.one * currentScale;

        // ✅ Movimiento del prefab completo
        if (enableMovement)
        {
            MoveWholePrefab();
        }

        if (timer >= shrinkDuration && !wasPressed)
        {
            ShowFeedback("Fallo!", Color.red);
            fallo.SetActive(true);
            CambiarColor(Color.red);
        }
    }

    void MoveWholePrefab()
    {
        Vector2 target = basePosition + randomOffset;
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, target, Time.deltaTime * moveSpeed);

        // Cuando llega al destino, genera otro punto
        if (Vector2.Distance(rectTransform.anchoredPosition, target) < 5f)
        {
            randomOffset = GetRandomOffset();
        }
    }

    Vector2 GetRandomOffset()
    {
        return new Vector2(Random.Range(-moveRadius, moveRadius), Random.Range(-moveRadius, moveRadius));
    }

    public void OnInnerClick()
    {
        if (wasPressed) return;

        wasPressed = true;
        float progress = timer / shrinkDuration;

        if (progress >= perfectStart && progress <= perfectEnd)
        {
            ShowFeedback("Perfecto!", Color.green);
            acierto.SetActive(true);
            CambiarColor(Color.green);
        }
        else
        {
            ShowFeedback("Fallo!", Color.red);
            fallo.SetActive(true);
            CambiarColor(Color.red);
        }
    }

    void CambiarColor(Color nuevoColor)
    {
        if (outerCircleImage != null)
            outerCircleImage.color = nuevoColor;
    }

    void ShowFeedback(string message, Color color)
    {
        if (feedbackText == null) return;

        feedbackText.text = message;
        feedbackText.color = color;
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        if (outerCircleImage != null)
            outerCircleImage.color = originalColor;

        Destroy(gameObject);
    }
}
