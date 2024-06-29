using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int life;

    private void OnEnable()
    {
        EventManager.PlayerDetected += PlayerSpotted;
    }
    private void OnDisable()
    {
        EventManager.PlayerDetected -= PlayerSpotted;
    }

    private void PlayerSpotted()
    {
        Debug.Log("OH NOOOO!!!");
    }
}
