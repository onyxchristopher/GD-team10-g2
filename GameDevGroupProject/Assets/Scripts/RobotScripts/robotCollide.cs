using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Logic for mesh collision 
public class robotCollide : MonoBehaviour
{
    private robotBehavior rBehavior;

    // Start is called before the first frame update
    void Start()
    {
        rBehavior = gameObject.GetComponentInParent<robotBehavior>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        rBehavior.Detected();
    }
}
