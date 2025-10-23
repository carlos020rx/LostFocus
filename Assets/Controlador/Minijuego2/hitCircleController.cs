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
    public float shrinkDuration = 1.5f;
    public float startScale = 2f;
    public float endScale = 1f;

    [Header("Rango de acierto (en porcentaje de tiempo)")]
    [Range(0f, 1f)] public float perfectStart = 0.85f;
    [Range(0f, 1f)] public float perfectEnd = 0.95f;

    [Header("Ajustes de Dificultad")]
    public bool enableMovement = false;   // ✅ Activa movimiento y desvanecimiento. Activa la forma dificil, 
    public float moveRadius = 100f;
    public float moveSpeed = 1.5f;
    public float fadeSpeed = 2.5f;          // ✅ Velocidad de desvanecimiento

    private float timer;
    private int contador=1;
    private bool wasPressed = false;

    public GameObject fallo, acierto;

    private Image outerCircleImage;
    private Image innerCircleImage;
    private Color originalColor;

    private RectTransform rectTransform;
    private Vector2 basePosition;
    private Vector2 randomOffset;


    void Start()
    {
        timer = 0f;
        shrinkDuration = 1.5f;
        feedbackText.text = "";

        rectTransform = GetComponent<RectTransform>();
        outerCircle.localScale = Vector3.one * startScale;

        // Guardar colores originales
        outerCircleImage = outerCircle.GetComponent<Image>();
        innerCircleImage = innerCircle.GetComponent<Image>();

        if (outerCircleImage != null)
            originalColor = outerCircleImage.color;

        basePosition = rectTransform.anchoredPosition;
        randomOffset = GetRandomOffset();

        //spawner = spawner.GetComponent<Spawner>();
    }

    void Update()
    {
        if (outerCircle == null) return;

        timer += Time.deltaTime;
        float progress = timer / shrinkDuration;
        float currentScale = Mathf.Lerp(startScale, endScale, progress);
        outerCircle.localScale = Vector3.one * currentScale;

        // ✅ Movimiento y desvanecimiento solo si está activado
        if (enableMovement)
        {
            MoveWholePrefab();
            FadeInnerCircle();
        }

        if (!wasPressed)
        {
            bool circleClosed = outerCircle.localScale.x <= endScale + 0.01f; // pequeño margen de error

            if (timer >= shrinkDuration && circleClosed)
            {
                if (contador == 1)
                {
                    FindObjectOfType<EnemyAttackSimulator>().atacar();
                    contador = 2;
                    Debug.Log(timer);
                    FindObjectOfType<Spawner>().QuitarVida();

                }

                ShowFeedback("Fallo!", Color.red);
                fallo.SetActive(true);
                CambiarColor(Color.red);

                
            }
        }

    }


    void MoveWholePrefab()
    {
        Vector2 target = basePosition + randomOffset;
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, target, Time.deltaTime * moveSpeed);

        if (Vector2.Distance(rectTransform.anchoredPosition, target) < 5f)
        {
            randomOffset = GetRandomOffset();
        }
    }

    void FadeInnerCircle()
    {
        if (innerCircleImage == null) return;

        Color currentColor = innerCircleImage.color;
        // Gradualmente baja el alpha (transparencia)
        currentColor.a = Mathf.Lerp(0.5f, 0.01f, timer / shrinkDuration * fadeSpeed);
        innerCircleImage.color = currentColor;
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
            FindObjectOfType<EnemyAttackSimulator>().PlayerDodge();
        }
        else
        {
            FindObjectOfType<EnemyAttackSimulator>().atacar();
            ShowFeedback("Fallo!", Color.red);
            FindObjectOfType<Spawner>().vida--;
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
