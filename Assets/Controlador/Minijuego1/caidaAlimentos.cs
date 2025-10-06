using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caidaAlimentos : MonoBehaviour
{
    [SerializeField] GameObject[] fruits;
    [SerializeField] float spam = 0.5f;
    [SerializeField] float minTras;
    [SerializeField] float maxTras;
    public PopupTester popupTester;
    private int contador=1;


    // Start is called before the first frame update
    void Start()
    {
        
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
        if (popupTester.terminoMensaje1 == true && contador==1)
        {

            StartCoroutine(fruitsSpawn());
            contador = 2;
        }
            
    }
}
