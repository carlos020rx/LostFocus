using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivacionTv : MonoBehaviour
{
    [Header("Objeto que se activará")]
    public GameObject objetoAActivar;

    [Header("Solo el jugador activa")]
    public string tagActivador = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagActivador))
        {
            if (objetoAActivar != null)
            {
                objetoAActivar.SetActive(true);
                Debug.Log("Objeto activado por colisión con " + other.name);
            }
        }
    }
}
