using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
    }

    public void abrirMenu()
    {
        menu.SetActive(true);
    }

    public void cerrarMenu()
    {
        menu.SetActive(false);
    }


}
