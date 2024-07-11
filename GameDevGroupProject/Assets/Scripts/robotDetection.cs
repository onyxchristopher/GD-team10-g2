using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.PlayerLoop;
using UnityEngine.InputSystem.XR.Haptics;
using JetBrains.Annotations;

public class robotDetection : MonoBehaviour
{
    //Variables for drawing the fov
    [SerializeField]
    float fov = 25f;
    /*[SerializeField]
    bool moving = false;*/
    [SerializeField]
    public bool facingRight = true; // if false, then facing left
    [SerializeField]
    int numRaycasts = 2;

    Mesh mesh;

    private float xDir;
    private float yDir;

    private float increments;

    private int triIndex;

    private Vector2 currentPos;

    private LayerMask ignoreRobotMask;

    private PolygonCollider2D [] allFOVs;
    private PolygonCollider2D pc;

    void OnEnable(){
        /*
        ignoreRobotMask = LayerMask.GetMask("Structure");

        Mesh mesh = new Mesh();
        mesh.name = "robotFOV";
        GetComponent<MeshFilter>().mesh = mesh;

        PolygonCollider2D pc = gameObject.AddComponent<PolygonCollider2D>();
        pc.isTrigger = true;
        Vector2[] points = new Vector2[numRaycasts + 1];

        setRobotPos();

        Vector3[] vertices = new Vector3[points.Length];
        Vector2[] uvs = new Vector2[points.Length];
        int[] triangles = new int[3 * (numRaycasts - 1)];

        points[0] = Vector2.zero;
        vertices[0] = Vector3.zero;

        int xDir = 1;
        if (!facingRight){
            xDir = -1;
            fov = -fov;
        }

        float increments = fov/(numRaycasts - 1);

        int triIndex = 0;

        for (int i = 1; i <= numRaycasts; i++){
            float yDir = Mathf.Tan(((fov / 2f) - increments * (i - 1)) * Mathf.PI / 180f);
            Vector2 directionVector = new Vector2(xDir, yDir);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(currentPos, directionVector, 100f, ignoreRobotMask);
            points[i] = raycastHit2D.point - currentPos;
            vertices[i] = points[i];

            if (i < numRaycasts){
                triangles[triIndex] = 0;
                triangles[triIndex + 1] = i;
                triangles[triIndex + 2] = i + 1;
                triIndex += 3;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        pc.SetPath(0, points);*/
    }


    void Start()
    {

        ignoreRobotMask = LayerMask.GetMask("Structure");

        xDir = 1;
        if (!facingRight)
        {
            xDir = -1;
            fov = -fov;
        }

        increments = fov / (numRaycasts - 1);

    }

    private void Update()
    {
        Debug.Log("robotDetection currentPos: " + currentPos);
        if(Time.frameCount % 5 == 0)
        {
            //Check if any other polygon collider is currently on scene
            
            allFOVs = GetComponents<PolygonCollider2D>();

            if(allFOVs != null & allFOVs.Length > 1 )
            {
                Destroy(allFOVs[0]);
            }

            //Create new mesh for redrawing
            mesh = new Mesh();
            mesh.name = "robotFOV";
            GetComponent<MeshFilter>().mesh = mesh;

            //Create new polygcon collider for nesh
            pc = gameObject.AddComponent<PolygonCollider2D>();
            pc.isTrigger = true;

            Vector2[] points = new Vector2[numRaycasts + 1];

            setRobotPos();

            Vector3[] vertices = new Vector3[points.Length];
            Vector2[] uvs = new Vector2[points.Length];
            int[] triangles = new int[3 * (numRaycasts - 1)];

            points[0] = Vector2.zero;
            vertices[0] = Vector3.zero;

            //Set triIndex to 0 beforee loop
            triIndex = 0;
            for (int i = 1; i <= numRaycasts; i++)
            {
                yDir = Mathf.Tan(((fov / 2f) - increments * (i - 1)) * Mathf.PI / 180f);
                Vector2 directionVector = new Vector2(xDir, yDir);
                RaycastHit2D raycastHit2D = Physics2D.Raycast(currentPos, directionVector, 100f, ignoreRobotMask);
                points[i] = raycastHit2D.point - currentPos;
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
    }

    private void setRobotPos(){
        currentPos = new Vector2(transform.position.x, transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other){
        Debug.Log("Player detected");
    }
}
