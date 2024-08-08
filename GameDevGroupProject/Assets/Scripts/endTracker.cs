using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endTracker : MonoBehaviour
{
    // Total powerpacks found this level
    private int powerpacksFound = 0;

    // Total robots destroyed this level
    private int robotsDestroyed = 0;

    // Total powerpacks in this level
    private int totalPowerpacks;

    // Total robots in this level
    private int totalRobots;

    // Text objects for robots/powerpacks stats
    private Text robotsText;
    private Text powerpacksText;

    // Image objects for yellow stars
    private Image robotStar;
    private Image powerpackStar;

    // Manager reference
    private sceneManager scnManager;
    private soundManager sndManager;
    private gameController gControl;

    private int level;

    void Start()
    {
        totalPowerpacks = GameObject.FindGameObjectsWithTag("Powerpack").Length;
        totalRobots = GameObject.FindGameObjectsWithTag("Robot").Length;
        scnManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<sceneManager>();
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        level = scnManager.currLevel;
    }

    // Resets all statistics (to use on restart)
    public void ResetAll(){
        powerpacksFound = 0;
        robotsDestroyed = 0;
    }

    // Calculate the number of powerpacks found
    private int NumPowerpacksFound(){
        int powerpacksLeft = GameObject.FindGameObjectsWithTag("Powerpack").Length;
        return totalPowerpacks - powerpacksLeft;
    }

    // Calculate the number of robots destroyed
    private int NumRobotsDestroyed(){
        int robotsLeft = GameObject.FindGameObjectsWithTag("Robot").Length;
        return totalRobots - robotsLeft;
    }
    
    // Evaluate player achievements and display endscreen
    void OnCollisionEnter2D(Collision2D collision){

        BuildEndScreen(gControl.endScreen);
    }

    public void BuildEndScreen(GameObject screen)
    {
        // Stop BGM and play completion sound
        sndManager.StopBGM();
        sndManager.PlaySFX(sndManager.levelComplete);

        // One star for completion
        int starsEarned = 1;

        // Evaluate player achievements
        powerpacksFound = NumPowerpacksFound();
        robotsDestroyed = NumRobotsDestroyed();

        // Display endscreen
        Instantiate(screen);

        // Display robot/powerpack count
        robotsText = GameObject.Find("Robots Destroyed").GetComponent<Text>();
        powerpacksText = GameObject.Find("Powerpacks Found").GetComponent<Text>();

        robotsText.text = $"{robotsDestroyed}/{totalRobots}";
        powerpacksText.text = $"{powerpacksFound}/{totalPowerpacks}";

        // Display yellow star if achievement earned
        if (robotsDestroyed == totalRobots)
        {
            robotStar = GameObject.Find("Robot Star").GetComponent<Image>();
            robotStar.color = new Color32(255, 255, 255, 255);
            starsEarned++;
        }
        if (powerpacksFound == totalPowerpacks)
        {
            powerpackStar = GameObject.Find("Powerpack Star").GetComponent<Image>();
            powerpackStar.color = new Color32(255, 255, 255, 255);
            starsEarned++;
        }

        // Pass stars earned to permanent storage in gamecontroller
        gControl.SetStars(level, starsEarned);
    }

    // Called when clicking on the achievement button
    public void AchievementButton(){
        /* Due to a Unity bug, buttons which call functions that require references to scripts
        cannot be assigned in Start(), but must be re-fetched every time they require usage */
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        gControl.OpenAchievementScreen();
    }

    // Called when clicking on the levels button
    public void LevelsButton(){
        
    }

    // Called when clicking on the replay button
    public void ReplayButton(){
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        gControl.Unpause();
    }

    // Called when clicking on the next/unpause button
    public void NextButton(){
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        gControl.Unpause();
    }

    // Enter play mode rather than being in menus
    
}
