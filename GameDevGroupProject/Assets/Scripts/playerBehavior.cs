using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerBehavior : MonoBehaviour
{
    private Rigidbody2D rb;

    private BoxCollider2D bc;

    private gameController gControl;

    [SerializeField]
    GameObject bullet;

    [SerializeField]
    GameObject pulse;

    [SerializeField]
    float jumpSpeed;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float fallAugmentMultiplier;

    [SerializeField]
    float fallAugmentThreshold;

    private PlayerInput playerInput;
    private InputAction playerMove;

    private LayerMask ignorePlayerMask;

    private bool facingRight;

    // Awake is called before Start but after every GameObject on the scene is instantiated
    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();

        ignorePlayerMask = LayerMask.GetMask("Structure", "Object");

        // Make a reference for the Move action from the player input
        playerInput = GetComponent<PlayerInput>();
        playerMove = playerInput.actions.FindAction("Move");

        // Make a reference for the rigidbody attached to the player
        rb = GetComponent<Rigidbody2D>();

        // Freeze the rotation of the player
        rb.freezeRotation = true;

        bc = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate(){
        // Move the player left/right using input
        rb.velocity = new Vector2(moveSpeed * playerMove.ReadValue<Vector2>().x, rb.velocity.y);
        if (rb.velocity.x > 0 && !facingRight){
            facingRight = true;
            transform.Rotate(0, 180, 0);
        } else if (rb.velocity.x < 0 && facingRight){
            facingRight = false;
            transform.Rotate(0, -180, 0);
        }

        // Increase downwards velocity linearly after a certain threshold, creating acceleration
        if (rb.velocity.y < fallAugmentThreshold){
            float fallAugment = (fallAugmentThreshold - rb.velocity.y) * fallAugmentMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - fallAugment);
        }
    }

    // Check if player is touching ground
    bool isGrounded(){
        // Since the player is a square, two raycasts are needed to determine if they are on the ground
        // One raycast from left, one from right
        Vector2 playerLeft = new Vector2(transform.position.x - 0.9f * bc.bounds.extents.x, transform.position.y);
        Vector2 playerRight = new Vector2(transform.position.x + 0.9f * bc.bounds.extents.x, transform.position.y);
        RaycastHit2D groundCheckLeft = Physics2D.Raycast(playerLeft, Vector2.down, bc.bounds.extents.y + 0.1f, ignorePlayerMask);
        RaycastHit2D groundCheckRight = Physics2D.Raycast(playerRight, Vector2.down, bc.bounds.extents.y + 0.1f, ignorePlayerMask);

        return groundCheckLeft || groundCheckRight;
    }

    // Called when Jump button is pressed
    public void OnJump()
    {
        // Only jump if the player is on the ground
        if (isGrounded() && gControl.CurrentGameState() == gameController.gameState.running)
        {
            var adjustedJumpSpeed = jumpSpeed;
            if (gControl.allowGreenPowerup){
                adjustedJumpSpeed = jumpSpeed * 1.4f;
            }
            rb.velocity = new Vector2(rb.velocity.x, adjustedJumpSpeed);
        }

    }

    // Called when Pause button is pressed
    public void OnPause()
    {
        gControl.PauseMenu();
    }

    // Called when fire bullet ability button is pressed

    public void OnFireBullet(){
        if (gControl.CurrentGameState() == gameController.gameState.running){
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }

    // Called when fire pulse ability button is pressed
    public void OnFirePulse()
    {
        // Spawn Blue pulses above and below the player
        if(gControl.CurrentGameState() == gameController.gameState.running)
        {
            Instantiate(pulse, gameObject.transform.position + Vector3.up * transform.localScale.y, gameObject.transform.rotation);
            Instantiate(pulse, gameObject.transform.position + Vector3.down * transform.localScale.y, gameObject.transform.rotation);
        }

    }
}


