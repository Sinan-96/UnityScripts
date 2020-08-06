using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class HookScript : MonoBehaviour, IPunObservable
{
    String powerUpType; // The kind of powerup the dot gives the player who conquers it
    [SerializeField]
    private float staminaCost; // the amount of stamina it costs to grapple to a point
    [SerializeField]
    GameObject bulletPrefab; // Bullet weapon fires
    [SerializeField]
    PhotonView photonView;
    [SerializeField]
    Rigidbody2D rb; //Rigidbody of player
    [SerializeField]
    float grapplingSpeed; // speed wich players move when using grappling
    Vector2 currentTarget; // the last hookPoint the player touched, and has not reached yet
    public bool onTheMove; // bool that decides if the player is moving towards the current target or not
    bool doubleClicked; // True if you tapped a hookpoint more than once;



    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        
        photonView.ObservedComponents.Add(this);
        if (!photonView.IsMine)
        {
            enabled = false;
        }
        onTheMove = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)// So a touch is only registered once
                {
                    if (containsHookPoint(touch))
                    {
                        Vector2 Start = rb.position;
                        Debug.Log("gor");
                        RaycastHit2D hit = Physics2D.Linecast(Start, currentTarget, 8); // Hits all colliders exept those in layer 2(hookpoint and player)
                        onTheMove = (hit.collider == null); // if there are no colliders in the line, onTheMove is true
                        // checks if the player does not have enough stamina for the grapple
                        if (!(gameObject.GetComponent<playerManager>().currentStamina > staminaCost))
                        {
                            onTheMove = false;
                        }
                        //If the player hasenough stamina for the grapple, do the grapple
                        //and subtract the stamina cost from the players stamina
                        else
                        {
                            gameObject.GetComponent<playerManager>().currentStamina -= staminaCost;
                            Debug.Log(gameObject.GetComponent<playerManager>().currentStamina);
                        }
                    }
                }
            }
           if(onTheMove)//move towards current target as long as onTheMove is true
            {
                Debug.Log("On the move");
                rb.transform.position = Vector2.MoveTowards(transform.position, currentTarget, grapplingSpeed);
                Debug.Log("moving");
                if (!onTheMove ) // if the player stops moving towards target, check if the player double clicked the target
                {
                    if (doubleClicked)
                    {
                        doubleClick(); //if he doubleclicked the target
                    }
                    else
                    {
                        stick(); // if he didnt doubleclick, then you stick to the grapplepoint
                    }
                    
                }
                   
            }
            
        }
    }
    //Checks if the touch is inside a hook point
    bool containsHookPoint(Touch touch)
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);//creates a raycast on the position of the touch
        if (hit.collider != null) // if the ray hits something or if the touch hits something
        {
            Debug.Log("hit is " + (hit.collider.tag == "HookPoint"));
            if(hit.collider.tag == "HookPoint")
            {
               Vector2 target = hit.collider.gameObject.transform.position;
               doubleClicked = (target == currentTarget);// if you already move towards that position, or that grapplepoint
               if (currentTarget == target)
                   return false;
               currentTarget = target;
               return true;
            }
        }
        return false;


    }



    void doubleClick() // What happens at the end of the grappling if you double clicked TODO
    {
        Debug.Log("DoubleClicked!");
    }

    void stick()//If you just clicked the target once, you stick to the target TODO
    {
        Debug.Log("Clicked once!");

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)//Fixes lag
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.velocity);
        }
        else
        {
            rb.position = (Vector2)stream.ReceiveNext();
            rb.velocity = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            rb.position += rb.velocity * lag;
        }
    }
}
