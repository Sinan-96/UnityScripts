using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpScript : MonoBehaviour
{
    [SerializeField]
    float speedBoost; // how much of a speed boost the player gets
    float seconds;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<move2D>().forceMultiplier += speedBoost;
            Destroy(gameObject);
        }

        StartCoroutine(SpeedUpTimer());
        other.gameObject.GetComponent<move2D>().forceMultiplier -= speedBoost;
    }

    IEnumerator SpeedUpTimer()
    {
        yield return new WaitForSeconds(seconds);

    }

}
