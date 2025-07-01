using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public Button achievementsButton;

    void Start()
    {
        // add click listener to the achievements button if it exists
        achievementsButton?.onClick.AddListener(AchievementsButtonClicked);

        // initialize the pause screen with the main menu
        OpenMenuHideOthers("MainPauseMenu");
    }


    // hides other menus and sets the selected menu as active
    private void OpenMenuHideOthers(string menuName)
    {
        // iterate through all child objects of the pause screen
        foreach (var child in gameObject.transform)
            if (child is Transform pauseScreenChild)
                // set active state based on whether the child's name matches the target menu
                pauseScreenChild.gameObject.SetActive(pauseScreenChild.name == menuName);
    }

    // handler for achievements button click
    private void AchievementsButtonClicked()
    {
        // switch to the achievements menu
        OpenMenuHideOthers("Achievements");
    }
}