using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulseCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Robot")
        {
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}
