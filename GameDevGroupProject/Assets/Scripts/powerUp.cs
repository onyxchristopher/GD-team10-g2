using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public bool isActive = false;

    public GameObject player;

    public Animator powerUpRandomMovement;


    public void Start()
    {
        powerUpRandomMovement = GetComponent<Animator>();

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
        var player = GameObject.FindWithTag("Player").GetComponent<playerBehavior>();

        if (player != null && other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Power up hit! {powerUpType}");
            
            if (player.AddPowerUp(this))
            {
                isActive = true;

                gameObject.transform.SetParent(player.transform);

                powerUpRandomMovement.Play("RotateAroundCenter");
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