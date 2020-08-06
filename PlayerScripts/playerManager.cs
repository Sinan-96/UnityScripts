using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    [SerializeField]
    private float playerHealth;
    [SerializeField]
    private float maxStamina; // The max amount of stamina the player can have
    public float currentStamina;//if the player has zero stamina, it cant move 
    [SerializeField]
    private float staminaRecovery; // the amount of stamina recovered
    public float conquerRate; // how fast the player can conquer a point
    public int playerNumber; // The number of the player decides how his growths are
    // Start is called before the first frame update
    public float fireRate { get; private set; }
    public float damage { get; private set; }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If the player has zero or less health it is destroyed
        if (playerHealth <= 0)
            Destroy(gameObject);
        //Stamina recovers if it is below the max stamina set
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRecovery;
            //Current stamina cant be higher than max stamina
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
        }
    }

    public void setHealth(float damage)
    {
        playerHealth -= damage;
        Debug.Log(playerHealth);
    }

    public void setStamina(float damage)
    {
        currentStamina -= damage;
        Debug.Log(currentStamina);
    }

    public void powerUp(string powerup) // Power up the player, based on the argument
    {
        //Cheks what powerup the player got and applies the powerup
        switch (powerup)
        {
            case "stamina":
                maxStamina += maxStamina * 0.25f;
                staminaRecovery += staminaRecovery * 0.25f;
                break;
            case "conquer":
                conquerRate += conquerRate * 0.3f;
                break;
            case "fireRate":
                fireRate += 0.3f * fireRate;
                break;
            case "damage":
                damage += 0.3f * damage;
                break;
        }
    }
    
}
