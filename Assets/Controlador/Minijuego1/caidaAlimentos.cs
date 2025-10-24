using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class caidaAlimentos : MonoBehaviour
{
    [SerializeField] GameObject[] fruits;
    [SerializeField] float spam = 0.5f;
    [SerializeField] float minTras;
    [SerializeField] float maxTras;
    public PopupTester popupTester;
    public int contador = 1;
    private int contador2 = 1;
    public AudioSource timerAudio;
    public AudioSource timerAudiox2;

    public GameObject limIzq, limDer;

    [Header("Temporizador")]
    private float timer;
    private int minutos, segundos;
    [SerializeField] private TMP_Text txtTiempo;
    [SerializeField, Tooltip("tiempo en segundos")] private float tiempoRestante = 30f;
    public GameObject temporizador;

    public bool enMinijuego1;
    public bool murio,miniJuegoFrutas;

    public int vida;
    public Slider slider;

    public Animation transition;

    public bool finMinijuego1;


    // Start is called before the first frame update
    public void Start()
    {
        miniJuegoFrutas = true;
        murio = false;
        limIzq.SetActive(false);
        limDer.SetActive(false);

        txtTiempo.text = "";
        timer = tiempoRestante;
        enMinijuego1 = false;
        temporizador.SetActive(false);

        vida = 3;
        slider.maxValue = vida;
    }
    public IEnumerator fruitsSpawn()
    {
        while(miniJuegoFrutas)
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
            popupTester.showMessage4=true;
            StartCoroutine(fruitsSpawn());
            contador = 2;
            ponerLimites();
            enMinijuego1 = true;
            Debug.Log("Hola1");
        }

        minutos = (int)(timer / 60f);
        segundos = (int)(timer - minutos * 60f);

        if (enMinijuego1)
        {
            temporizador.SetActive(true);
            if (timer > 0)
            {
                timerAudio.Play();
                //Recoge el tiempo con el que trabaja el computador(?
                timer -= Time.deltaTime;
                //Vamos actualizando el texto en pantalla
                txtTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
            }

            if (timer > 0 && timer <= 15 && contador2 == 1)
            {
                timerAudio.Stop();
                timerAudiox2.Play();
                popupTester.blinkAutoTrigger = true;
                contador2 = 2;
                popupTester.showMessage5 = true;


                //Aqu� lo del aumento de dificultad

            }
            else if (timer <= 0)
            {
                //Aqu� lo que se hace cuando se acaba el tiempo (gana)
                timerAudio.Stop();
                timerAudiox2.Stop();
                enMinijuego1 = false;
                temporizador.SetActive(false);
                timerAudiox2.Stop();
                miniJuegoFrutas = false;
                finMinijuego1 = true;
                quitarLimites();


            }
        }

        slider.value = (float)vida;

        if (vida == 0  && murio == false)
        {
            transition.Play();
            murio = true;
            enMinijuego1 = false;
            timer = 30f;
            miniJuegoFrutas = false;

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
