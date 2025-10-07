using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    
    private string currentNPCID;
    private Queue<string> playerLines;
    private Queue<string> npcLines;

    public PopupTester popupTester;


    private bool isPlayerTurn = true;

    private GameObject playerBubble;
    private TextMeshProUGUI playerText;

    private GameObject npcBubble;
    private TextMeshProUGUI npcText;

    public GameObject btnMov1, btnMov2, btnSalto, Nutrientes,Nutrientes2,GranGota1;

    // Indica si hay un diálogo activo
    public bool isDialogueActive { get; private set; } = false;
    private bool isLineActive = false;

    private void Awake()
    {
        playerLines = new Queue<string>();
        npcLines = new Queue<string>();
    }

    public void StartDialogue(
        string npcID,
        string[] playerDialogue, string[] npcDialogue,
        GameObject playerBubbleObj, TextMeshProUGUI playerTextObj,
        GameObject npcBubbleObj, TextMeshProUGUI npcTextObj,
        bool startsWithPlayer)
    {
        currentNPCID = npcID;
        btnMov1.SetActive(false);
        btnMov2.SetActive(false);
        btnSalto.SetActive(false);

        StartCoroutine(EmpezarDialogo(playerDialogue, npcDialogue,
        playerBubbleObj, playerTextObj, npcBubbleObj, npcTextObj, startsWithPlayer));

    }

    public void DisplayNextLine()
    {
        if (!isDialogueActive || isLineActive) return;
        StartCoroutine(SigDialogo());

    }

    public void EndDialogue()
    {
        if (playerBubble != null) playerBubble.SetActive(false);
        if (npcBubble != null) npcBubble.SetActive(false);

        isDialogueActive = false;
        btnMov1.SetActive(true);
        btnMov2.SetActive(true);
        btnSalto.SetActive(true);
        Debug.Log("Diálogo terminado");
        if (currentNPCID == "GranGota1")
        {
            Nutrientes.SetActive(true);
            Nutrientes2.SetActive(true);
            StartCoroutine(desaparecerGranGota());
        }
        if (currentNPCID == "GranGota2")
        {
            Nutrientes.SetActive(false);
            StartCoroutine(Minijuego1Mensaje());

        }

        if (currentNPCID == "Inicio")
        {
            TriggerInicial.SetActive(false);

        }

        if (currentNPCID == "Medio")
        {
            TriggerMedio.SetActive(false);

        }
    }

    /*Corutinas*/
    IEnumerator Minijuego1Mensaje() {
        yield return new WaitForSeconds(1.2f);

        popupTester.playSequence = true;
    }
    IEnumerator EmpezarDialogo(string[] playerDialogue, string[] npcDialogue,
    GameObject playerBubbleObj, TextMeshProUGUI playerTextObj,
    GameObject npcBubbleObj, TextMeshProUGUI npcTextObj,
    bool startsWithPlayer)
    {
        yield return new WaitForSeconds(1f); // Espera antes de iniciar


        playerLines.Clear();
        npcLines.Clear();

        foreach (string line in playerDialogue)
            playerLines.Enqueue(line);

        foreach (string line in npcDialogue)
            npcLines.Enqueue(line);

        playerBubble = playerBubbleObj;
        playerText = playerTextObj;
        npcBubble = npcBubbleObj;
        npcText = npcTextObj;

        isPlayerTurn = startsWithPlayer;
        isDialogueActive = true;

        DisplayNextLine(); // muestra la primera línea
    }

    IEnumerator desaparecerGranGota()
    {
        yield return new WaitForSeconds(4f);
        GranGota1.SetActive(false);


    }

    IEnumerator SigDialogo()
    {
        isLineActive = true; // 🔹 activa bandera

        if (!isDialogueActive)
        {
            isLineActive = false;
            yield break;
        }

        // Apagamos burbujas antes de mostrar la siguiente
        if (playerBubble != null) playerBubble.SetActive(false);
        if (npcBubble != null) npcBubble.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        // Caso: si el turno actual no tiene líneas, saltamos al otro si existen líneas
        if (isPlayerTurn)
        {
            if (playerLines.Count > 0)
            {
                string line = playerLines.Dequeue();
                playerBubble?.SetActive(true);
                if (playerText != null) playerText.text = line;
                // después de mostrar jugador, el siguiente será NPC
                isPlayerTurn = false;
            }
            else if (npcLines.Count > 0)
            {
                // no hay línea del jugador -> mostramos NPC en este mismo toque
                string line = npcLines.Dequeue();
                npcBubble?.SetActive(true);
                if (npcText != null) npcText.text = line;
                // después de mostrar NPC, el siguiente será jugador
                isPlayerTurn = true;
            }
            else
            {
                EndDialogue();
                isLineActive = false;
                yield break;
            }
        }
        else // turno NPC
        {
            if (npcLines.Count > 0)
            {
                string line = npcLines.Dequeue();
                npcBubble?.SetActive(true);
                if (npcText != null) npcText.text = line;
                isPlayerTurn = true;
            }
            else if (playerLines.Count > 0)
            {
                string line = playerLines.Dequeue();
                playerBubble?.SetActive(true);
                if (playerText != null) playerText.text = line;
                isPlayerTurn = false;
            }
            else
            {
                EndDialogue();
                isLineActive = false;
                yield break;
            }
        }

        isLineActive = false;
    }
}
