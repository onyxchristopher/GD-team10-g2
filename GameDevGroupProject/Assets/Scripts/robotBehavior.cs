using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class robotBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    //Fields to tweak enemy movement range, speed and time still on Unity inspector
    [SerializeField]
    public float movSpeed;
    [SerializeField]
    public float movRange;
    [SerializeField]
    public float patrolPauseTime;

    private int faceDir = 1;

    //Variables to keep start and current position of enemy to control movement
    private Vector2 startPosition;
    private Vector2 currentPosition;

    //EPOCH - Variables to control pause in patrol movement
    private long timeStamp = 0;
    private System.DateTime epochStart;

    //State Machine
    private enum states { MOVING, STOPPED };
    private states myState;
    private Dictionary<states, Action> stateLogic = new Dictionary<states, Action>();

    void Start()
    {
        //Setting up intial state and SM logic
        myState = states.MOVING;
        stateLogic.Add(states.MOVING, MoveEnemy);
        stateLogic.Add(states.STOPPED, StopEnemy);

        //Variable to control time robot stays still
        epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;

        Action toExecute = null;

        //Checks for current enemy state and executes corresponding function
        if (stateLogic.TryGetValue(myState, out toExecute))
        {
            toExecute();
        }
    }

    private void MoveEnemy()
    {
        this.transform.Translate(new Vector2(faceDir, 0) * movSpeed * Time.deltaTime);

        if ((startPosition - currentPosition).sqrMagnitude > movRange * movRange)
        {
            timeStamp = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
            myState = states.STOPPED;
            currentPosition = new Vector2(faceDir, 0) * movRange;
            transform.position = startPosition + currentPosition;
            faceDir = -faceDir;
        }
    }

    private void StopEnemy()
    {
        int currentTimeStamp = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;

        if (currentTimeStamp - timeStamp > patrolPauseTime)
        {
            myState = states.MOVING;
        }
    }
}
