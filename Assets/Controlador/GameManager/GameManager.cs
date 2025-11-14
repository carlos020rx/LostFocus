using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Coroutine randomSoundCoroutine;

    public AudioSource audioSource;
    public AudioClip[] soundEffects;
    public float minDelay = 5f;
    public float maxDelay = 10f;
    public PlayerMovement player;
    public GameObject granGota2, granGota3, playerPosition, lugarcerebro,lugarcerebro2,lugarcerebro3,lugarcerebro4, zoom;
    public PopupTester popupTester;
    public caidaAlimentos caidaAlimentos;

    public DialogueManager dialogueManager;

    public GameObject btnIz, btnDer, btnSalto;

    public AudioSource BGSound, miniJuego1Sound;

    private int contador = 1;

    public Animation transition;
    public GameObject panelTransition;
    public BlinkingPanel blinkingPanel;

    public Spawner minijuego2;

    public NPCPatron nPCPatron;





    // Start is called before the first frame update
    void Start()
    {
        randomSoundCoroutine = StartCoroutine(PlayRandomSound());

    }

    // Update is called once per frame
    void Update()
    {
        if (player.inicioMinijuego)
        {
            if (randomSoundCoroutine != null)
            {
                StopCoroutine(randomSoundCoroutine);
                randomSoundCoroutine = null;
            }

            audioSource.Stop();
            BGSound.Stop();

            if (!caidaAlimentos.murio && caidaAlimentos.miniJuegoFrutas == true)
            {
                miniJuego1Sound.mute = false;
            }

            //miniJuego1Sound.time = 5f;

            player.inicioMinijuego = false;
        }
        if (player.nutrientesFin)
        {
            granGota2.SetActive(true);
            popupTester.showMessage6 = true;
            contador = 2;
            player.nutrientesFin = false;
        }

        if (caidaAlimentos.murio == true)
        {
            miniJuego1Sound.mute = true;
            caidaAlimentos.vida = 3;
            caidaAlimentos.murio = false;
            blinkingPanel.StopBlink();
            //popupTester.blinkAutoTrigger = false;
            StartCoroutine(reinicioJuego());

        }

        if (minijuego2.murio2 == true)
        {
            //miniJuego1Sound.mute = true;
            minijuego2.vida = 3;
            minijuego2.murio2 = false;
            minijuego2.timerTemporizador=25f;
            //popupTester.blinkAutoTrigger = false;
            StartCoroutine(reinicioJuego2());

        }

        if (caidaAlimentos.finMinijuego1 == true)
        {
            popupTester.terminoMensaje1 = false;
            granGota3.SetActive(true);
            zoom.SetActive(false);
            popupTester.blinkAutoTrigger = false;
            //activarBotones();
            miniJuego1Sound.mute = true;
            BGSound.Play();
            audioSource.Play();

        }
        if (dialogueManager.acaboIntestino == true)
        {
            transition.Play();
            panelTransition.SetActive(true);
            StartCoroutine(cerebro(lugarcerebro));
            dialogueManager.acaboIntestino = false;
        }

        if (dialogueManager.minijuego2 == true)
        {
            popupTester.showMessage7 = true;
            StartCoroutine(inicioMinijuego2());
            dialogueManager.minijuego2 = false;

        }
        //Transición para cambiar de escenario entre cerebros
        if (dialogueManager.siguienteEscenario)
        {
            transition.Play();
            panelTransition.SetActive(true);
            StartCoroutine(cerebro(lugarcerebro2));
            dialogueManager.siguienteEscenario=false;
        }

                //Transición para cambiar de escenario entre cerebros
        if (dialogueManager.siguienteEscenario2)
        {
            transition.Play();
            panelTransition.SetActive(true);
            StartCoroutine(cerebro2(lugarcerebro3));
            dialogueManager.siguienteEscenario2=false;

        }

        if (dialogueManager.siguienteEscenario3)
        {
            transition.Play();
            panelTransition.SetActive(true);
            StartCoroutine(cerebro2(lugarcerebro4));
            dialogueManager.siguienteEscenario3=false;

        }
    }

    IEnumerator inicioMinijuego2()
    {
        yield return new WaitForSeconds(1.5f);
        minijuego2.minijuego2 = true;

    }


    IEnumerator cerebro(GameObject obj)
    {
        
        yield return new WaitForSeconds(1.5f);
        playerPosition.transform.position = obj.transform.position;
        transition.Play("transition_exit");
        yield return new WaitForSeconds(1.5f);
        panelTransition.SetActive(false);

    }

        IEnumerator cerebro2(GameObject obj)
    {
        
        yield return new WaitForSeconds(1.5f);
        playerPosition.transform.position = obj.transform.position;
        transition.Play("transition_exit");
        yield return new WaitForSeconds(1.5f);
        panelTransition.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        nPCPatron.caminarAhora=true;


    }

    IEnumerator reinicioJuego()
    {
        yield return new WaitForSeconds(4f);
        caidaAlimentos.vida = 3;
        caidaAlimentos.murio = false;
        transition.Play("transition_exit");
        miniJuego1Sound.mute = false;
        caidaAlimentos.miniJuegoFrutas = true;
        caidaAlimentos.enMinijuego1 = true;
        popupTester.terminoMensaje1 = true;
        caidaAlimentos.contador = 1;


    }
    IEnumerator reinicioJuego2()
    {
        yield return new WaitForSeconds(3f);
        minijuego2.vida = 3;
        minijuego2.murio2 = false;
        
        transition.Play("transition_exit");
        minijuego2.minijuego2 = true;
        yield return new WaitForSeconds(0.6f);
        panelTransition.SetActive(false);
        //miniJuego1Sound.mute = false;
        //caidaAlimentos.miniJuegoFrutas = true;
        //caidaAlimentos.enMinijuego1 = true;
        //opupTester.terminoMensaje1 = true;
        //caidaAlimentos.contador = 1;


    }
    IEnumerator PlayRandomSound()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            int index = Random.Range(0, soundEffects.Length);
            audioSource.PlayOneShot(soundEffects[index]);
        }
    }

    public void desactivarBotones()
    {
       
        btnDer.SetActive(false);
        btnSalto.SetActive(false);
        btnIz.SetActive(false);
    }

    public void activarBotones()
    {
        btnDer.SetActive(true);
        btnSalto.SetActive(true);
        btnIz.SetActive(true);
    }
}
