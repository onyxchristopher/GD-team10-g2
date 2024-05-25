using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    Rigidbody2D rb;

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

    void FixedUpdate(){
        // Move the player left/right using input
        rb.velocity = new Vector2(moveForce * playerMove.ReadValue<Vector2>().x, rb.velocity.y);

        // Increase downwards velocity linearly after a certain threshold, creating acceleration
        if (rb.velocity.y < fallAugmentThreshold){
            float fallAugment = (fallAugmentThreshold - rb.velocity.y) * fallAugmentMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - fallAugment);
        }
    }

    void OnJump(){
        // Only jump if the player is on the ground
        if (isGrounded()){
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        
    }

    bool isGrounded(){
        // The player is on the ground if a short ray emitted downwards from the player's lower edge hits the ground
        // For a different shape (i.e. square) two rays will be needed and if either one collides the player is grounded
        Vector2 playerEdge = new Vector2(transform.position.x, transform.position.y) + (Vector2.down / 2);
        RaycastHit2D groundCheck = Physics2D.Raycast(playerEdge, Vector2.down, 0.1f);

        return groundCheck.collider != null;
    }
}
