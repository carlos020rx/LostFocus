using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] soundEffects;
    public float minDelay = 5f;
    public float maxDelay = 10f;
    public PlayerMovement player;
    public GameObject granGota2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayRandomSound());

    }

    // Update is called once per frame
    void Update()
    {
        if (player.nutrientesFin)
        {
            granGota2.SetActive(true);

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
}
