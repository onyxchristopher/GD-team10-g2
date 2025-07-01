using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{
    // the number of levels in ./Scenes folder
    private int LevelCount = 5;
    private gameController gControl;

    void Start()
    {
        // grab the game controller when the script starts
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        
        // set up all the level buttons
        InstantiateLevelButtons();
    }

    void InstantiateLevelButtons()
    {
        // loop through all the levels we have
        for (int i = 0; i < LevelCount; i++)
        {
            // actual level numbers start at 1, not 0
            int actualLevelNumber = i + 1;

            GameObject levelButton = transform.GetChild(actualLevelNumber + 2).gameObject;

            // get the script component attached to the button
            LevelButtonInstance levelButtonInstance = levelButton.GetComponent<LevelButtonInstance>();
            
            // tell the button which level it should load when clicked
            levelButtonInstance.GoesToScene(actualLevelNumber);

            // only show buttons for levels the player has unlocked
            levelButton.SetActive(i < gControl.highestLevelReached);
        }
    }
}