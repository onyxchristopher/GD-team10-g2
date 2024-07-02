using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//We will use this script to everything related to Loading, Unloading scenes and information about the level.
public class sceneManager : MonoBehaviour
{

    private Scene persistentElements;
    private Scene[] levelCollection;

    // Start is called before the first frame update
    void Start()
    {

        StartGame();

        //Debug.Log("Amount of loaded sceenes " + sceneCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Loads initial scenes for game to start
    public void StartGame()
    {
        //Loop throguh all scenes and only load first two. Index starts at one to keep persistent elements loaded
        for (int i = 1; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if(scene.name == "PersistentElements")
            {
                SceneManager.SetActiveScene(scene);
            }
            if (scene.name != "Level_1" && scene.name != "Level_2")
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }

    public void NextLevel(string nextLevel)
    {
        //Check if there is a level to load and if its not already loaded
        if(nextLevel != "" && !SceneManager.GetSceneByName(nextLevel).isLoaded )
        {
            SceneManager.LoadSceneAsync(nextLevel, LoadSceneMode.Additive);
        }
    }

    public void UnloadScene(string level)
    {
        if(level != "")
        {
            SceneManager.UnloadSceneAsync(level);
        }
    }

    public void RestartLevel()
    {

    }

}
