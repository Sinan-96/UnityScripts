using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootyShooty : MonoBehaviour
{
    [SerializeField]
    Transform fireOrigin; //origin of the shoot, deduct this vector from firepoint position vector to get the direction of the shot
    Touch touch;
    private float accumulatedTime; // how long the tap has been held
    [SerializeField]
    private GameObject bulletPrefab; // Bullet weapon fires
    [SerializeField]
    Transform firePoint;//muzzle of the gun, point where bullet spawns
    [SerializeField]
    PhotonView photonView;
    public float fireRate;
    public float minTime; //minimum amount of time to fire a bullet;
    private float damage; // how much damage a shoot does

    private void Awake()
    {
        damage = gameObject.GetComponent<playerManager>().damage;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Stationary:
                    charge();
                    break;

                case TouchPhase.Moved:
                    goto case TouchPhase.Stationary;

                case TouchPhase.Ended:
                    if (bulletPrefab != null) // if there exists a bullet prefab
                        shootPrefab();
                    else
                        shootRay();
                    break;


            }
        }
        
        //Remove
        if (Input.GetMouseButtonDown(0))
            shootRay();
    }
    // if bullet shoots a bullet type like object
    void shootPrefab()
    {
        Debug.Log("Boom!");
        if (accumulatedTime >= minTime)
        {
            bulletPrefab.transform.localScale *= accumulatedTime; // change size of prefab based on how long the player has held the touch
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            accumulatedTime = 0;
            bulletPrefab.transform.localScale = new Vector3(3, 3, 3); //revert prefab to original form
        }
    }

    //if bullet shoots a laserlike beam
    void shootRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, (firePoint.transform.position - fireOrigin.position));
        Debug.DrawRay(firePoint.transform.position, (firePoint.transform.position - fireOrigin.position) * 100, Color.red,0.05f);
        if(hit.collider != null)
        {
            /*if it hits other player
             * damage that players stamina*/
            if (hit.collider.tag == "Player")
                hit.collider.gameObject.GetComponent<playerManager>().currentStamina -= damage * accumulatedTime; 
        }
    
    }
    //While the shoot is charging
    void charge()
    {
        accumulatedTime += touch.deltaTime;
        Debug.Log(accumulatedTime);
        if(accumulatedTime> fireRate)
        {
            accumulatedTime = fireRate;
            if (bulletPrefab != null) // if there exists a bullet prefab
                shootPrefab();
            else
                shootRay();
        }
        //TODO
    }
}
