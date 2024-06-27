using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//We will use this script to everything related to Loading, Unloading scenes and information about the level.
public class sceneManager : MonoBehaviour
{

    private Scene currentLevel;
    private Scene[] levelCollection;

    public int sceneCount; 

    // Start is called before the first frame update
    void Start()
    {
        sceneCount = SceneManager.sceneCount;

        currentLevel = SceneManager.GetActiveScene();

        Debug.Log("Scene loaded " + currentLevel.name);
        Debug.Log("Amount of loadeed sceenes " + sceneCount);

        for (int i = 0; i < sceneCount; i++)
        {
           // Debug.Log("Loaded scene " + levelCollection[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel(string nextLevel)
    {
        SceneManager.LoadScene(nextLevel, LoadSceneMode.Additive);
    }

    public void UnloadScene(string currentLevel)
    {
        SceneManager.UnloadSceneAsync(currentLevel);
    }

    public void RestartLevel()
    {

    }

}
