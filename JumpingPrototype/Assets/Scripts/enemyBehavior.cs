using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Pulse")
        {
            Debug.Log("Enemy hit by pulse");
        }
    }
}
