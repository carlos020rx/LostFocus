using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public GameObject menuOpciones;

    public GameObject botonPausa;
    



    [Header("-----MIXER-----")]
    public AudioMixer myMixer;
    public Slider musicSlider;
    public Slider effectsSlider;

    public GameObject musicaIMG, noMusicaIMG, effectsIMG, noEffectsIMG;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Inicio")
        {
            Debug.Log("MenuController reseteado.");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        menuOpciones.SetActive(false);

        musicaIMG.SetActive(true);
        effectsIMG.SetActive(true);
        noMusicaIMG.SetActive(false);
        noEffectsIMG.SetActive(false);
    }

    private void Update()
    {
        sinEfectos();
        sinMusica();
    }

    public void abrirMenu()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
    }

    public void cerrarMenu()
    {
        menu.SetActive(false);
        menuOpciones.SetActive(false);
        Time.timeScale = 1;
        botonPausa.SetActive(true);
    }

    public void abrirMenuOpciones()
    {
        menu.SetActive(false);
        menuOpciones.SetActive(true);
        botonPausa.SetActive(false);
    }


    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }
    public void SetEffectsVolume()
    {
        float volume = effectsSlider.value;
        myMixer.SetFloat("effects", Mathf.Log10(volume) * 20);
    }

    private void sinMusica()
    {
        if (musicSlider.value <= 0.01)
        {
            musicaIMG.SetActive(false);
            noMusicaIMG.SetActive(true);
        }
        else
        {
            musicaIMG.SetActive(true);
            noMusicaIMG.SetActive(false);
        }
    }

    private void sinEfectos()
    {
        if (effectsSlider.value <= 0.01)
        {
            effectsIMG.SetActive(false);
            noEffectsIMG.SetActive(true);
        }
        else
        {
            effectsIMG.SetActive(true);
            noEffectsIMG.SetActive(false);
        }
    }


}
