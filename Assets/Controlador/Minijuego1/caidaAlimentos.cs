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
            popupTester.showMessage4=true;
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
                timerAudio.Play();
                //Recoge el tiempo con el que trabaja el computador(?
                timer -= Time.deltaTime;
                //Vamos actualizando el texto en pantalla
                txtTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
            } 
            
            if (timer > 0 && timer <= 15 && contador2==1) 
            {
                timerAudio.Stop();
                timerAudiox2.Play();
                popupTester.blinkAutoTrigger = true;
                contador2 = 2;
                popupTester.showMessage5 = true;
                    

                //Aquí lo del aumento de dificultad
                
            }
            else if (timer <= 0)
            {
                //Aquí lo que se hace cuando se acaba el tiempo (gana)
                enMinijuego1 = false;
                temporizador.SetActive(false);
                timerAudiox2.Stop();


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
