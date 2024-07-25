using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    private ParticleSystem enemyDestroy;

    private ParticleSystem enemyDestroyInstance;

    private Rigidbody2D rb;

    private soundManager sndManager;

    void Awake()
    {
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }


    private void spawnDestroyParticles()
    {
        enemyDestroyInstance = Instantiate(enemyDestroy, transform.position, Quaternion.identity);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            sndManager.PlaySFX(sndManager.enemyDefeated);
            Destroy(gameObject);
            Destroy(other.gameObject.transform.parent.gameObject);
            spawnDestroyParticles();
        }
        else if (other.gameObject.tag == "Structure"){
            Destroy(gameObject);
        }
    }
}
