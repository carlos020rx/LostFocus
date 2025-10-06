using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnciclopediaEntryUI : MonoBehaviour
{
    [SerializeField] private string entryID;
    [SerializeField] private GameObject lockedVisual;
    [SerializeField] private GameObject unlockedVisual;

    private void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        bool unlocked = EnciclopediaController.Instance.IsUnlocked(entryID);
        lockedVisual.SetActive(!unlocked);
        unlockedVisual.SetActive(unlocked);
    }

}
