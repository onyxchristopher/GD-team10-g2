using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotCollide : MonoBehaviour
{
    private robotBehavior rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponentInParent<robotBehavior>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        rb.Detected(collision.otherCollider);
    }
}
