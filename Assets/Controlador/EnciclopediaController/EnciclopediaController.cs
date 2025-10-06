using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnciclopediaController : MonoBehaviour
{
    public static EnciclopediaController Instance;

    private HashSet<string> unlockedItems = new HashSet<string>();

    public GameObject Enciclopedia;

    public GameObject scrollPersonajes;
    public GameObject scrollEscenarios;
    public GameObject scrollColleccionables;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Enciclopedia.SetActive(false);
        scrollPersonajes.SetActive(true);
        scrollEscenarios.SetActive(false);
        scrollColleccionables.SetActive(false);
    }

    private void Load()
    {
        string saved = PlayerPrefs.GetString("encyclopedia_data", "");
        if (!string.IsNullOrEmpty(saved))
        {
            unlockedItems = new HashSet<string>(saved.Split(','));
        }
    }

    public bool IsUnlocked(string id)
    {
        return unlockedItems.Contains(id);
    }

    public void Unlock(string id)
    {
        if (unlockedItems.Add(id))
        {
            Debug.Log($"Desbloqueado: {id}");
        }
    }

    public void abrirEnciclopedia()
    {
        Enciclopedia.SetActive(true);
    }
    public void cerrarEnciclopedia()
    {
        Enciclopedia.SetActive(false);
    }

    public void personajes()
    {
        scrollPersonajes.SetActive(true);
        scrollEscenarios.SetActive(false);
        scrollColleccionables.SetActive(false);
    }

    public void escenarios()
    {
        scrollPersonajes.SetActive(false);
        scrollEscenarios.SetActive(true);
        scrollColleccionables.SetActive(false);
    }

    public void coleccionables()
    {
        scrollPersonajes.SetActive(false);
        scrollEscenarios.SetActive(false);
        scrollColleccionables.SetActive(true);
    }

    public string GetUnlockedItemsAsString()
    {
        return string.Join(",", unlockedItems);
    }

    public void LoadUnlockedItemsFromString(string data)
    {
        unlockedItems = new HashSet<string>(data.Split(',', System.StringSplitOptions.RemoveEmptyEntries));
    }


}
