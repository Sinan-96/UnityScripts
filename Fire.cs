using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Fire : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    PhotonView photonView;
    Vector2 prevPos; //pos of first touch, if same in next update then it is a tap

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        photonView.ObservedComponents.Add(this);
        if (!photonView.IsMine)
        {
            enabled = false;
        }

    }
    void Update()
    {
        if (photonView.IsMine) {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Shoot();
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Began )
            {
            }
        }
        
    }
    //Method for firing weapon
    void Shoot()
    {
        Instantiate(bulletPrefab,firePoint.position,firePoint.rotation);

    }

   
}
