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
    private soundManager sndManager;
    private cameraMovement mainCam;

    [SerializeField] private GameObject nextLevelEntry;


    // Start is called before the first frame update
    void Start()
    {
        scnManager = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<sceneManager>();
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponentInChildren<soundManager>();
        pBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<playerBehavior>();
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<cameraMovement>();
        //nextLevelEntry = GameObject.FindGameObjectWithTag("NextEntry");

    }

    //If player reached exit, call sceneManager to load next level
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" )
        {
            sndManager.PlaySFX(sndManager.levelComplete);

            //Load next level
            scnManager.LoadLevel(nextLevel);
            //Tp player
            pBehavior.MovePlayer(nextLevelEntry.transform);
            //Remove pUps
            pBehavior.ClearPowerups();
            //Update camera
            mainCam.SnapToPlayer();
            //Unload pre level
            scnManager.UnloadLevel(currentLevel);
            //sndManager.StopBGM();
        }
    }

}
