using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject hitCirclePrefab;
    public RectTransform spawnArea;

    [Header("Ajustes")]
    public float spawnInterval = 2f;

    private float timer;

    public bool minijuego2; //Es el booleano que activa el minijuego, se podria llamar desde el gameManager 

    public int vida;
    public Slider slider;


    private void Start()
    {
        vida = 3;
        slider.maxValue = vida;
    }

    void Update()
    {
        if (minijuego2)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnHitCircle();
                timer = 0f;
            }
        }

        slider.value = (float)vida;
    }

    public void QuitarVida()
    {
        StartCoroutine(MenosVida());
    }

    IEnumerator MenosVida()
    {
       
        yield return new WaitForSeconds(1.3f);
        vida--;
    }

    void SpawnHitCircle()
    {
        if (hitCirclePrefab == null || spawnArea == null)
        {
            Debug.LogWarning("Falta asignar hitCirclePrefab o spawnArea en el inspector.");
            return;
        }

        // Generar posición aleatoria dentro del área
        Vector2 randomPos = new Vector2(
            Random.Range(spawnArea.rect.xMin, spawnArea.rect.xMax),
            Random.Range(spawnArea.rect.yMin, spawnArea.rect.yMax)
        );

        // Instanciar círculo
        GameObject newCircle = Instantiate(hitCirclePrefab, spawnArea);
        RectTransform rect = newCircle.GetComponent<RectTransform>();

        if (rect != null)
            rect.anchoredPosition = randomPos;
        else
            Debug.LogWarning("El prefab del círculo necesita un RectTransform.");
    }
}
