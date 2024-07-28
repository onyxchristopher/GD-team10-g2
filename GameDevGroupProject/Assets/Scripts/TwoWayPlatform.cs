using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime = 0.5f;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        // Allow player to drop through the platform by pressing the down arrow key
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(DropThroughPlatform());
        }
    }

    private IEnumerator DropThroughPlatform()
    {
        effector.rotationalOffset = 180f; // Temporarily disable the one-way collision
        yield return new WaitForSeconds(waitTime); // Wait for a moment
        effector.rotationalOffset = 0f; // Re-enable the one-way collision
    }
}