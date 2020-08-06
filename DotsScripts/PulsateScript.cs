using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsateScript : MonoBehaviour
{
    [SerializeField]
    float pulsateRate; // Speed which the dot pulsates
    [SerializeField]
    float pulsateCap; // The biggest it can pulsate to
    [SerializeField]
    float pulsateMin; // the samllest it can pulsate to
    [SerializeField]
    bool scaleDirection; // If it scales up or down currently, true up, false down
    Vector3 scaleChange;
    // Update is called once per frame
    void Update()
    {
        if(scaleDirection)
        {
            scaleChange = new Vector3(pulsateRate, pulsateRate, pulsateRate);
        }
        else
        {
            scaleChange = new Vector3(-pulsateRate, -pulsateRate, -pulsateRate);
        }

        
        gameObject.transform.localScale += scaleChange;
        if (gameObject.transform.localScale.x >= pulsateCap)
        {
            scaleDirection = false;
        }
        else if (gameObject.transform.localScale.x <= pulsateMin)
        {
            scaleDirection = true;
        }
        
    }
}
