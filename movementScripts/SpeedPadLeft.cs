using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPadLeft : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    private float force; // Amount of force in x direction the speedpad emits
    List<Rigidbody2D> speeders; //People who have used the pad recently, used for knowing which rigidbodies to slow down
    [SerializeField]
    private float slowDownFactor; // how fast the jump is slowed down

    void Awake()
    {
        speeders = new List<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.collider.tag);
        rb = other.collider.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(-force, 0));
        Debug.Log(rb.velocity.x);
        speeders.Add(rb);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Rigidbody2D toBeRemoved = null;
        foreach (Rigidbody2D r in speeders) // slows the speed(in x direction) of the rigidbodies, if speed is zero removes rigidbody from the list
        {
            if (r.velocity.x > 0)
            {
                Debug.Log("got here");
                toBeRemoved = r;
                break;
            }
            Debug.Log("Before " + rb.velocity.x);
            r.AddForce(Vector2.right * force / slowDownFactor);
            Debug.Log("After " + rb.velocity.x);
            
        }
        speeders.Remove(toBeRemoved);
    }
}
