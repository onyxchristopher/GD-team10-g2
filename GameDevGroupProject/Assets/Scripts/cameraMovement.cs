using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    private GameObject player;
    [SerializeField] float levelCenter = 30.5f;
    private float cameraOffset;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] float smoothingTime;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        cameraOffset = transform.position.y - player.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // calculate the ideal position of the camera
        Vector3 idealCamPos = new Vector3(levelCenter, player.transform.position.y + cameraOffset, -10f);

        transform.position = Vector3.SmoothDamp(transform.position, idealCamPos, ref velocity, smoothingTime); 
    }
}
