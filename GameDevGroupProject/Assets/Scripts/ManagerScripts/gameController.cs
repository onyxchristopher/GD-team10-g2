using System.Collections;
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

    private int[] starsObtained;


    // Start is called before the first frame update
    void Start()
    {
        currentState = gameState.running;

        if (currentState == gameState.running)
        {
            Debug.Log("Gamestate is running");
        }

        starsObtained = new int[5];
    }

    void Update(){
        //Debug.Log(gameObject.activeSelf);
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

    public void SetStars(int level, int stars){
        starsObtained[level - 1] = stars;
    }

    public void OpenAchievementScreen(){
        // Display achievement screen
        Instantiate(achievScreen);

        // Iterate through all levels
        for (int i = 1; i <= 5; i++){
            // Iterate through all stars in current level
            for (int j = 1; j <= 3; j++){
                if (starsObtained[i - 1] == j){
                    Image star = GameObject.Find($"L{i}S{j}").GetComponent<Image>();
                    star.color = new Color32(255, 255, 255, 255);
                }
            }
        }
    }

}