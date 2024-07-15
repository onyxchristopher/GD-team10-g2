using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    private GameObject player;
    private float cameraOffset;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] float smoothingTime;
    private int levelLeftBound = -7;
    private int levelRightBound = 69;
    private float cameraLeftBound;
    private float cameraRightBound;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        cameraOffset = transform.position.y - player.transform.position.y;
        
        Camera cam = GetComponent<Camera>();
        float cameraHalfWidth = cam.orthographicSize * cam.aspect;
        cameraLeftBound = levelLeftBound + cameraHalfWidth;
        cameraRightBound = levelRightBound - cameraHalfWidth;
    }

    // Update is called once per frame
    void Update()
    {
        // calculate the ideal position of the camera, bounded by the left and right of the level
        Vector3 idealCamPos = new Vector3(player.transform.position.x, player.transform.position.y + cameraOffset, -10f);
        if (idealCamPos.x < cameraLeftBound){
            idealCamPos.Set(cameraLeftBound, idealCamPos.y, idealCamPos.z);
        } else if (idealCamPos.x > cameraRightBound){
            idealCamPos.Set(cameraRightBound, idealCamPos.y, idealCamPos.z);
        }

        transform.position = Vector3.SmoothDamp(transform.position, idealCamPos, ref velocity, smoothingTime); 
    }
}
