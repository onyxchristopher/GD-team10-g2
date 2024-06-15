using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventManager : MonoBehaviour
{

    //Possible game states
    public enum gameState { running, pause, loading };
    private gameState currentState;

    //Some stats we may want to keep track of 

    //Variable to keep track of enemies killed
    public int enemyKillCount;

    //Variable to keep track of player deaths
    public int playerDeahts;

    // Start is called before the first frame update
    void Start()
    {
        currentState = gameState.running;

        enemyKillCount = 0;
        playerDeahts = 0;

        if (currentState == gameState.running)
        {
            Debug.Log("Gamestate is running");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Return current game state
    public gameState CurrentGameState()
    {
        return currentState;
    }

    //Changes game state to pause
    public void PauseMenu()
    {
        //Pause Game
        if (currentState == gameState.running)
        {
            currentState = gameState.pause;
            Time.timeScale = 0;
            Debug.Log("Game as been paused");
        }

        //Unpause game
        else if (currentState == gameState.pause)
        {
            currentState = gameState.running;
            Time.timeScale = 1;
            Debug.Log("Game as been unpaused");
        }

        return;
    }

}