using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulseCollision : MonoBehaviour
{

    //Variables to hold References to sound manager and particle animation
    private soundManager sndManager;

    [SerializeField] private ParticleSystem enemyDestroy;

    void Awake()
    {
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
    }

    //Instantiates particles after robot destruction
    private void spawnDestroyParticles(Vector3 robotPosition)
    {
        Instantiate(enemyDestroy, robotPosition, Quaternion.identity);
    }
    
    //Check pulse collision with enemy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            sndManager.PlaySFX(sndManager.enemyDefeated);
            Destroy(other.gameObject.transform.parent.gameObject);
            spawnDestroyParticles(other.transform.position);
        }
    }
}
