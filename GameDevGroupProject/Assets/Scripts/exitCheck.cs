using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitCheck : MonoBehaviour
{
    [SerializeField] 
    private string nextLevel;

    [SerializeField] 
    private string prevLevel;

    private sceneManager sManager;
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        sManager = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<sceneManager>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //If player reached exit, call sceneManager to load next level
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Player reached exit");

            sManager.NextLevel(nextLevel);
            sManager.UnloadScene(prevLevel);
        }
    }

}
