using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alimentos : MonoBehaviour
{
    // Start is called before the first frame update
    //public Animator anim;
    public Rigidbody2D RB;
    //public GameObject Player;    
    //private Animator animationPlayer;


public void Start()
    {
        //animationPlayer = Player.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //anim.SetTrigger("point");
            //animationPlayer.SetTrigger("dano");
            RB.isKinematic = true;
            RB.velocity = Vector2.zero;
            Debug.Log("collision");
            Destroy(gameObject);

        }
    }

    public void destruir(){
        Destroy(gameObject);
    }
}
