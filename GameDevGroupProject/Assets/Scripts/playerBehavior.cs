using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerBehavior : MonoBehaviour
{
    Rigidbody2D rb;

    eventManager eManager;

    public GameObject pulse;

    [SerializeField] float moveForce;

    [SerializeField] float fallAugmentMultiplier;

    [SerializeField] float fallAugmentThreshold;

    private PlayerInput playerInput;
    private InputAction playerMove;


    //Awake is called before Start but after every GameObject on the scene is instantiated
    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        eManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<eventManager>();

        // Make a reference for the Move action from the player input
        playerInput = GetComponent<PlayerInput>();
        playerMove = playerInput.actions.FindAction("Move");

        // Make a reference for the rigidbody attached to the player
        rb = GetComponent<Rigidbody2D>();

        // Freeze the rotation of the player
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (eManager.CurrentGameState() == eventManager.gameState.pause)
        {
            return;
        }

        // Move the player left/right using input
        rb.velocity = new Vector2(moveForce * playerMove.ReadValue<Vector2>().x, rb.velocity.y);

        // Increase downwards velocity linearly after a certain threshold, creating acceleration
        if (rb.velocity.y < fallAugmentThreshold)
        {
            float fallAugment = (fallAugmentThreshold - rb.velocity.y) * fallAugmentMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - fallAugment);
        }
    }

    //Check if player is touching ground

    //Called when Jump button is pressed
    void OnJump()
    {
        // Only jump if the player is on the ground
        // if (isGrounded() && eManager.CurrentGameState() == eventManager.gameState.running)
        // {   
        //     rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        // }
    }

    //Called when Pause button is pressed
    void OnPause()
    {
        eManager.PauseMenu();
    }


    //Called when fire pulse ability button is pressed
    void OnFirePulse()
    {
        //Spawn Blue pulses above and below the player
        if (eManager.CurrentGameState() == eventManager.gameState.running)
        {
            Instantiate(pulse, gameObject.transform.position + Vector3.up * transform.localScale.y,
                gameObject.transform.rotation);
            Instantiate(pulse, gameObject.transform.position + Vector3.down * transform.localScale.y,
                gameObject.transform.rotation);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision happened");
    }
}