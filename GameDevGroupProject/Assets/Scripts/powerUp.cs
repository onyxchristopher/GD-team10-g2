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

    //References to manangers
    private soundManager sndManager;

    void Start()
    {
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = GameObject.FindWithTag("Player").GetComponent<playerBehavior>();

        if (player != null && other.gameObject.CompareTag("Player"))
        {   
            if (player.AddPowerUp(this))
            {
                isActive = true;
            }

            //Sound effects for powerup pickups
            if(powerUpType == PowerUpType.Red)
            {
                sndManager.PlaySFX(sndManager.powerUpPickUpRed);
            }

            if (powerUpType == PowerUpType.Green)
            {
                sndManager.PlaySFX(sndManager.powerUpPickUpGreen);
            }

            if (powerUpType == PowerUpType.Blue)
            {
                sndManager.PlaySFX(sndManager.powerUpPickUpPulse);
            }
        }
    }
}