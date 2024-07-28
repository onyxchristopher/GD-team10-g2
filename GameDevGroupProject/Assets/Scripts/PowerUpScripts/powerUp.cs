﻿using System;
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

    [HideInInspector]
    public bool isActive = false;

    private playerBehavior pBehavior;

    //References to manangers
    private soundManager sndManager;

    void Start()
    {
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        pBehavior = GameObject.FindWithTag("Player").GetComponent<playerBehavior>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (pBehavior != null && other.gameObject.CompareTag("Player"))
        {   
            if (pBehavior.AddPowerUp(this))
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