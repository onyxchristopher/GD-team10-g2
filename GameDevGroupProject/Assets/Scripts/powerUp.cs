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

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = GameObject.FindWithTag("Player").GetComponent<playerBehavior>();

        if (player != null && other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Power up hit! {powerUpType}");
            
            if (player.AddPowerUp(this))
            {
                isActive = true;
            }
        }
    }
}