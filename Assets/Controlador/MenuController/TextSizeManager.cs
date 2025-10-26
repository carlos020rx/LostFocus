using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TextSizeManager : MonoBehaviour
{
    public static TextSizeManager Instance;

    [Header("Tamaños de texto")]
    [SerializeField] private float[] scaleLevels = { 0.85f, 1f, 1.25f }; // Pequeño, medio, grande

    [Header("Referencias UI")]
    [SerializeField] private Image sizeIndicator;      // Imagen que representa el tamaño actual
    [SerializeField] private Sprite[] sizeIcons;       // Array de 3 imágenes (pequeña, mediana, grande)

    private int currentSizeIndex = 1; // 0=pequeño, 1=mediano, 2=grande
    private Dictionary<TextMeshProUGUI, float> baseSizes = new();
    private void Awake()
    {
        // Singleton (única instancia global)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Cuando se carga una escena, volver a aplicar el tamaño
        SceneManager.sceneLoaded += (scene, mode) => ApplyTextSize();
    }
    private void Start()
    {
        CacheBaseSizes();
        UpdateUI();
        ApplyTextSize();
    }

    private void CacheBaseSizes()
    {
        baseSizes.Clear();
        var allTexts = FindObjectsOfType<TextMeshProUGUI>(true);
        foreach (var tmp in allTexts)
        {
            if (tmp.CompareTag("IgnoreTextResize")) continue;
            if (!baseSizes.ContainsKey(tmp))
                baseSizes[tmp] = tmp.fontSize; // Guardar el tamaño base
        }
    }

    // Llamado por el botón +
    public void IncreaseSize()
    {
        currentSizeIndex = Mathf.Min(currentSizeIndex + 1, scaleLevels.Length - 1);
        ApplyTextSize();
        UpdateUI();
    }

    // Llamado por el botón -
    public void DecreaseSize()
    {
        currentSizeIndex = Mathf.Max(currentSizeIndex - 1, 0);
        ApplyTextSize();
        UpdateUI();
    }


    // Cambia todos los textos de la escena
    private void ApplyTextSize()
    {
        float scale = scaleLevels[currentSizeIndex];
        var allTexts = FindObjectsOfType<TextMeshProUGUI>(true);

        foreach (var tmp in allTexts)
        {
            if (tmp.CompareTag("IgnoreTextResize")) continue;

            // Si no está cacheado, guardamos su base actual
            if (!baseSizes.ContainsKey(tmp))
                baseSizes[tmp] = tmp.fontSize;

            tmp.fontSize = baseSizes[tmp] * scale;
        }
    }

    // Actualiza la imagen representativa
    private void UpdateUI()
    {
        if (sizeIndicator != null && sizeIcons != null && sizeIcons.Length == scaleLevels.Length)
            sizeIndicator.sprite = sizeIcons[currentSizeIndex];
    }
}