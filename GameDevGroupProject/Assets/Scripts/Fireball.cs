using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f; // Speed of the fireball
    public float lifeTime = 5f; // Time before the fireball is destroyed

    public PlayerMovement PlayerMovement;

    private Vector3 Direction;

    private void Start()
    {
        PlayerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        // Destroy the fireball after lifeTime seconds
        Destroy(gameObject, lifeTime);

        Direction = ((Vector3)PlayerMovement.FacingDirection) + Vector3.zero;
    }

    private void FixedUpdate()
    {
        // Move the fireball horizontally
        transform.Translate((Direction) * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Destroy the robot on contact
            Destroy(collision.gameObject);

            // Destroy the fireball on contact
            Destroy(gameObject);
        }
    }
}