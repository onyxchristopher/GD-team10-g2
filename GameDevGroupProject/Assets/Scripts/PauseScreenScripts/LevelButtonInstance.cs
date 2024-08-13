using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonInstance : MonoBehaviour
{
    private int targetScene;
    private sceneManager scnManager;
    private gameController gControl;

    private void Start()
    {
        scnManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<sceneManager>();
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
    }

    // Perform all necessary operations to start the player in a level
    void GoToScene()
    {
        scnManager.UnloadCurrentLevel();
        scnManager.currLevel = targetScene;
        scnManager.LoadLevel(targetScene.ToString());
        gControl.Unpause();
    }

    // the scene this button goes when clicked
    public void GoesToScene(int sceneName)
    {
        targetScene = sceneName;
        this.GetComponent<Button>().onClick.AddListener(GoToScene);
    }
}