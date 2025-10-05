using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveManager : MonoBehaviour
{
    public GameObject player; // Asigna el objeto del jugador en el Inspector
    public EnciclopediaController encyclopediaManager; // Asigna el manager si lo tienes

    private void Start()
    {
        LoadGame(); // Carga datos al iniciar
    }

    public void SaveAndExit()
    {
        SaveGame();
        Debug.Log("Juego guardado. Saliendo...");

        // Aquí puedes cambiar de escena, por ejemplo al menú principal:
        //SceneManager.LoadScene("Menu");
    }

    public void SaveGame()
    {
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            PlayerPrefs.SetFloat("player_x", pos.x);
            PlayerPrefs.SetFloat("player_y", pos.y);
            PlayerPrefs.SetFloat("player_z", pos.z);
        }

        // Ejemplo: guardar un bool (si el jugador tiene una llave, por ejemplo)
        //bool hasKey = PlayerPrefs.GetInt("hasKey", 0) == 1; // o si lo tienes en otro script, lo lees de allí
        //PlayerPrefs.SetInt("hasKey", hasKey ? 1 : 0);

        // Guardar progreso de enciclopedia (si tienes ese manager)
        if (encyclopediaManager != null)
        {
            string data = encyclopediaManager.GetUnlockedItemsAsString();
            PlayerPrefs.SetString("encyclopedia_data", data);
        }

        PlayerPrefs.Save();
        Debug.Log("Datos del juego guardados.");
    }

    public void LoadGame()
    {
        if (player != null && PlayerPrefs.HasKey("player_x"))
        {
            float x = PlayerPrefs.GetFloat("player_x");
            float y = PlayerPrefs.GetFloat("player_y");
            float z = PlayerPrefs.GetFloat("player_z");
            player.transform.position = new Vector3(x, y, z);
        }

        // Cargar progreso de enciclopedia
        if (encyclopediaManager != null)
        {
            string data = PlayerPrefs.GetString("encyclopedia_data", "");
            encyclopediaManager.LoadUnlockedItemsFromString(data);
        }

        Debug.Log("Datos del juego cargados.");
    }
}
