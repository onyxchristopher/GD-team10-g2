using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    // Player reference
    private GameObject player;

    // Camera component;
    private Camera cam;

    // Camera offset from player
    private Vector3 cameraOffset;

    // Camera velocity for smoothing
    private Vector3 velocity = Vector3.zero;

    // Time to dampen camera movement
    [SerializeField] float smoothingTime;

    // Stored camera aspect
    private float prevCamAspect;

    // Camera bounds
    private int levelLeftBound = -7;
    private int levelRightBound = 69;
    private float cameraLeftBound;
    private float cameraRightBound;

    private float levelCenter = 31;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        cameraOffset = new Vector3(1, transform.position.y - player.transform.position.y, -10f);
        
        cam = GetComponent<Camera>();
        prevCamAspect = cam.aspect;
        CalcCameraBounds(prevCamAspect);
        
    }

    private void CalcCameraBounds(float aspect)
    {
        float cameraHalfWidth = cam.orthographicSize * aspect;
        cameraLeftBound = levelLeftBound + cameraHalfWidth;
        cameraRightBound = levelRightBound - cameraHalfWidth;
    }

    Vector3 CalcIdealPosition()
    {
        if (cam.aspect != prevCamAspect) {
            CalcCameraBounds(cam.aspect);
        }

        Vector3 idealCamPos;
        idealCamPos = player.transform.position + cameraOffset;

        // on ultrawide resolutions, the camera is in the center of the level all the time
        if (cam.aspect >= 2.375) {
            idealCamPos.Set(levelCenter, idealCamPos.y, idealCamPos.z);
            return idealCamPos;
        }

        // calculate the ideal position of the camera, bounded by the left and right of the level
        if (idealCamPos.x < cameraLeftBound){
            idealCamPos.Set(cameraLeftBound, idealCamPos.y, idealCamPos.z);
            return idealCamPos;
        } else if (idealCamPos.x > cameraRightBound){
            idealCamPos.Set(cameraRightBound, idealCamPos.y, idealCamPos.z);
        } 

        return idealCamPos;
    }

    public void SnapToPlayer()
    {
        transform.position = CalcIdealPosition();;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, CalcIdealPosition(), ref velocity, smoothingTime); 
    }
}
