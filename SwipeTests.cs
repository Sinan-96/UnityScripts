using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;

public class SwipeTests : MonoBehaviour
{
    Vector2 firstPosition;
    Vector2 lastPosition;
    [SerializeField]
    Rigidbody2D player;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPosition = Input.mousePosition;   
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lastPosition = Input.mousePosition;
            swipe((lastPosition - firstPosition));

        }

    }

    public void swipe(Vector2 dir)
    {
        player.AddForce(dir);
    }

}
