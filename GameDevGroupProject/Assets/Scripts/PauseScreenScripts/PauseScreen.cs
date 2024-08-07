using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public Button achievementsButton;

    void Start()
    {
        achievementsButton?.onClick.AddListener(AchievementsButtonClicked);

        OpenMenuHideOthers("MainPauseMenu");
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
        // Load the LevelSelectScreen scene
        //   UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelectScreen");
    }

    private void AchievementsButtonClicked()
    {
        OpenMenuHideOthers("Achievements");
    }
}