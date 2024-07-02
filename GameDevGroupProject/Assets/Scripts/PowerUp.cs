using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PowerUpType
{
    Red,
    Blue,
    Green
}

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;

    public int playerJumpMultiplier = 2;
    public int playerSpeedMultiplier = 2;

    public bool isActive = false;

    public GameObject player;

    public Animator powerUpRandomMovement;


    public void Start()
    {
        powerUpRandomMovement = GetComponent<Animator>();

        // powerUpRandomMovement.Play("MoveUpDown",-1);

        powerUpRandomMovement.speed = Random.Range(0.75f, 1.25f);
    }

    private void FixedUpdate()
    {
        if (player)
        {
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var powerupHandler = other.gameObject.GetComponent<PlayerPowerupsHandler>();

        if (powerupHandler != null)
        {
            if (!powerupHandler.powerUpsActive.Contains(this))
            {
                player = powerupHandler.gameObject;

                powerupHandler.AddPowerUp(this);

                isActive = true;

                gameObject.transform.SetParent(player.transform);

                powerUpRandomMovement.Play("RotateAroundCenter");

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