using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomCamaraController : MonoBehaviour
{
    [Header("VCams")]
    public CinemachineVirtualCamera vcamNormal; // la vcam de siempre
    public CinemachineVirtualCamera vcamZoom;   // la vcam para zoom ampliado

    [Header("Control")]
    public PopupTester popupTester; // referencia que ya tienes (terminoMensaje1)
    public bool autoRevert = false; // si quieres que vuelva solo después de N segundos
    public float revertAfterSeconds = 3f;

    [Header("Priorities")]
    public int normalPriority = 20;
    public int zoomPriority = 40; // debe ser mayor que normalPriority

    // guardamos prioridades originales por si se necesitan
    private int origNormalPriority;
    private int origZoomPriority;

    private bool zoomActivo = false;

    private int contador = 1;

    private void Start()
    {
        contador = 1;
        if (vcamNormal == null || vcamZoom == null)
        {
            Debug.LogError("Asignar vcamNormal y vcamZoom en el inspector.");
            enabled = false;
            return;
        }

        // guardar prioridades iniciales
        origNormalPriority = vcamNormal.Priority;
        origZoomPriority = vcamZoom.Priority;

        // Asegurar estado inicial: normal activo (mayor prioridad)
        vcamNormal.Priority = normalPriority;
        vcamZoom.Priority = normalPriority - 10;
    }

    private void Update()
    {
        // activación única por tu PopupTester
        if (popupTester.terminoMensaje1 == true)
        {
            Debug.Log("Hola");

            if (contador == 1)
            {
                contador = 2;
                SetZoom(true);
                Debug.Log("mmmm");
            }
            

            if (autoRevert)
                StartCoroutine(RevertAfterDelay(revertAfterSeconds));
        }
    }

    public void SetZoom(bool activar)
    {
        if (activar == zoomActivo) return;

        zoomActivo = activar;

        if (activar)
        {
            // poner la vcamZoom con mayor prioridad para que Cinemachine la active
            vcamZoom.Priority = zoomPriority;
            vcamNormal.Priority = normalPriority;
        }
        else
        {
            // devolver prioridades al estado donde la normal domina
            vcamZoom.Priority = normalPriority - 10;
            vcamNormal.Priority = normalPriority;
        }

        // (opcional) imprime para debugging
        Debug.Log($"SetZoom({activar}) -> vcamNormal.Priority={vcamNormal.Priority}, vcamZoom.Priority={vcamZoom.Priority}");
    }

    private IEnumerator RevertAfterDelay(float secs)
    {
        yield return new WaitForSeconds(secs);
        SetZoom(false);
    }
}
