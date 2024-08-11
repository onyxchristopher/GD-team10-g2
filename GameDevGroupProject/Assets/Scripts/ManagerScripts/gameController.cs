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
    // Pause screen prefab
    [SerializeField] private GameObject pauseScreen;
    // Pause button UI prefab
    [SerializeField] private GameObject pauseButtonUI;
    // End screen prefab
    public GameObject endScreen;
    // Mission complete screen prefab
    public GameObject completeScreen;

    private int[] starsObtained;

    private soundManager sndManager;

    // Start is called before the first frame update
    void Start()
    {
        currentState = gameState.running;

        starsObtained = new int[5];
        Debug.Log($"Gamestate is {CurrentGameState()}");
    }

    //Return current game state
    public gameState CurrentGameState()
    {
        return currentState;
    }

    //Pauses and unpauses game state
    public void Pause()
    {
        
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        sndManager.PauseBGM();

        currentState = gameState.pause;
        Time.timeScale = 0;
        Debug.Log("Game has been paused");

        return;
    }

    public void Unpause()
    {
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        sndManager.PlayBGM();
        Instantiate(pauseButtonUI);

        currentState = gameState.running;
        Time.timeScale = 1;
        Debug.Log("Game has been unpaused");

        return;
    }

    public void PauseScreen()
    {
        GameObject screen = Instantiate(pauseScreen);
        screen.name = "Pause Screen";
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
                else
                {
                    GameObject.Find($"L{i}S{j}")
                    .GetComponent<Image>()
                    .color = new Color32(180, 170, 169, 180);
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