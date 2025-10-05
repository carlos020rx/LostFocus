using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnciclopediaInteraction : MonoBehaviour
{
    [SerializeField] private string entryID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Si entra");
            EnciclopediaController.Instance.Unlock(entryID);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        EnciclopediaEntryUI[] allEntries = FindObjectsOfType<EnciclopediaEntryUI>(true);
        foreach (var entry in allEntries)
        {
            entry.UpdateVisual();
        }
    }
}
