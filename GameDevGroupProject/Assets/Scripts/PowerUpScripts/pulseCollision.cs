using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulseCollision : MonoBehaviour
{

    private soundManager sndManager;

    void Awake()
    {
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            sndManager.PlaySFX(sndManager.enemyDefeated);
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}
