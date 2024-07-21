using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerpackCollision : MonoBehaviour
{
    [SerializeField] private GameObject powerpack;
    private soundManager sndManager;

    void Start()
    {
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player")){
            Instantiate(powerpack, transform.position, transform.rotation);
            sndManager.PlaySFX(sndManager.powerpackFound, 0.8f);
            Destroy(gameObject);
        }
    }
}
