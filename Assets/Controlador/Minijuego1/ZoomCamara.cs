using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ZoomCamara : MonoBehaviour
{
    public CinemachineVirtualCamera camaraVirtual;
    public float zoomNormal = 5f;
    public float zoomAmpliado = 8f;
    public float velocidadZoom = 2f;
    public PopupTester popupTester;
    private float zoomObjetivo;
    private int contador = 1;
    private void Start()
    {
        zoomObjetivo = zoomNormal;
    }

    private void Update()
    {
        // Cambiar zoom con una tecla (ejemplo: Z)
        if (popupTester==true && contador==1)
        {
            zoomObjetivo = (zoomObjetivo == zoomNormal) ? zoomAmpliado : zoomNormal;
            contador = 2;
        }

        // Interpolar suavemente el zoom actual hacia el objetivo
        camaraVirtual.m_Lens.OrthographicSize = Mathf.Lerp(
            camaraVirtual.m_Lens.OrthographicSize,
            zoomObjetivo,
            Time.deltaTime * velocidadZoom
        );
    }
}
