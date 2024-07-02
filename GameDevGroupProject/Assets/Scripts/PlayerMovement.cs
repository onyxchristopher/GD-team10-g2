using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    private PlayerPowerupsHandler playerPowerupsHandler;

    [SerializeField] public float jumpSpeed = 10f;
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float fallAugmentMultiplier = 2.5f;
    [SerializeField] public float lowJumpMultiplier = 2f;

    public PlayerInput playerInput;
    public InputAction playerMove;
    public InputAction playerJump;
    public Vector2 moveInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMove = playerInput.actions["Move"];
        playerJump = playerInput.actions["Jump"];

        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();

        playerPowerupsHandler = GetComponent<PlayerPowerupsHandler>();

        groundLayerMask = LayerMask.GetMask("LevelObstacles", "LevelBorders");
    }

    private void OnEnable()
    {
        playerJump.started += OnTriggerJump;
    }

    private void OnDisable()
    {
        playerJump.started -= OnTriggerJump;
    }

    public Vector2 FacingDirection { get; private set; } = Vector2.right;

    private void FixedUpdate()
    {
        // Get the move input
        moveInput = playerMove.ReadValue<Vector2>();

        if (moveInput.x > 0)
        {
            FacingDirection = Vector2.right;
        }
        else if (moveInput.x < 0)
        {
            FacingDirection = Vector2.left;
        }

        var greenPowerUp =
            playerPowerupsHandler
                .powerUpsActive
                .FirstOrDefault(x => x.powerUpType == PowerUpType.Green);

        if (greenPowerUp)
        {
            moveInput *= greenPowerUp.playerSpeedMultiplier;
        }


        // Smooth horizontal movement
        float targetVelocityX = moveInput.x * moveSpeed;

        rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);

        // Apply better fall mechanics
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallAugmentMultiplier - 1) * Time.fixedDeltaTime);
        }
        else if (rb.velocity.y > 0 && playerJump.ReadValue<float>() == 0)
        {
            rb.velocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            var multiplier = 1;

            var greenPowerUp =
                playerPowerupsHandler
                    .powerUpsActive
                    .FirstOrDefault(x => x.powerUpType == PowerUpType.Green);

            if (greenPowerUp)
            {
                multiplier *= greenPowerUp.playerJumpMultiplier;
            }

            rb.AddForce(Vector2.up * jumpSpeed * multiplier, ForceMode2D.Impulse);
            //     rb.velocity = new Vector2(rb.velocity.x, jumpSpeed * multiplier, );
        }
    }

    private LayerMask groundLayerMask;

    private bool IsGrounded()
    {
        Debug.Log("hi");
        Vector2 boxCenter = new Vector2(transform.position.x, transform.position.y - bc.bounds.extents.y - 0.1f);
        Vector2 boxSize = new Vector2(bc.bounds.size.x - 0.2f, 0.1f);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, groundLayerMask);

        return colliders.Length > 0;
    }
}