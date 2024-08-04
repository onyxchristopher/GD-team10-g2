using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antennaDish : MonoBehaviour
{
    public int antennaHP = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(antennaHP == 0)
        {
            AntennaDestroyed();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Pulse")
        {
            antennaHP--;
            Debug.Log("Antenna hp = " + antennaHP);
        }

    }

    private void AntennaDestroyed()
    {
        //Do something when antenna hp reaches 0
        Debug.Log("Antenna destroyed");

    }
}
