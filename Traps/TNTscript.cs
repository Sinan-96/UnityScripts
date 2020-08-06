using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTscript : MonoBehaviour
{
    [SerializeField]
    float hp;
    [SerializeField]
    float radius;
    [SerializeField]
    float damage;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Explode();
            Destroy(gameObject);
        }   
    }

    public void setHealth(float damage)
    {
        hp -= damage;
    }

    void Explode()
    {

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, radius); //gets all gameobjects with a collider within a radius
        Debug.Log("Start" + hitColliders.Length );
        int i = 0;
        //Goes through the colliders, if player or explosive, it takes damage
        while (i < hitColliders.Length)
        {
            Debug.Log("here");
            if(hitColliders[i].gameObject.tag == "Player")
            {
                Debug.Log("Gothere!");
                hitColliders[i].gameObject.GetComponent<playerManager>().setHealth(damage);
            }
            else if(hitColliders[i].gameObject.tag == "Explosive")
            {
                hitColliders[i].gameObject.GetComponent<TNTscript>().setHealth(damage);
            }
            i++;

        }
    }
}
