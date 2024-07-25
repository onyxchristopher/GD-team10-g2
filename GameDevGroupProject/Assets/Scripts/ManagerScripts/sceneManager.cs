using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//We will use this script to everything related to Loading, Unloading scenes and information about the level.
public class sceneManager : MonoBehaviour
{

    private Scene persistentElements;
    private Scene[] levelCollection;
    private GameObject player;
    private playerBehavior pBehavior;
    public int currLevel;
    private soundManager sndManager;
    private cameraMovement camMove;
    [SerializeField] GameObject restartScreen;

    // Start is called before the first frame update
    void Start()
    {
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        camMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameraMovement>();

        player = GameObject.FindGameObjectWithTag("Player");
        pBehavior = player.GetComponent<playerBehavior>();

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
        //Check if there is a level to load and if its not already loaded
        if(nextLevel != "" && !SceneManager.GetSceneByName(nextLevel).isLoaded )
        {
            SceneManager.LoadSceneAsync(nextLevel, LoadSceneMode.Additive);
        }
    }

    public void UnloadLevel(string level)
    {
        if(level != "")
        {
            SceneManager.UnloadSceneAsync(level);
        }
    }

    public void RestartLevel()
    {
        SceneManager.UnloadSceneAsync($"Level_{currLevel}");
        player.transform.position = new Vector3(6, 150 * (currLevel - 1) + 2, 0);
        camMove.SnapToPlayer();
        SceneManager.LoadSceneAsync($"Level_{currLevel}", LoadSceneMode.Additive);
        pBehavior.ClearPowerups();
        
    }

}
