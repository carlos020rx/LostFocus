using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Spawner : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject hitCirclePrefab;
    public RectTransform spawnArea;
    public PopupTester popupTester2;
    [Header("Ajustes")]
    public float spawnInterval = 2f;

    private float timer;

    [Header("Control del minijuego")]
    public bool minijuego2 = false; // Activa el minijuego

    public int vida;
    public Slider slider;

    [Header("Temporizador")]
    private float timerTemporizador;
    private int minutos, segundos;
    [SerializeField] private TMP_Text txtTiempo;
    [SerializeField, Tooltip("tiempo en segundos")] private float tiempoRestante = 45f;
    public GameObject temporizador;
    private int contador = 1;
    
    public GameObject panelTransition;
    public Animation transition;

    public bool murio2 = false;



    //public AudioSource timerAudio;
    //public AudioSource timerAudiox2;

    private void Start()
    {
        vida = 3;
        slider.maxValue = vida;

        txtTiempo.text = "";
        timerTemporizador = tiempoRestante;
        minijuego2 = false;
        temporizador.SetActive(false);
    }

    void Update()
    {
        if (popupTester2.terminoMensaje1 == true && contador == 1)
        {
            popupTester2.showMessage4 = true;
            contador = 2;
            minijuego2 = true;
        }
        minutos = (int)(timerTemporizador / 60f);
        segundos = (int)(timerTemporizador - minutos * 60f);

        

        if (minijuego2)
        {
            popupTester2.showMessage4 = true;
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnHitCircle();
                timer = 0f;
            }

            temporizador.SetActive(true);
            if (timerTemporizador > 0)
            {
                //timerAudio.Play();
                //Recoge el tiempo con el que trabaja el computador(?
                timerTemporizador -= Time.deltaTime;
                //Vamos actualizando el texto en pantalla
                txtTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
            }

            if (timerTemporizador > 0 && timerTemporizador <= 15 && contador == 2)
            {
                //timerAudio.Stop();
                //timerAudiox2.Play();
                contador = 2;

                //Aquí lo del aumento de dificultad
                popupTester2.showMessage5 = true;
                HitCircleController.modoDificilGlobal = true;

            }
            else if (timerTemporizador <= 0)
            {
                //Aquí lo que se hace cuando se acaba el tiempo (gana)
                minijuego2 = false;
                temporizador.SetActive(false);
                //timerAudiox2.Stop();


            }
        }

        slider.value = (float)vida;

        if (vida == 0  && murio2 == false)
        {
            Debug.Log("Perdiste");
            panelTransition.SetActive(true);
            transition.Play();
            murio2 = true;
            minijuego2 = false;
            timer = 45f;

        }
        


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

        HitCircleController circleCtrl = newCircle.GetComponent<HitCircleController>();
        if (circleCtrl != null && HitCircleController.modoDificilGlobal)
            circleCtrl.enableMovement = true;

    }
}
