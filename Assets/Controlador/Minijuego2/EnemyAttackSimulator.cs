using UnityEngine;
using System.Collections;

public class EnemyAttackSimulator : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;              // El personaje principal
    public GameObject enemyPrefab;        // Prefab del enemigo
    public Transform[] spawnPoints;       // Lugares desde donde aparecen los enemigos

    [Header("Parámetros")]
    public float attackInterval = 2f;     // Tiempo entre ataques
    public float attackSpeed = 5f;        // Velocidad con la que los enemigos se mueven hacia el jugador
    public float fadeSpeed = 2f;          // Velocidad del desvanecimiento

    [Header("Control de dificultad")]
    public bool enableSimulation = true;  // Booleano para activar o desactivar el sistema (puedes probar con esto)

    private bool playerDodging = false;

    void Start()
    {
        //if (enableSimulation)
        //StartCoroutine(EnemyAttackLoop());
    }
    
    public void atacar()
    {
        if (enableSimulation)
        StartCoroutine(EnemyAttackLoop());
    }

    IEnumerator EnemyAttackLoop()
    {
        //while (enableSimulation)
        //{
            SpawnEnemy();
            yield return new WaitForSeconds(attackInterval);
        //}
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null || player == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        StartCoroutine(MoveEnemyTowardsPlayer(enemy));
    }

    IEnumerator MoveEnemyTowardsPlayer(GameObject enemy)
    {
        SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
        if (sr == null) sr = enemy.AddComponent<SpriteRenderer>(); // Evita errores si no hay renderer

        while (enemy != null && Vector3.Distance(enemy.transform.position, player.position) > 0.1f)
        {
            enemy.transform.position = Vector3.MoveTowards(
                enemy.transform.position,
                player.position,
                attackSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Cuando llega al jugador
        StartCoroutine(FadeAndDestroy(sr));
    }

    IEnumerator FadeAndDestroy(SpriteRenderer sr)
    {
        Color color = sr.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            sr.color = color;
            yield return null;
        }
        Destroy(sr.gameObject);
    }

    // Este método lo puedes llamar desde tu otro script (por ejemplo, cuando el jugador acierta)
    public void PlayerDodge()
    {
        if (!playerDodging)
            StartCoroutine(DodgeAnimation());
    }

    IEnumerator DodgeAnimation()
    {
    playerDodging = true;

    Vector3 originalPos = player.position;

    // Límite horizontal del movimiento (ajústalos a tu escenario)
    float leftLimit = -3f;
    float rightLimit = 3f;

    // Elegir dirección aleatoria: -1 (izquierda) o +1 (derecha)
    int direction = Random.value < 0.5f ? -1 : 1;

    // Distancia de desplazamiento
    float dodgeDistance = 0.5f;

    // Calcular nueva posición dentro de los límites
    float targetX = Mathf.Clamp(originalPos.x + (dodgeDistance * direction), leftLimit, rightLimit);
    Vector3 dodgePos = new Vector3(targetX, originalPos.y, originalPos.z);

    // Movimiento suave
    float duration = 0.2f;
    float t = 0f;

    while (t < duration)
    {
        player.position = Vector3.Lerp(originalPos, dodgePos, t / duration);
        t += Time.deltaTime;
        yield return null;
    }

    // Aseguramos posición final exacta
    player.position = dodgePos;

    playerDodging = false;
    }
}
