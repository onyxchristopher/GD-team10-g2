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

    void Start()
    {
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        camMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameraMovement>();
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();

        pBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<playerBehavior>();

        currLevel = 1;
        // Load level 1 if it is not loaded
        if (!SceneManager.GetSceneByName("Level_1").isLoaded)
        {
            SceneManager.LoadSceneAsync("Level_1", LoadSceneMode.Additive);
        }

        // Unload any scenes other than level 1 and persistent elements
        List<string> scenesToUnload = new List<string>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name != "Level_1" && scene.name != "Persistent_Elements")
            {
                scenesToUnload.Add(scene.name);
            }
        }
        for (int i = 0; i < scenesToUnload.Count; i++)
        {
            SceneManager.UnloadSceneAsync(scenesToUnload[i]);
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
            Debug.Log(e);
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
