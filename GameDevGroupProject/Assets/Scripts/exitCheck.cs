using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitCheck : MonoBehaviour
{
    private sceneManager scnManager;
    private gameController gControl;

    // Start is called before the first frame update
    void Start()
    {
        scnManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<sceneManager>();
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
    }

    //If player reached exit, call sceneManager to load next level
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Unload pre level
            scnManager.UnloadCurrentLevel();
            //Set current level
            scnManager.currLevel++;
            //Load next level
            scnManager.LoadLevel(scnManager.currLevel.ToString());
            // Check player's progression
            LevelProgressionCheck();
            //Pause game to avoid player moving while in UI
            gControl.Pause();
            //Destroy pause UI to avoid player unpausing game in menu
            GameObject pauseUI = GameObject.FindGameObjectWithTag("PauseUI");
            if (pauseUI){
                Destroy(pauseUI);
            }
            
        }
    }

    //Check if player has completed a new level for the first time
    private void LevelProgressionCheck()
    {
        if (scnManager.currLevel > gControl.highestLevelReached)
        {
            gControl.highestLevelReached = scnManager.currLevel;
        }
    }

}
