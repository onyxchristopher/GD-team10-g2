using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    Red,
    Blue,
    Green
}

public class powerUp : MonoBehaviour
{
    public PowerUpType powerUpType;

    public int playerJumpMultiplier = 2;
    public int playerSpeedMultiplier = 2;

    public GameObject player;

    
    void OnTriggerEnter2D(Collider2D other)
    {
        var powerupHandler = other.gameObject.GetComponent<playerPowerupsHandler>();
        
        if (powerupHandler != null)
        {
            if (!powerupHandler.powerUpsActive.Contains(this))
            {
                player = powerupHandler.gameObject;
                
                powerupHandler.powerUpsActive.Add(this);
                
                Debug.Log($"Power up added {powerUpType}");
            }
        }
    }

    // This method is called when another collider stays within the trigger
    void OnTriggerStay2D(Collider2D other)
    {
    }

    // This method is called when another collider exits the trigger
    void OnTriggerExit2D(Collider2D other)
    {
    }
}