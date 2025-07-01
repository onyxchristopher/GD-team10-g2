using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime = 0.5f;

    void Start()
    {
        // get the platform effector component attached to this object
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        // Allow player to drop through the platform by pressing the down arrow key
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // start the coroutine to handle dropping through the platform
            StartCoroutine(DropThroughPlatform());
        }
    }

    private IEnumerator DropThroughPlatform()
    {
        // Temporarily disable the one-way collision
        effector.rotationalOffset = 180f;

        // Wait for a moment
        yield return new WaitForSeconds(waitTime);

        // Re-enable the one-way collision
        effector.rotationalOffset = 0f; 
    }
}