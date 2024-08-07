﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    // Possible game states
    public enum gameState { running, pause, loading };
    private gameState currentState;


    // Prefab achievement screen
    [SerializeField] private GameObject achievScreen;
    [SerializeField] private GameObject pauseScreen;

    // End screen prefab
    public GameObject endScreen;
    
    // Mission complete screen prefab
    public GameObject completeScreen;

    private int[] starsObtained;


    // Start is called before the first frame update
    void Start()
    {
        currentState = gameState.running;

        starsObtained = new int[5];
    }

    //Return current game state
    public gameState CurrentGameState()
    {
        return currentState;
    }

    //Changes game state to pause
    public void Pause()
    {
        //Pause Game
        if (currentState == gameState.running)
        {
            currentState = gameState.pause;
            Time.timeScale = 0;
            Debug.Log("Game has been paused");
            AudioListener.pause = true;
            AudioListener.volume = 0.03f;
        }

        //Unpause game
        else if (currentState == gameState.pause)
        {
            currentState = gameState.running;
            Time.timeScale = 1;
            Debug.Log("Game has been unpaused");
            AudioListener.pause = false;
            AudioListener.volume = 1.0f;
        }

        return;
    }

    public void PauseScreen()
    {
        Instantiate(pauseScreen);
    }

    /*
    If stars earned exceed previous stars earned in that level,
    set stars earned as the new value
    */
    public void SetStars(int level, int stars){
        // 
        if (starsObtained[level - 1] < stars){
            starsObtained[level - 1] = stars;
        }
        
    }

    // Open the achievement screen
    public void OpenAchievementScreen(){
        // Display achievement screen
        Instantiate(achievScreen);

        // Iterate through all levels
        for (int i = 1; i <= 5; i++){
            // Iterate through all stars in current level
            for (int j = 1; j <= 3; j++){
                if (starsObtained[i - 1] >= j){
                    GameObject.Find($"L{i}S{j}")
                    .GetComponent<Image>()
                    .color = new Color32(255, 255, 255, 255);
                }
            }
        }
    }

    public void MissionComplete()
    {
        StartCoroutine(EndGameCoroutine());
    }

    private IEnumerator EndGameCoroutine()
    {
        Debug.Log("Start of coroutine: " + Time.time);
        //Getting reference to end of level script
        endTracker eTracker = GameObject.FindGameObjectWithTag("Exit Door").GetComponent<endTracker>();

        //Waiting for 5 seconds
        yield return new WaitForSeconds(5);

        //Build endscreen
        eTracker.BuildEndScreen(completeScreen);

        Debug.Log("End of coroutine: " + Time.time);
    }

}