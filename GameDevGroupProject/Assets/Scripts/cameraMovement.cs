using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    private sceneManager sManager;
    private GameObject player;
    private float levelBottom;
    private float levelLeft;
    private float levelTop;
    private float levelRight;
    private float cameraLeftBound;
    private float cameraLowerBound;
    private float cameraRightBound;
    private float cameraUpperBound;
    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    float smoothingTime;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        // set the level bounds (later these will be fetched from the level manager)
        levelBottom = 0f;
        levelLeft = 0f;
        levelTop = 100f;
        levelRight = 32f;
        
        // get camera size and aspect info
        Camera camera = GetComponent<Camera>();
        float cameraHalfHeight = camera.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * camera.aspect;

        // get the bounds outside of which the camera should not move
        cameraLeftBound = levelLeft + cameraHalfWidth;
        cameraLowerBound = levelBottom + cameraHalfHeight;
        cameraRightBound = levelRight - cameraHalfWidth;
        cameraUpperBound = levelTop - cameraHalfHeight;
    }

    // Update is called once per frame
    void Update()
    {
        // calculate the ideal position of the camera
        Vector3 idealCamPos = new Vector3(player.transform.position.x, player.transform.position.y, -10f);

        // bound the position of the camera to within the level
        if (idealCamPos.x < cameraLeftBound){
            idealCamPos.Set(cameraLeftBound, idealCamPos.y, idealCamPos.z);
        } else if (idealCamPos.x > cameraRightBound){
            idealCamPos.Set(cameraRightBound, idealCamPos.y, idealCamPos.z);
        }
        if (idealCamPos.y < cameraLowerBound){
            idealCamPos.Set(idealCamPos.x, cameraLowerBound, idealCamPos.z);
        } else if (idealCamPos.y > cameraUpperBound){
            idealCamPos.Set(idealCamPos.x, cameraUpperBound, idealCamPos.z);
        }

        transform.position = Vector3.SmoothDamp(transform.position, idealCamPos, ref velocity, smoothingTime); 
    }
}
