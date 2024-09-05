using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Properties and rigid body needed for red powerup physics 
    [SerializeField]
    float speed;

    private Rigidbody2D rb;

    //Variables for references to particles and sound manager
    [SerializeField] private ParticleSystem enemyDestroy;

    private soundManager sndManager;
    
    //Set bullet properties on instantiation
    void Awake()
    {
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }


    private void spawnDestroyParticles(Vector3 robotPosition)
    {
        Instantiate(enemyDestroy, robotPosition, Quaternion.identity);
    }

    //Check bullet collision with enemy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            sndManager.PlaySFX(sndManager.enemyDefeated);
            Destroy(gameObject);
            Destroy(other.gameObject.transform.parent.gameObject);
            spawnDestroyParticles(other.transform.position);
        }
        else if (other.gameObject.tag == "Structure"){
            Destroy(gameObject);
        }
    }
}
