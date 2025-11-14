using UnityEngine;
using System.Collections;

public class NPCPatron : MonoBehaviour
{
    [Header("Puntos de patrulla")]
    public Transform[] puntos;
    public float velocidad = 2f;
    public float tiempoEspera = 2f;

    public bool caminarAhora;

    private bool caminar2 = true;

    [Header("Animator")]
    public Animator animator;
    public string walkAnimParam = "isWalking2";

    private int indiceActual = 0;

    void Start()
    {

    }

    void Update()
    {
        if (puntos.Length > 0 && caminarAhora)
        {
            caminarAhora = false;
            StartCoroutine(Patrullar(0));
        }


    }

    public void siguientePaso(int obj)
    {
        StartCoroutine(Patrullar(obj));
    }

    public IEnumerator Patrullar(int obj)
    {


        while (true)
        {
            animator.SetBool(walkAnimParam, true);

            Transform objetivo = puntos[obj];

            // GIRAR HACIA EL PUNTO (2D)
            Vector3 scale = transform.localScale;
            if (objetivo.position.x > transform.position.x)
                scale.x = Mathf.Abs(scale.x);      // mirar derecha
            else
                scale.x = -Mathf.Abs(scale.x);     // mirar izquierda
            transform.localScale = scale;

            // MOVERSE HACIA EL PUNTO
            while (Vector3.Distance(transform.position, objetivo.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    objetivo.position,
                    velocidad * Time.deltaTime
                );

                yield return null;
            }

            animator.SetBool(walkAnimParam, false);

            yield return new WaitForSeconds(tiempoEspera);

            indiceActual = indiceActual % puntos.Length;
        }
    }





}
