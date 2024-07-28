using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

// we are assuming all gameobjects below or children of this gameobject
// are the menus!!!!
public class LevelSelectScreen : MonoBehaviour
{
    public Button levelSelectionButton;
    public Button achievementsButton;

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
        levelSelectionButton.onClick.AddListener(LevelSelectionButtonClicked);
        achievementsButton.onClick.AddListener(AchievementsButtonClicked);

        OpenMenuHideOthers("MainPauseMenu");

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
                levelButtonInstance.SetText("Level " + actualLevelNumber);

                levelButtonInstance.GoesToScene(LevelScenePrefix + actualLevelNumber);

                levelButtonInstance.isEnabled = i <= CurrentlyCompletedLevels;
            }
        }
    }

    // hides other menus and sets the selected menu as active
    private void OpenMenuHideOthers(string menuName)
    {
        foreach (var child in gameObject.transform)
            if (child is Transform pauseScreenChild)
                pauseScreenChild.gameObject.SetActive(pauseScreenChild.name == menuName);
    }

    private void LevelSelectionButtonClicked()
    {
        OpenMenuHideOthers("LevelSelectMenu");
    }

    private void AchievementsButtonClicked()
    {
        // or the acheivements scene
        OpenMenuHideOthers("Achievements");
    }
}