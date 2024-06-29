using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{

    private void OnEnable()
    {
        EventManager.PlayerDetected += RestartGame;
    }

    private void onDisable()
    {
        EventManager.PlayerDetected -= RestartGame;
    }
    public void RestartGame() 
    {
        Debug.Log("RESTART GAME");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    
    }
}
