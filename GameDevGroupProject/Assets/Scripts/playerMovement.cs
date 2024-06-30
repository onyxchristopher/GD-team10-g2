using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    private BoxCollider2D bc;

    [SerializeField] float jumpSpeed;

    [SerializeField] public float moveSpeed;

    [SerializeField] float fallAugmentMultiplier;

    [SerializeField] float fallAugmentThreshold;

    public eventManager eventManager;

    private PlayerInput playerInput;

    private InputAction playerMove;

    private playerBehavior playerBehavior;
    private playerPowerupsHandler playerPowerupsHandler;

    // Start is called before the first frame update
    void Start()
    {
        // Make a reference for the Move action from the player input
        playerInput = GetComponent<PlayerInput>();
        playerBehavior = GetComponent<playerBehavior>();
        playerPowerupsHandler = GetComponent<playerPowerupsHandler>();

        playerMove = playerInput.actions.FindAction("Move");

        // Make a reference for the rigidbody attached to the player
        rb = GetComponent<Rigidbody2D>();

        // Freeze the rotation of the player
        rb.freezeRotation = true;

        bc = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        // Move the player left/right using input
        rb.velocity = new Vector2(moveSpeed * playerMove.ReadValue<Vector2>().x, rb.velocity.y);

        var greenPowerUp =
            playerPowerupsHandler
                .powerUpsActive
                .FirstOrDefault(x => x.powerUpType == PowerUpType.Green);


        // Increase downwards velocity linearly after a certain threshold, creating acceleration
        if (rb.velocity.y < fallAugmentThreshold)
        {
            float fallAugment = (fallAugmentThreshold - rb.velocity.y) * fallAugmentMultiplier;

            float multiplier = 1;

            if (greenPowerUp)
            {
                multiplier = greenPowerUp.playerSpeedMultiplier;
            }

            fallAugment = fallAugment * multiplier;
            rb.velocity = new Vector2(rb.velocity.x * multiplier, rb.velocity.y - fallAugment);
        }
    }

    void OnSkill()
    {
    }

    void OnJump()
    {
        if (isGrounded())
        {
            var realJumpSpeed = jumpSpeed;

            var greenPowerUp =
                playerPowerupsHandler
                    .powerUpsActive
                    .FirstOrDefault(x => x.powerUpType == PowerUpType.Green);

            if (greenPowerUp != null)
            {
                realJumpSpeed = realJumpSpeed * greenPowerUp.playerJumpMultiplier;
            }

            rb.velocity = new Vector2(rb.velocity.x, realJumpSpeed);
        }
    }

    bool isGrounded()
    {
        // Since the player is a square, two raycasts are needed to determine if they are on the ground
        // One raycast from left, one from right
        Vector2 playerLeft = new Vector2(transform.position.x - bc.bounds.extents.x, transform.position.y);
        Vector2 playerRight = new Vector2(transform.position.x + bc.bounds.extents.x, transform.position.y);
        RaycastHit2D groundCheckLeft = Physics2D.Raycast(playerLeft, Vector2.down, bc.bounds.extents.y + 0.1f);
        RaycastHit2D groundCheckRight = Physics2D.Raycast(playerRight, Vector2.down, bc.bounds.extents.y + 0.1f);

        return groundCheckLeft || groundCheckRight;
    }
}