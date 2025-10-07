using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Coroutine randomSoundCoroutine;

    public AudioSource audioSource;
    public AudioClip[] soundEffects;
    public float minDelay = 5f;
    public float maxDelay = 10f;
    public PlayerMovement player;
    public GameObject granGota2;
    private int contador = 1;

    public GameObject btnIz, btnDer, btnSalto;

    public AudioSource BGSound, miniJuego1Sound;

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
            BGSound.Pause();

            miniJuego1Sound.mute = false;
            //miniJuego1Sound.time = 5f;

            player.inicioMinijuego = false;
        }
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
