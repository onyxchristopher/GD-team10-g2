using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitCheck : MonoBehaviour
{
    [SerializeField] 
    string nextLevel;

    [SerializeField] 
    string prevLevel;

    private sceneManager scnManager;
    private GameObject player;
    private soundManager sndManager;


    // Start is called before the first frame update
    void Start()
    {
        scnManager = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<sceneManager>();
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponentInChildren<soundManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //If player reached exit, call sceneManager to load next level
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == player)
        {
            sndManager.PlaySFX(sndManager.levelComplete);
            // sndManager.StopBGM();
            //scnManager.NextLevel(nextLevel);
            //scnManager.UnloadScene(prevLevel);
        }
    }

}
