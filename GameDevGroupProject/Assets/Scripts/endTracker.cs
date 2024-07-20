using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endTracker : MonoBehaviour
{
    private int powerpacksFound = 0;
    private int robotsDestroyed = 0;
    private int totalPowerpacks;
    private int totalRobots;

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
        Debug.Log($"{powerpacksFound}/{totalPowerpacks} powerpacks found");
        Debug.Log($"{robotsDestroyed}/{totalRobots} robots destroyed");
        
        // Display endscreen
    }
}
