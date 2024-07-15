using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class furnitureBounce : MonoBehaviour
{
    [SerializeField] Vector2 launchVelocity;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            collision.rigidbody.velocity = launchVelocity;
        }
    }
}
