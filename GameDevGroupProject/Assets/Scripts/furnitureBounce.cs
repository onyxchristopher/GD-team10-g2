using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class furnitureBounce : MonoBehaviour
{
    
    [SerializeField] Vector2 launchVelocity;

    private soundManager sndManager;

    void Start()
    {
        //Reference to sound manager
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
    }

    //Detect collision with bouncy furniture
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            sndManager.PlaySFX(sndManager.furnitureBounce);
            collision.rigidbody.velocity = launchVelocity;
        }
    }
}