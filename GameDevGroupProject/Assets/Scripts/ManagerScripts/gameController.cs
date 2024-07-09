using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{

    // Possible game states
    public enum gameState { running, pause, loading };
    private gameState currentState;

    // Some stats we may want to keep track of 

    // Variable to keep track of enemies killed
    public int enemyKillCount;

    // Variable to keep track of player deaths
    public int playerDeaths;

    // For red powerup
    public bool allowRedPowerup = false;

    // For green powerup
    public bool allowGreenPowerup = false;

    // For blue powerup
    public bool allowBluePowerup = false;

    public bool onePowerUpOnly = true;
    // Start is called before the first frame update
    void Start()
    {
        currentState = gameState.running;

        enemyKillCount = 0;
        playerDeaths = 0;

        if (currentState == gameState.running)
        {
            Debug.Log("Gamestate is running");
        }

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
            Debug.Log("Game has been paused");
        }

        //Unpause game
        else if (currentState == gameState.pause)
        {
            currentState = gameState.running;
            Time.timeScale = 1;
            Debug.Log("Game has been unpaused");
        }

        return;
    }

}