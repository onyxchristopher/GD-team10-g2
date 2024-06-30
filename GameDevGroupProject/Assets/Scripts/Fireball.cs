using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f; // Speed of the fireball
    public float lifeTime = 5f; // Time before the fireball is destroyed

    private void Start()
    {
        // Destroy the fireball after lifeTime seconds
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Move the fireball horizontally
        transform.Translate(Vector3.right * speed * Time.deltaTime);
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