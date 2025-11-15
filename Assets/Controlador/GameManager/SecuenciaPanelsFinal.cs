using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecuenciaPanelsFinal : MonoBehaviour
{
    [Header("Pantallas negras (CanvasGroup)")]
    public CanvasGroup pantallaNegraInicial;
    public CanvasGroup pantallaNegraFinal;
    public GameObject menuControl;
    [Header("Paneles con fade (CanvasGroup)")]
    public CanvasGroup panelImagen1;

    [Header("Paneles normales (PanelAppear.cs)")]
    public PanelAppear panelNormal1;
    public PanelAppear panelNormal2;

    [Header("Tiempos")]
    public float duracionFadePantallaNegraInicial = 1f;
    public float esperaTrasPantallaNegraInicial = 1f;

    public float duracionFadePanelImagen1 = 1f;
    public float esperaTrasPanelImagen1 = 1f;

    public float esperaTrasPanelNormal1 = 1.5f;

    public float duracionFadePantallaNegraFinal = 1f;
    public float esperaTrasPantallaNegraFinal = 1f;

    public float esperaTrasPanelNormal2 = 2f;

    [Header("Escena siguiente")]
    public string nombreEscenaSiguiente;


    
    void Start()
    {
        PrepararEstadosIniciales();
        
    }
    public void IniciarSecuencia()
    {
        StartCoroutine(Secuencia());
    }
    void PrepararEstadosIniciales()
    {
        SetCanvasGroupInvisible(pantallaNegraInicial);
        SetCanvasGroupInvisible(pantallaNegraFinal);
        SetCanvasGroupInvisible(panelImagen1);

        if (panelNormal1 != null) panelNormal1.gameObject.SetActive(false);
        if (panelNormal2 != null) panelNormal2.gameObject.SetActive(false);
    }

    IEnumerator Secuencia()
    {
        // 1) Fade a pantalla negra inicial
        yield return FadeCanvasGroup(pantallaNegraInicial, 0, 1, duracionFadePantallaNegraInicial);
        yield return new WaitForSeconds(esperaTrasPantallaNegraInicial);

        // 2) Fade del panel imagen 1
        yield return FadeCanvasGroup(panelImagen1, 0, 1, duracionFadePanelImagen1);
        yield return new WaitForSeconds(esperaTrasPanelImagen1);

        // 3) Panel normal 1 aparece con animación por código
        if (panelNormal1 != null)
        {
            panelNormal1.Play();
            yield return new WaitForSeconds(esperaTrasPanelNormal1);
        }

        // 4) Segunda pantalla negra encima
        yield return FadeCanvasGroup(pantallaNegraFinal, 0, 1, duracionFadePantallaNegraFinal);
        yield return new WaitForSeconds(esperaTrasPantallaNegraFinal);

        // 5) Panel normal final
        if (panelNormal2 != null)
        {
            panelNormal2.Play();
            yield return new WaitForSeconds(esperaTrasPanelNormal2);
        }

        // 6) Cambiar de escena
        
        SceneManager.LoadScene(nombreEscenaSiguiente);
    }

    // Helpers ------------------------------------------------

    void SetCanvasGroupInvisible(CanvasGroup cg)
    {
        if (!cg) return;
        cg.gameObject.SetActive(true);
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        cg.gameObject.SetActive(true);
        cg.alpha = from;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);
            cg.alpha = Mathf.Lerp(from, to, k);
            yield return null;
        }

        cg.alpha = to;
    }
}