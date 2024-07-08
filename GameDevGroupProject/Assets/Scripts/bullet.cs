using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] float speed;
    private Rigidbody2D rb;

    private Vector3 Direction;

    public PlayerMovement PlayerMovement;
    void Start()
    {
        PlayerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        
        rb = GetComponent<Rigidbody2D>();
        
        Direction = ((Vector3)PlayerMovement.FacingDirection) + Vector3.zero;
        
        rb.velocity = Direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            Destroy(other.gameObject.transform.parent.gameObject);
        }
        else if (other.gameObject.tag == "Structure")
        {
            Destroy(gameObject);
        }
    }
}