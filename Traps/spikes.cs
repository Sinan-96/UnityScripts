using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikes : MonoBehaviour
{
    [SerializeField]
    float baseDamage; // the base damage the spikes incur, regardless of speed
    [SerializeField]
    float damageMult; //damage multiplier, timed with speed
    float speed; //speed at which the item collides with the spikes



    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
            speed = other.collider.GetComponent<Rigidbody2D>().velocity.magnitude;
            other.collider.GetComponent<playerManager>().setHealth(baseDamage + speed*damageMult);
        }

    }
}