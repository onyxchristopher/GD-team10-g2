using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emp : MonoBehaviour
{
    public float duration = 1.0f; // Duration the EMP pulse exists
    public float radius = 2.0f; // Radius of the EMP pulse

    private void Start()
    {
        // Destroy the EMP object after the duration
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit by EMP!");
            
            Destroy(collision.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the EMP pulse radius for visualization in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}