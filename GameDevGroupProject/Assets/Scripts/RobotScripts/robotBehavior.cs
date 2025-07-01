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

    //Variable to indicate detection
    private bool detection;


    //Variables for drawing the fov
    [SerializeField] float fov = 25f;

    private int numRaycasts = 2;

    Mesh mesh;

    private float yDir;

    private float increments;

    private int triIndex;

    private LayerMask ignoreRobotMask;

    private PolygonCollider2D[] allFOVs;
    private PolygonCollider2D pc;

    private SpriteRenderer childSprite;

    //Reference to restart screen
    [SerializeField] GameObject restartScreen;
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

        //Setting detection variable on instance
        detection = false;

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

        //Draws fov mesh every 2 frames to reduce lag and keep smooth update
        if (Time.frameCount % 2 == 0)
        {
            drawFOV();
        }
    }

    //State machine functions
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

    // draw robot fov
    private void drawFOV()
    {
        // check if any other polygon collider is currently on scene
        allFOVs = GetComponents<PolygonCollider2D>();

        if (allFOVs != null & allFOVs.Length > 1)
        {
            Destroy(allFOVs[0]);
        }

        // create new mesh for redrawing
        mesh = new Mesh();
        mesh.name = "robotFOV";
        GetComponent<MeshFilter>().mesh = mesh;

        // create new polygcon collider for mesh
        pc = gameObject.AddComponent<PolygonCollider2D>();
        pc.isTrigger = true;

        Vector2[] points = new Vector2[numRaycasts + 1]; // define polygon points array
        Vector3[] vertices = new Vector3[points.Length]; // mesh vertices array
        Vector2[] uvs = new Vector2[points.Length]; // mesh uvs array
        int[] triangles = new int[3 * (numRaycasts - 1)]; // mesh triangle indices array

        points[0] = Vector2.zero;
        vertices[0] = Vector3.zero;

        triIndex = 0;
        // multiple raycasts evenly spread over FOV
        for (int i = 1; i <= numRaycasts; i++) {
            // y-component of raycast direction
            yDir = Mathf.Tan(((fov / 2f) - increments * (i - 1)) * Mathf.PI / 180f);
            Vector2 directionVector = new Vector2(faceDir, yDir); // direction of raycast

            // raycast will always hit a surface
            RaycastHit2D raycastHit2D = Physics2D.Raycast(currentPosition,
                directionVector, 100f, ignoreRobotMask);

            // copy raycast hit point to become a mesh vertex
            points[i] = raycastHit2D.point - currentPosition;
            vertices[i] = points[i];

            // set triangle indices
            if (i < numRaycasts) {
                triangles[triIndex] = 0;
                triangles[triIndex + 1] = i;
                triangles[triIndex + 2] = i + 1;
                triIndex += 3;
            }
        }

        // draw mesh with obtained vertices
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

    //Detects collision with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !detection){
            Detected();
            detection = true;

        }
    }

    //Detection of player if colliding with robot or fovmesh
    public void Detected()
    {
        if (!GameObject.FindGameObjectWithTag("Respawn"))
        {
            sndManager.PlaySFX(sndManager.playerDetected);
            gController.Pause();
            StartCoroutine(DetectionDelay());
        }
    }

    //Coroutine to add delay upon detection
    private IEnumerator DetectionDelay()
    {
        // Disable the gameplay actionmap on detection
        playerBehavior pBehavior = GameObject.FindWithTag("Player").GetComponent<playerBehavior>();
        pBehavior.playerInput.actions.FindActionMap("Gameplay").Disable();

        yield return new WaitForSecondsRealtime(0.3f);
        gController.Unpause();
        Instantiate(restartScreen);
        sndManager.StopBGM();
        sManager.RestartLevel();
        pBehavior.ResetVelocity();

        // Enable the restart actionmap when the restart screen is instantiated
        pBehavior.playerInput.actions.FindActionMap("RestartMap").Enable();
    }
}
