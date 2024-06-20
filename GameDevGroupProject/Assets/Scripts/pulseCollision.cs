using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulseCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OntriggerEnter call");

        if (other.gameObject.tag == "Robot")
        {
            Debug.Log("Pulse has collided with enemy");
            Destroy(other.gameObject);
        }
    }
}
