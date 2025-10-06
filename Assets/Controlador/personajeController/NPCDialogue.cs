using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public string npcID;
    //Diálogo del Jugador
    public string[] playerDialogue;

    //Diálogo del NPC
    public string[] npcDialogue;

    //Configuración
    public bool startsWithPlayer = false;

    //Referencias de Burbujas
    public GameObject playerBubble;          // Burbuja del jugador
    public TextMeshProUGUI playerText;

    public GameObject npcBubble;             // Burbuja del NPC
    public TextMeshProUGUI npcText;

    private bool playerInRange = false;
    private bool dialogueStarted = false; // para que no se reinicie

    private void Update()
    {
        // Iniciar el diálogo SOLO al presionar tecla estando en rango
        if (playerInRange && !dialogueStarted)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(npcID,
                playerDialogue, npcDialogue,
                playerBubble, playerText,
                npcBubble, npcText,
                startsWithPlayer
            );

            dialogueStarted = true;
        }

        // Avanzar con Q
        if (dialogueStarted && (Input.GetKeyDown(KeyCode.Q) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))// Input.GetKeyDown(KeyCode.Q))
        {
            //StartCoroutine(MostrarSig());
            FindObjectOfType<DialogueManager>().DisplayNextLine();
            Debug.Log("Q");

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Jugador entró en rango del NPC");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            dialogueStarted = false; // resetear cuando salga
        }
    }

    IEnumerator MostrarSig()
    {
        Debug.Log("Inicio de la coroutine");
        yield return new WaitForSeconds(1f);
        //FindObjectOfType<DialogueManager>().DisplayNextLine();
        Debug.Log("Han pasado 2 segundos");
    }
}
