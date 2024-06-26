﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerBehavior : MonoBehaviour
{
    Rigidbody2D rb;

    public GameObject pulse;

    public Transform shootingPoint;
    public GameObject bulletPrefab;


    [SerializeField]
    float jumpForce;

    [SerializeField]
    float moveForce;

    [SerializeField]
    float fallAugmentMultiplier;

    [SerializeField]
    float fallAugmentThreshold;

    private PlayerInput playerInput;
    private InputAction playerMove;
    
    // Start is called before the first frame update
    void Start()
    {
        // Make a reference for the Move action from the player input
        playerInput = GetComponent<PlayerInput>();
        playerMove = playerInput.actions.FindAction("Move");

        // Make a reference for the rigidbody attached to the player
        rb = GetComponent<Rigidbody2D>();

        // Freeze the rotation of the player
        rb.freezeRotation = true;
    }

     void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
        }
    }
    void FixedUpdate(){
        // Move the player left/right using input
        rb.velocity = new Vector2(moveForce * playerMove.ReadValue<Vector2>().x, rb.velocity.y);

        // Increase downwards velocity linearly after a certain threshold, creating acceleration
        if (rb.velocity.y < fallAugmentThreshold){
            float fallAugment = (fallAugmentThreshold - rb.velocity.y) * fallAugmentMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - fallAugment);
        }
        
    }

    //Player Jump
    void OnJump(){
        // Only jump if the player is on the ground
        if (isGrounded()){
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        
    }
    void Flip()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            transform.Rotate(0, 180, 0);
        }
        else
        {
            transform.Rotate(0, 180, 0);
        }

    }
    //Check if player is touching ground
    bool isGrounded(){
        // Since the player is a square, two raycasts are needed to determine if they are on the ground
        // One raycast from lower left, one from lower right
        Vector2 playerCenter = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerLowerLeft = playerCenter + transform.localScale.x * Vector2.left + transform.localScale.y * Vector2.down;
        Vector2 playerLowerRight = playerCenter + transform.localScale.x * Vector2.right + transform.localScale.y * Vector2.down;
        RaycastHit2D groundCheckLeft = Physics2D.Raycast(playerLowerLeft, Vector2.down, 0.1f);
        RaycastHit2D groundCheckRight = Physics2D.Raycast(playerLowerRight, Vector2.down, 0.1f);

        return ((groundCheckLeft.collider != null) || (groundCheckRight.collider != null));
    }

    //Blue ability spawn
    void OnFirePulse()
    {
        //Spawn Blue pulses above and below the player
        Instantiate(pulse, gameObject.transform.position + Vector3.up * transform.localScale.y, gameObject.transform.rotation);
        Instantiate(pulse, gameObject.transform.position + Vector3.down * transform.localScale.y, gameObject.transform.rotation);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision happened");

        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Collision with an enemy");
        }
    }

}


