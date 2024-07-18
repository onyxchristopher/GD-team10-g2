using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//We will use this script to everything related to Loading, Unloading scenes and information about the level.
public class sceneManager : MonoBehaviour
{

    private Scene persistentElements;
    private Scene[] levelCollection;
    public Vector3[] levelStarts = new Vector3[1];
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
        levelStarts[0] = new Vector3(6, 2, 0);
        StartGame();

        //Debug.Log("Amount of loaded sceenes " + sceneCount);
    }

    //Loads initial scenes for game to start
    public void StartGame()
    {
        if (!SceneManager.GetSceneByName("Level_1").isLoaded){
            SceneManager.LoadSceneAsync("Level_1", LoadSceneMode.Additive);
        }
        
        /*
        Scene scene = SceneManager.GetSceneByName("Persistent elements");

        //Loop through all scenes and only load first two. Index starts at one to keep persistent elements loaded
        for (int i = 1; i < SceneManager.sceneCount; i++)
        {
            scene = SceneManager.GetSceneAt(i);
           
            if (scene.name != "Level_1" && scene.name != "Level_2")
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }*/
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
        SceneManager.UnloadSceneAsync("Level_1");
        player.transform.position = levelStarts[currLevel - 1];
        camMove.SnapToPlayer();
        SceneManager.LoadSceneAsync("Level_1", LoadSceneMode.Additive);
        pBehavior.ClearPowerups();
        
    }

}
