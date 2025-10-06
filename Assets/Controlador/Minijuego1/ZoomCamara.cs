using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ZoomCamara : MonoBehaviour
{
    [Header("Referencias")]
    public CinemachineVirtualCamera camaraVirtual;
    public PopupTester popupTester;

    [Header("Zoom")]
    public float zoomNormal = 2f;
    public float zoomAmpliado = 6f;
    public float velocidadZoom = 2f;

    [Header("Límites de cámara (Confiner2D)")]
    public Collider2D limitesNormales;
    public Collider2D limitesZoom;

    private CinemachineConfiner2D confiner;
    private float zoomObjetivo;
    private int contador = 1;
    private bool usandoZoomAmpliado = false;

    private void Start()
    {
        if (camaraVirtual == null)
            camaraVirtual = GetComponent<CinemachineVirtualCamera>();

        confiner = camaraVirtual.GetComponent<CinemachineConfiner2D>();

        // Valor inicial
        zoomObjetivo = zoomNormal;

        if (confiner != null && limitesNormales != null)
            confiner.m_BoundingShape2D = limitesNormales;
    }

    private void Update()
    {
        // Activar zoom una vez cuando termine el mensaje
        if (popupTester.terminoMensaje1 == true && contador == 1)
        {
            AlternarZoom();
            contador = 2; // evita repetir
        }

        // Interpolar suavemente el zoom actual hacia el objetivo
        camaraVirtual.m_Lens.OrthographicSize = Mathf.Lerp(
            camaraVirtual.m_Lens.OrthographicSize,
            zoomObjetivo,
            Time.deltaTime * velocidadZoom
        );
    }

    private void AlternarZoom()
    {
        usandoZoomAmpliado = !usandoZoomAmpliado;
        zoomObjetivo = usandoZoomAmpliado ? zoomAmpliado : zoomNormal;

        if (confiner != null)
        {
            // Asignar el nuevo collider de límites
            confiner.m_BoundingShape2D = usandoZoomAmpliado ? limitesZoom : limitesNormales;

            // Forzar recalcular la caché de colisión del confiner
            StartCoroutine(ForzarActualizacionConfiner());
        }
    }
    private IEnumerator ForzarActualizacionConfiner()
    {
        // Desactivar temporalmente el confiner para forzar el refresco
        confiner.enabled = false;
        yield return null; // esperar un frame
        confiner.enabled = true;

        // Invalidar la caché y recalcular
        confiner.InvalidateCache();
    }
}
