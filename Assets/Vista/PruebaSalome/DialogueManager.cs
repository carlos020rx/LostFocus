using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> playerLines;
    private Queue<string> npcLines;

    private bool isPlayerTurn = true;

    private GameObject playerBubble;
    private TextMeshProUGUI playerText;

    private GameObject npcBubble;
    private TextMeshProUGUI npcText;

    // Indica si hay un diálogo activo
    public bool IsDialogueActive { get; private set; } = false;

    private void Awake()
    {
        playerLines = new Queue<string>();
        npcLines = new Queue<string>();
    }

    public void StartDialogue(
        string[] playerDialogue, string[] npcDialogue,
        GameObject playerBubbleObj, TextMeshProUGUI playerTextObj,
        GameObject npcBubbleObj, TextMeshProUGUI npcTextObj,
        bool startsWithPlayer)
    {
        // Si ya hay un diálogo en curso no lo reiniciamos
        if (IsDialogueActive) return;

        playerLines.Clear();
        npcLines.Clear();

        foreach (string line in playerDialogue)
            playerLines.Enqueue(line);

        foreach (string line in npcDialogue)
            npcLines.Enqueue(line);

        // Guardamos referencias
        playerBubble = playerBubbleObj;
        playerText = playerTextObj;
        npcBubble = npcBubbleObj;
        npcText = npcTextObj;

        isPlayerTurn = startsWithPlayer;
        IsDialogueActive = true;

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (!IsDialogueActive) return;

        // Apagamos burbujas antes de mostrar la siguiente
        if (playerBubble != null) playerBubble.SetActive(false);
        if (npcBubble != null) npcBubble.SetActive(false);

        // Caso: si el turno actual no tiene líneas, saltamos al otro si existen líneas
        if (isPlayerTurn)
        {
            if (playerLines.Count > 0)
            {
                string line = playerLines.Dequeue();
                if (playerBubble != null) playerBubble.SetActive(true);
                if (playerText != null) playerText.text = line;
                isPlayerTurn = !isPlayerTurn;
                return;
            }
            else if (npcLines.Count > 0)
            {
                isPlayerTurn = false; // saltamos al NPC
            }
            else
            {
                EndDialogue();
                return;
            }
        }

        // Aquí manejamos turno NPC (o llegamos por salto)
        if (!isPlayerTurn)
        {
            if (npcLines.Count > 0)
            {
                string line = npcLines.Dequeue();
                if (npcBubble != null) npcBubble.SetActive(true);
                if (npcText != null) npcText.text = line;
                isPlayerTurn = !isPlayerTurn;
                return;
            }
            else if (playerLines.Count > 0)
            {
                isPlayerTurn = true;
                DisplayNextLine(); // recursion segura: ahora mostrará la línea del jugador
                return;
            }
            else
            {
                EndDialogue();
                return;
            }
        }
    }

    public void EndDialogue()
    {
        if (playerBubble != null) playerBubble.SetActive(false);
        if (npcBubble != null) npcBubble.SetActive(false);

        IsDialogueActive = false;
        Debug.Log("Diálogo terminado");
    }
}
