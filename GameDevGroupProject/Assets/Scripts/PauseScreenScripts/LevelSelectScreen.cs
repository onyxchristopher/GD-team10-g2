using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{
    // the number of levels in ./Scenes folder
    private int LevelCount = 5;
    private gameController gControl;

    void Start()
    {
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        InstantiateLevelButtons();
    }

    void InstantiateLevelButtons()
    {
        for (int i = 0; i < LevelCount; i++)
        {
            int actualLevelNumber = i + 1;

            GameObject levelButton = transform.GetChild(actualLevelNumber + 2).gameObject;

            LevelButtonInstance levelButtonInstance = levelButton.GetComponent<LevelButtonInstance>();
            
            levelButtonInstance.GoesToScene(actualLevelNumber);

            levelButton.SetActive(i < gControl.highestLevelReached);
        }
    }
}