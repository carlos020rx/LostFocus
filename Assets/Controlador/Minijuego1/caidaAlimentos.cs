using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caidaAlimentos : MonoBehaviour
{
    [SerializeField] GameObject[] fruits;
    [SerializeField] float spam = 0.5f;
    [SerializeField] float minTras;
    [SerializeField] float maxTras;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fruitsSpawn());
    }
    IEnumerator fruitsSpawn()
    {
        while(true)
        {
            var wanted = Random.Range(minTras, maxTras);
            var position = new Vector3 (wanted, transform.position.y);
            GameObject gameObject = Instantiate(fruits[Random.Range(0,fruits.Length)],position,Quaternion.identity);
            yield return new WaitForSeconds(spam);
            Destroy(gameObject,2f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
