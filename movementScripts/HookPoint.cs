

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
public class HookPoint : MonoBehaviour
{
    GameObject player;
    private float currentConquerPercentage; //How far percentwise the dot is to being conquered
    public bool Conquered; // if the dot is conquered or not
    [SerializeField]
    String powerUpType;// what kind of powerup the dot gives the player after it is conquered

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player" && player == null) // If a player hits the grappling point
        {
            stick(other.collider.gameObject);
        }
    }

    void stick(GameObject pl) // makes the player stick to the grapplingPoint TODO
    {
        player = pl;
        Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
        player.transform.SetParent(gameObject.transform.GetChild(0).transform);
        Debug.Log("Stuck");
        rb.velocity = Vector2.zero; // sets player speed to zero
        rb.gravityScale = 0; // sets players gravity to zero so it dosent fall when connect
        player.GetComponent<HookScript>().onTheMove = false; // sets the players ontheMove variable to false, to stop further movement
        rb.freezeRotation = false; //Enables rotation for the rotation controls
        player.gameObject.GetComponent<RotateScript>().enabled = true; // enables rotation controls for player





    }

    void unStick()// unstick player from grapplePoint TODO
    {
        Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; //diables rotation
        player.gameObject.GetComponent<RotateScript>().enabled = false; // disables rotation controls
        player.transform.SetParent(player.transform);
        player = null; // resets player variable
        
        if (!Conquered)
            currentConquerPercentage = 0;

    }

    void Update()
    {
        if (player != null)
        {
            if (player.GetComponent<HookScript>().onTheMove) // if the player is moving to another dot
            {
                unStick();
            }
            // If the point is conquered
            if (currentConquerPercentage >= 100 && !Conquered)
            {
                Debug.Log("I am conquered");
                conquered();
            }
            /* if there is a player on the dot, 
             * increment the currentConquerPercentage
             * with the players conquerrate*/
            else if (!Conquered)
            {
                currentConquerPercentage += player.GetComponent<playerManager>().conquerRate;
            }
        }
    }

    private void conquered() // what happens when the dot is conquered
    {
        Conquered = true;
        player.GetComponent<playerManager>().powerUp(powerUpType); // Gives the player the dot powerup
        int pn = player.GetComponent<playerManager>().playerNumber;
        grow(pn);
    }
    /* growth that happens after
     * the dot is conquered, different
     * dependig on what player conquers it*/
    private void grow(int player)
    {
        Debug.Log("growing");
        //TODO
    }

}
