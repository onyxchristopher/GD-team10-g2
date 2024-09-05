using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

//We will use this script to everything related to Loading, Unloading scenes and information about the level.
public class sceneManager : MonoBehaviour
{
    //This is how levels are named in the ./Scenes folder
    //We just add the level number after this prefix
    private const string LevelScenePrefix = "Level_";

    [SerializeField] GameObject restartScreen;
    
    // The current level
    public int currLevel;

    // Script references
    private soundManager sndManager;
    private cameraMovement camMove;
    private gameController gControl;
    private playerBehavior pBehavior;
    

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1080, 1920, true);
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        camMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameraMovement>();
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();

        pBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<playerBehavior>();

        currLevel = 1;
        StartGame();

    }

    //Loads initial scenes for game to start
    public void StartGame()
    {
        if (!SceneManager.GetSceneByName("Level_1").isLoaded){
            SceneManager.LoadSceneAsync("Level_1", LoadSceneMode.Additive);
        }
        

        //Loop through all scenes and unload every level but level 1 and persistent elements
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
           
            if (scene.name != "Level_1" && scene.name != "Persistent_Elements")
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }

    public void LoadLevel(string nextLevel)
    {
        string nextSceneName = LevelScenePrefix + nextLevel;
        //Check if there is a level to load and if its not already loaded
        if(nextSceneName != "" && !SceneManager.GetSceneByName(nextSceneName).isLoaded)
        {
            SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        }
        gControl.NewLevelState();
    }

    public bool UnloadCurrentLevel()
    {
        try
        {
            SceneManager.UnloadSceneAsync(LevelScenePrefix + currLevel);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public void RestartLevel()
    {
        if(UnloadCurrentLevel())
        {
            LoadLevel(currLevel.ToString());
        }
        else
        {
            RestartLevel();
        }
    }
}
