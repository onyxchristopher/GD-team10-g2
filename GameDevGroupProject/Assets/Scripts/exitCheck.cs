using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitCheck : MonoBehaviour
{
    [SerializeField] 
    string nextLevel;

    [SerializeField] 
    string currentLevel;

    private sceneManager scnManager;
    private playerBehavior pBehavior;
    private gameController gControl;
    private cameraMovement mainCam;

    [SerializeField] private GameObject nextLevelEntry;


    // Start is called before the first frame update
    void Start()
    {
        scnManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<sceneManager>();
        pBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<playerBehavior>();
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<cameraMovement>();
    }

    //If player reached exit, call sceneManager to load next level
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Unload pre level
            scnManager.UnloadLevel(currentLevel);
            //Load next level
            scnManager.LoadLevel(nextLevel);
            //Tp player
            pBehavior.MovePlayer(nextLevelEntry.transform);
            //Set current level
            scnManager.currLevel++;
            //Remove pUps
            pBehavior.ClearPowerups();
            //Update camera
            mainCam.SnapToPlayer();
            //Pause game to avoid player moving in UI
            gControl.Pause();
            //Destroy pause UI to avoid player unpausing game in menu
            GameObject pauseUI = GameObject.FindGameObjectWithTag("PauseUI");
            if (pauseUI){
                Destroy(pauseUI);
            }
            
        }
    }

}
