using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RotateScript : MonoBehaviour
{
    [SerializeField]
    private float staminaCost; // amount of stamina it cost to rotate
    Vector2 fingerUpPosition;
    Vector2 fingerDownPosition;
    [SerializeField]
    float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        checkRotation(); // checks if the player has made a movement(swipe) that rotates the player
    }


    void checkRotation()// checks if the player has made a movement(swipe) that rotates the player
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    fingerDownPosition = touch.position;
                    break;
                case TouchPhase.Ended:
                    // if the player has enough stamina to do the rotation
                    if (gameObject.GetComponent<playerManager>().currentStamina > staminaCost)
                    {
                        gameObject.GetComponent<playerManager>().currentStamina -= staminaCost;
                        Debug.Log(gameObject.GetComponent<playerManager>().currentStamina);
                        if(Math.Abs(fingerDownPosition.x - touch.position.x)> 10) // min distance to be considered a swipe
                        {
                            if (fingerDownPosition.x > touch.position.x) // if you have moved left(swiped left)
                            {
                            transform.parent.transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);

                            }
                            else if (fingerDownPosition.x < touch.position.x) //if you have moved right(swiped right)
                            {
                            transform.parent.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                            }
                        }
                    }
                        break;
                    

            }
        }
    }
}
    


