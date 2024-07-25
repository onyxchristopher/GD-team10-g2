using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulseCollision : MonoBehaviour
{

    soundManager sManager;

    void Awake()
    {
        sManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            sManager.PlaySFX(sManager.enemyDefeated);
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}
