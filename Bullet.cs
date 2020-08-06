using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Animator animator;
    public float speed = 20f;
    public Rigidbody2D rb;
    public float damage;
    
    // Start is called before the first frame update
    void Start()
    {
        //animator.SetBool("OnHit",false);
        rb.velocity = transform.right * speed;
        
    }
     //When the bullet hits something
     private void OnTriggerEnter2D(Collider2D other) {
        speed = 0f;
        rb.velocity = transform.right * speed;
        //animator.SetBool("OnHit",true);
        //StartCoroutine("wait");
        Destroy(gameObject);
        //If the bullet hits another player, the player loses health
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<playerManager>().setHealth(damage);
        }
        else if(other.gameObject.tag == "Explosive")
        {
            other.gameObject.GetComponent<TNTscript>().setHealth(damage);
        }
        
        
    
    }
    //Waits for the animation to finish
    IEnumerator wait()
    {
        yield return new WaitForSeconds(5);
    }

    
}
