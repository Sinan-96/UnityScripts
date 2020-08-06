using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class dashControls : MonoBehaviour, IPunObservable
{
    [SerializeField]
    PhotonView photonView;
    [SerializeField]
    Rigidbody2D rb; //Rigidbody of player
    [SerializeField]
    float dashSpeed;//Speed of dash :)
    [SerializeField]
    float startingDashTime; // the time a dash last
    [SerializeField]
    float dashFrequency; // how often you can dash
    float dashFrequencyTimer; // Timer too see if you can do a dash now
    private float dashTime; // time of the current dash
    private DraggedDirection currentDirection; // direction of current dash, None if not dashing
    Vector2 fingerUpPosition;
    Vector2 fingerDownPosition;


    private enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left,
        None
    }
    void Awake()
    {
        photonView = GetComponent<PhotonView>();

        photonView.ObservedComponents.Add(this);
        if (!photonView.IsMine)
        {
            enabled = false;
        }
        dashTime = startingDashTime;
        dashFrequencyTimer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (canDash())
            {
                dashing();
            }
            checkTouches();
        }
    }


    bool canDash() // Function that decides if you can dash, based on the timer
    {
        if (dashFrequencyTimer > 0)
        {
            dashFrequencyTimer -= Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }
    void dashing() // function that control dashing
    {
        if (currentDirection != DraggedDirection.None)
        {
            if (dashTime <= 0) // If dash is over set direction to none
            {
                currentDirection = DraggedDirection.None;
                dashTime = startingDashTime;
                rb.velocity = Vector2.zero;
            }
            else // if not over , deduct time passed from dashtime and find the direction of the dash
            {
                dashTime -= Time.deltaTime;
                if (currentDirection == DraggedDirection.Left)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                }
                else if (currentDirection == DraggedDirection.Right)
                {
                    rb.velocity = Vector2.right * dashSpeed;
                }
                else if (currentDirection == DraggedDirection.Down)
                {
                    rb.velocity = Vector2.down * dashSpeed;
                }
                else if (currentDirection == DraggedDirection.Up)
                {
                    rb.velocity = Vector2.up * dashSpeed;
                }
            }


        }
    }

        void checkTouches()
    {
        foreach (Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = touch.position;
            }
            if(touch.phase == TouchPhase.Ended)
            {
                fingerUpPosition = touch.position;
                Vector2 dragVector = fingerUpPosition - fingerDownPosition;
                if(dragVector.magnitude > 10) // checks that it is not a touch
                {
                    dragVector = dragVector.normalized;
                    currentDirection = GetDraggedDirection(dragVector);
                }
            }
        }
    }

    DraggedDirection GetDraggedDirection(Vector2 dragVector) //Get directionof the swipe
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        Debug.Log(draggedDir);
        return draggedDir;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
