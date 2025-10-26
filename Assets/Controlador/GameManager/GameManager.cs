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
    public GameObject granGota2,granGota3;
    public PopupTester popupTester;
    public caidaAlimentos caidaAlimentos;

    public GameObject btnIz, btnDer, btnSalto;

    public AudioSource BGSound, miniJuego1Sound;

    private int contador = 1;
    
    public Animation transition;

    public BlinkingPanel blinkingPanel;



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
        
        if(caidaAlimentos.finMinijuego1 == true)
        {
            popupTester.terminoMensaje1 = false;
            granGota3.SetActive(true);
            popupTester.blinkAutoTrigger = false;
            //activarBotones();
            miniJuego1Sound.mute = true;
            BGSound.Play();
            audioSource.Play();

        }
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
