using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class JumpPad : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    private float force; // Amount of force in y direction the jumppad emits
    List<Rigidbody2D> jumpers; //People who have used the pad recently, used for knowing which rigidbodies to slow down
    [SerializeField]
    private float slowDownFactor; // how fast the jump is slowed down

    void Awake()
    {
        jumpers = new List<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
       Debug.Log(other.collider.tag);
       rb = other.collider.GetComponent<Rigidbody2D>();
       rb.velocity = new Vector2(rb.velocity.x, force);
       jumpers.Add(rb);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody2D toBeRemoved = null;
        foreach (Rigidbody2D r in jumpers) // slows the speed(in y direction) of the rigidbodies, if speed is zero removes rigidbody from the list
        {
            if (r.velocity.y > 0)
            {
                r.velocity = new Vector2(r.velocity.x, r.velocity.y - force / slowDownFactor);
            }
            else
            {
                toBeRemoved = r;
            }
        }
        jumpers.Remove(toBeRemoved);

    }
}
