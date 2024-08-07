using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{

    public GameObject levelSelectionLayout;

    public GameObject levelSelectButtonPrefab;
    // this is how levels are named in the ./Scenes folder
    // we just add the level number after this prefix
    private const string LevelScenePrefix = "Level_";

    // the number of levels in ./Scenes folder
    public int LevelCount = 5;
    public int CurrentlyCompletedLevels = 2;

    void Start()
    {
        InstantiateLevelButtons();
    }

    void InstantiateLevelButtons()
    {
        // clear the current level buttons 
        for (int i = 0; i < LevelCount; i++)
        {
            var actualLevelNumber = (i + 1);

            var levelSelectButton = Instantiate(levelSelectButtonPrefab, levelSelectionLayout.transform);

            if (levelSelectButton.GetComponent<LevelButtonInstance>() is LevelButtonInstance levelButtonInstance)
            {
                levelButtonInstance.SetText($"{actualLevelNumber}");

                levelButtonInstance.GoesToScene(LevelScenePrefix + actualLevelNumber);

                levelButtonInstance.isEnabled = i <= CurrentlyCompletedLevels;
            }
            else
            {
                Debug.LogError($"{levelSelectButton} has no LevelButtonInstance!");
            }
        }
    }
}