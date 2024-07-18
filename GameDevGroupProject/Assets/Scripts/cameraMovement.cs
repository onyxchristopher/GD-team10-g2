using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    private GameObject player;
    private Vector3 cameraOffset;
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
        cameraOffset = new Vector3(player.transform.position.x, transform.position.y - player.transform.position.y, -10f);
        
        Camera cam = GetComponent<Camera>();
        float cameraHalfWidth = cam.orthographicSize * cam.aspect;
        cameraLeftBound = levelLeftBound + cameraHalfWidth;
        cameraRightBound = levelRightBound - cameraHalfWidth;
    }

    Vector3 CalcIdealPosition()
    {
        // calculate the ideal position of the camera, bounded by the left and right of the level
        Vector3 idealCamPos = player.transform.position + cameraOffset;
        if (idealCamPos.x < cameraLeftBound){
            idealCamPos.Set(cameraLeftBound, idealCamPos.y, idealCamPos.z);
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
