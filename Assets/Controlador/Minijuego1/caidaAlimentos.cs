using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class caidaAlimentos : MonoBehaviour
{
    [SerializeField] GameObject[] fruits;
    [SerializeField] float spam = 0.5f;
    [SerializeField] float minTras;
    [SerializeField] float maxTras;
    public PopupTester popupTester;
    private int contador = 1;

    public GameObject limIzq, limDer;

    [Header("Temporizador")]
    private float timer;
    private int minutos, segundos;
    [SerializeField] private TMP_Text txtTiempo;
    [SerializeField, Tooltip("tiempo en segundos")] private float tiempoRestante = 30f;
    public GameObject temporizador;

    private bool enMinijuego1;


    // Start is called before the first frame update
    void Start()
    {
        limIzq.SetActive(false);
        limDer.SetActive(false);

        txtTiempo.text = "";
        timer = tiempoRestante;
        enMinijuego1 = false;
        temporizador.SetActive(false);
    }
    IEnumerator fruitsSpawn()
    {
        while(true)
        {
            var wanted = Random.Range(minTras, maxTras);
            var position = new Vector3 (wanted, transform.position.y);
            GameObject gameObject = Instantiate(fruits[Random.Range(0,fruits.Length)],position,Quaternion.identity);
            yield return new WaitForSeconds(spam);
            Destroy(gameObject,2f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (popupTester.terminoMensaje1 == true && contador==1)
        {

            StartCoroutine(fruitsSpawn());
            contador = 2;
            ponerLimites();
            enMinijuego1 = true;
        }

        minutos = (int)(timer / 60f);
        segundos = (int)(timer - minutos * 60f);

        if (enMinijuego1)
        {
            temporizador.SetActive(true);
            if (timer > 0)
            {

                //Recoge el tiempo con el que trabaja el computador(?
                timer -= Time.deltaTime;
                //Vamos actualizando el texto en pantalla
                txtTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
            } 
            
            if (timer > 0 && timer <= 15) 
            {
                //Aqu� lo del aumento de dificultad
                popupTester.blinkAutoTrigger = true;
            }
            else if (timer <= 0)
            {
                //Aqu� lo que se hace cuando se acaba el tiempo (gana)
                enMinijuego1 = false;
                temporizador.SetActive(false);


            }
        }
       

    }

    private void ponerLimites()
    {
        limIzq.SetActive(true);
        limDer.SetActive(true);
    }

    private void quitarLimites()
    {
        limIzq.SetActive(false);
        limDer.SetActive(false);
    }

}
