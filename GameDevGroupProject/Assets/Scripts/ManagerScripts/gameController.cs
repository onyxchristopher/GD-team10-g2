﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    // Possible game states
    public enum gameState
    {
        running,
        pause,
        loading
    };

    private gameState currentState;

    // Prefab references
    [SerializeField] private GameObject achievScreen;
    [SerializeField] private GameObject levelsScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject pauseButtonUI;
    public GameObject endScreen;
    public GameObject completeScreen;

    // Stars obtained per level
    private int[] starsObtained;

    // Highest level reached by the player so far
    public int highestLevelReached = 1;

    // Script references
    private sceneManager scnManager;
    private soundManager sndManager;
    private playerBehavior pBehavior;
    private cameraMovement camMove;

    // Start is called before the first frame update
    void Start()
    {
        
        currentState = gameState.running;
        starsObtained = new int[5];

        scnManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<sceneManager>();
        pBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<playerBehavior>();
        camMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameraMovement>();
    }

    //Return current game state
    public gameState CurrentGameState()
    {
        return currentState;
    }

    //Pauses and unpauses game state
    public void Pause()
    {
        currentState = gameState.pause;

        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        sndManager.PauseBGM();
        
        GameObject pauseUI = GameObject.FindGameObjectWithTag("PauseUI");
        Destroy(pauseUI);

        Time.timeScale = 0;
    }

    public void Unpause()
    {
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        sndManager.PlayBGM();
        Instantiate(pauseButtonUI);

        currentState = gameState.running;
        Time.timeScale = 1;
    }

    //Instantiates pause screen
    public void PauseScreen()
    {
        GameObject screen = Instantiate(pauseScreen);
        screen.name = "Pause Screen";
    }

    // Reset the player to a fresh start on a level
    public void NewLevelState()
    {
        // these are null for some reason so we need to get them right in this function
        pBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<playerBehavior>();
        scnManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<sceneManager>();
        camMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameraMovement>();
        
        //TP player to current level start
        pBehavior.MovePlayer(new Vector3(6, 150 * (scnManager.currLevel - 1) + 2, 0));
        //Remove powerups
        pBehavior.ClearPowerups();
        //Update camera
        camMove.SnapToPlayer();
    }

    /*
    If stars earned exceed previous stars earned in that level,
    set stars earned as the new value
    */
    public void SetStars(int level, int stars)
    {
        if (starsObtained[level - 1] < stars)
        {
            starsObtained[level - 1] = stars;
        }
    }

    // Open the achievement screen
    public void OpenAchievementScreen()
    {
        // Display achievement screen
        Instantiate(achievScreen);

        // Iterate through all levels
        for (int i = 1; i <= 5; i++)
        {
            // Iterate through all stars in current level
            for (int j = 1; j <= 3; j++){
                if (starsObtained[i - 1] >= j){
                    GameObject.Find($"L{i}S{j}L")
                   .GetComponent<Image>()
                   .color = new Color32(255, 255, 255, 255);

                    GameObject.Find($"L{i}S{j}")
                        .GetComponent<Image>()
                        .color = new Color32(255, 255, 255, 255);
                }
                else
                {
                    GameObject.Find($"L{i}S{j}L")
                    .GetComponent<Image>()
                    .color = new Color32(255, 255, 255, 255);
                }
            }
        }
    }

    public void OpenLevelsScreen()
    {
        // Display levels screen
        Instantiate(levelsScreen);
    }

    //Starts engame sequence 
    public void MissionComplete()
    {
        StartCoroutine(EndGameCoroutine());
    }

    private IEnumerator EndGameCoroutine()
    {
        //Getting reference to end of level script
        endTracker eTracker = GameObject.FindGameObjectWithTag("Exit Door").GetComponent<endTracker>();

        //Waiting for 5 seconds
        yield return new WaitForSeconds(5);

        //Build endscreen
        eTracker.BuildEndScreen(completeScreen);
        Pause();
    }
}