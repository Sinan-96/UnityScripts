using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class move2D : MonoBehaviour, IPunObservable
{
    Vector2 StartPosition; // StartPosition of the touch
    [SerializeField]
    GameObject bulletPrefab; // Bullet weapon fires
    [SerializeField]
    Transform firePoint;//muzzle of the gun, point where bullet spawns
    [SerializeField]
    PhotonView photonView;
    [SerializeField]
    Rigidbody2D rb;//rigidbody
    public float forceMultiplier; // Number to multiply with the force vector


    void Awake()
    {
        photonView = GetComponent<PhotonView>();

        photonView.ObservedComponents.Add(this);
        if (!photonView.IsMine)
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine) {
            // Touches
            foreach (Touch touch in Input.touches)
            {
                if(touch.phase == TouchPhase.Began)
                {
                    StartPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    Vector2 movementVector = touch.position - StartPosition; // Vector that moves the player
                    if (touch.deltaPosition.magnitude>0.5)//Swipe
                    {
                        swipe(movementVector);
                    }

                    else//Tap
                    {
                        Shoot();
                    }

                }



            }
        }
    }


            public void swipe(Vector2 dir) //What to do when a swipe is recognized
            
                {
                    rb.AddForce(dir * forceMultiplier);
                }

                //Method for firing weapon
            void Shoot()
                {
                    Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

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
 


    






