using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField]
    float speed;
    private Rigidbody2D rb;

    soundManager sManager;

    void Start()
    {
        sManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            sManager.PlaySFX(sManager.enemyDefeated);
            Destroy(gameObject);
            Destroy(other.gameObject.transform.parent.gameObject);
        } else if (other.gameObject.tag == "Structure"){
            Destroy(gameObject);
        }
    }
}
