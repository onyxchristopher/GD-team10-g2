﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingScript : MonoBehaviour
{

    public Transform shootingPoint;
    public GameObject bulletPrefab;



    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
        }
    }
}
