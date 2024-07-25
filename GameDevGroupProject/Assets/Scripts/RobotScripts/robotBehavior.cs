using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class robotBehavior : MonoBehaviour
{

    //Fields to tweak enemy movement range, speed and time still on Unity inspector
    [SerializeField]
    public float movSpeed;
    [SerializeField]
    public float movRange;
    [SerializeField]
    public float patrolPauseTime;

    //References to game controller and sound manager
    private soundManager sndManager;
    private gameController gController;
    private sceneManager sManager;

    //If positive looking right, negative looking left
    private int faceDir = 1;

    //Variables to keep start and current position of enemy to control movement
    private Vector2 startPosition;
    private Vector2 currentPosition;

    //EPOCH - Variables to control pause in patrol movement
    private long timeStamp = 0;
    private System.DateTime epochStart;

    //State Machine
    public enum states { MOVING, STOPPED };
    private states myState;
    private Dictionary<states, Action> stateLogic = new Dictionary<states, Action>();

    //Variables for drawing the fov
    [SerializeField] float fov = 25f;

    [SerializeField] int numRaycasts = 2;

    Mesh mesh;

    private float yDir;

    private float increments;

    private int triIndex;

    private LayerMask ignoreRobotMask;

    private PolygonCollider2D[] allFOVs;
    private PolygonCollider2D pc;

    private SpriteRenderer childSprite;

    [SerializeField] GameObject restartScreen;

    void Awake()
    {
       
    }

    void Start()
    {
        //Setting up manager references
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        gController = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        sManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<sceneManager>();

        //Setting up intial state and SM logic
        myState = states.MOVING;
        stateLogic.Add(states.MOVING, MoveEnemy);
        stateLogic.Add(states.STOPPED, StopEnemy);

        //Variable to control time robot stays still
        epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

        startPosition = transform.position;

        //Parameters for drawing the robot FOV

        ignoreRobotMask = LayerMask.GetMask("Structure");

        if (faceDir == -1)
        {
            fov = -fov;
        }

        increments = fov / (numRaycasts - 1);

        childSprite = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPosition = new Vector2 (transform.position.x, transform.position.y);

        Action toExecute = null;

        //Checks for current enemy state and executes corresponding function
        if (stateLogic.TryGetValue(myState, out toExecute))
        {
            toExecute();
        }

        //Draws fov mesh
        if (Time.frameCount % 4 == 0)
        {
            drawFOV();
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
        }
    }

    private void StopEnemy()
    {
        int currentTimeStamp = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;

        if (currentTimeStamp - timeStamp > patrolPauseTime)
        {
            UpdateFaceDir();
            myState = states.MOVING;
        }
    }

    public states GetCurrentState()
    {
        return myState;
    }
    private void drawFOV()
    {
        //Check if any other polygon collider is currently on scene
        allFOVs = GetComponents<PolygonCollider2D>();

        if (allFOVs != null & allFOVs.Length > 1)
        {
            Destroy(allFOVs[0]);
        }

        //Create new mesh for redrawing
        mesh = new Mesh();
        mesh.name = "robotFOV";
        GetComponent<MeshFilter>().mesh = mesh;

        //Create new polygcon collider for mesh
        pc = gameObject.AddComponent<PolygonCollider2D>();
        pc.isTrigger = true;

        Vector2[] points = new Vector2[numRaycasts + 1];

        Vector3[] vertices = new Vector3[points.Length];
        Vector2[] uvs = new Vector2[points.Length];
        int[] triangles = new int[3 * (numRaycasts - 1)];

        points[0] = Vector2.zero;
        vertices[0] = Vector3.zero;

        //Set triIndex to 0 before loop
        triIndex = 0;
        for (int i = 1; i <= numRaycasts; i++)
        {
            yDir = Mathf.Tan(((fov / 2f) - increments * (i - 1)) * Mathf.PI / 180f);
            Vector2 directionVector = new Vector2(faceDir, yDir);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(currentPosition, directionVector, 100f, ignoreRobotMask);
            points[i] = raycastHit2D.point - currentPosition;
            vertices[i] = points[i];

            if (i < numRaycasts)
            {
                triangles[triIndex] = 0;
                triangles[triIndex + 1] = i;
                triangles[triIndex + 2] = i + 1;
                triIndex += 3;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        pc.SetPath(0, points);

    }

    //Update robot face direction and mesh direction
    private void UpdateFaceDir()
    {
        faceDir = -faceDir;
        fov = -fov;
        increments = -increments;
        childSprite.flipX = !childSprite.flipX;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Detected(other);
    }

    public void Detected(Collider2D player)
    {
        if (!GameObject.FindGameObjectWithTag("Respawn")){
            Instantiate(restartScreen);
            sndManager.PlaySFX(sndManager.playerDetected);
            sndManager.StopBGM();
            sManager.RestartLevel();
        }
    }
}
