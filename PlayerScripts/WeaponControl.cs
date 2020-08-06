using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponControl : MonoBehaviour
{
  private Transform aimTransform;
  public float speed = 5f;
  PhotonView photonView;

    private void Awake()
  {
    aimTransform = transform.Find("Weapon");
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
        if (photonView.IsMine)
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        }
    }

}
