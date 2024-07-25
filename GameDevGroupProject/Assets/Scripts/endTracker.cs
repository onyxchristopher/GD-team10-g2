using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endTracker : MonoBehaviour
{

    [SerializeField] private GameObject endScreen;

    private int powerpacksFound = 0;
    private int robotsDestroyed = 0;
    private int totalPowerpacks;
    private int totalRobots;

    private Text robotsText;
    private Text powerpacksText;
    private Image robotStar;
    private Image powerpackStar;

    void Start()
    {
        totalPowerpacks = GameObject.FindGameObjectsWithTag("Powerpack").Length;
        totalRobots = GameObject.FindGameObjectsWithTag("Robot").Length;
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
        // Evaluate player achievements
        powerpacksFound = NumPowerpacksFound();
        robotsDestroyed = NumRobotsDestroyed();
        
        // Display endscreen
        Instantiate(endScreen);
        
        // Display robot/powerpack count
        robotsText = GameObject.Find("Robots Destroyed").GetComponent<Text>();
        powerpacksText = GameObject.Find("Powerpacks Found").GetComponent<Text>();

        robotsText.text = $"{robotsDestroyed}/{totalRobots}";
        powerpacksText.text = $"{powerpacksFound}/{totalPowerpacks}";

        // Display yellow star if achievement earned
        if (robotsDestroyed == totalRobots){
            robotStar = GameObject.Find("Robot Star").GetComponent<Image>();
            robotStar.color = new Color32(255, 255, 255, 255);
        }
        if (powerpacksFound == totalPowerpacks){
            powerpackStar = GameObject.Find("Powerpack Star").GetComponent<Image>();
            powerpackStar.color = new Color32(255, 255, 255, 255);
        }
    }
}
