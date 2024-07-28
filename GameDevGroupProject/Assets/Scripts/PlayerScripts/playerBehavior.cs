using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerBehavior : MonoBehaviour
{
    private Rigidbody2D rb;

    private BoxCollider2D bc;


    // Managers references
    private gameController gControl;
    private soundManager sndManager;
    private sceneManager scnManager;
    private endTracker endTrack;

    [SerializeField] GameObject bullet;

    [SerializeField] GameObject pulseUp;
    [SerializeField] GameObject pulseDown;

    [SerializeField] float jumpSpeed = 19f;

    [SerializeField] float moveSpeed = 10f;

    [SerializeField] float fallAugmentMultiplier = 0.05f;

    [SerializeField] float fallAugmentThreshold = 3f;

    [SerializeField] float fallAugmentMax = -40f;

    private PlayerInput playerInput;
    private InputAction playerMove;

    private LayerMask ignorePlayerMask;

    private bool facingRight = true;

    // this is the power ups active on this player
    public List<powerUp> powerUpsActive = new List<powerUp>();

    private Vector3 fireOffset = new Vector3(0, 0.5f, 0);

    public void ClearPowerups()
    {
        powerUpsActive.Clear();
    }

    public bool AddPowerUp(powerUp powerUp)
    {
        powerUpsActive.Clear();

        // make sure no duplicate powerups
        if (powerUpsActive.All(x => x.powerUpType != powerUp.powerUpType))
        {
            powerUpsActive.Add(powerUp);

            return true;
        }

        return false;
    }

    //Tps player
    public void MovePlayer(Transform location)
    {
        transform.position = location.position;
    }

    // Awake is called before Start but after every GameObject on the scene is instantiated
    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        scnManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<sceneManager>();
        endTrack = GameObject.FindGameObjectWithTag("Exit Door").GetComponent<endTracker>();

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

    void FixedUpdate()
    {
        // Move the player left/right using input
        var xVelocity = playerMove.ReadValue<Vector2>().x;
        rb.velocity = new Vector2(moveSpeed * xVelocity, rb.velocity.y);

        // Rotate the player when they change direction
        if (rb.velocity.x > 0 && !facingRight)
        {
            facingRight = true;
            transform.Rotate(0, 180, 0);
        }
        else if (rb.velocity.x < 0 && facingRight)
        {
            facingRight = false;
            transform.Rotate(0, -180, 0);
        }

        // Increase downwards velocity linearly after a certain threshold, creating acceleration
        if (rb.velocity.y < fallAugmentThreshold && rb.velocity.y > fallAugmentMax)
        {
            float fallAugment = (fallAugmentThreshold - rb.velocity.y) * fallAugmentMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - fallAugment);
        }
    }

    // Check if player is touching ground
    bool isGrounded()
    {
        // Three raycasts are needed to determine if the player they are on the ground
        // One from left, one from center, one from right

        // Calculate player left, center, right
        Vector2 playerLeft = new Vector2(transform.position.x - 0.9f * bc.bounds.extents.x, transform.position.y);
        Vector2 playerCenter = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerRight = new Vector2(transform.position.x + 0.9f * bc.bounds.extents.x, transform.position.y);
        
        // Perform three raycasts
        RaycastHit2D groundCheckLeft = Physics2D.Raycast(playerLeft,
        Vector2.down, bc.bounds.extents.y + 0.05f, ignorePlayerMask);
        RaycastHit2D groundCheckCenter = Physics2D.Raycast(playerCenter,
        Vector2.down, bc.bounds.extents.y + 0.1f, ignorePlayerMask);
        RaycastHit2D groundCheckRight = Physics2D.Raycast(playerRight,
        Vector2.down, bc.bounds.extents.y + 0.05f, ignorePlayerMask);

        return (rb.velocity.y < 3) && (groundCheckLeft || groundCheckCenter || groundCheckRight);
    }

    private bool IsPowerUpActive(PowerUpType powerUpType)
    {
        foreach (var powerUp in powerUpsActive)
        {
            if (powerUp.powerUpType == powerUpType)
                return true;
        }

        return false;
    }

    //++++++++++++++++++++Control scheme related++++++++++++++++++++++++++

    // Called when Jump button is pressed
    public void OnJump()
    {
        // Only jump if the player is on the ground
        if (isGrounded() && gControl.CurrentGameState() == gameController.gameState.running)
        {

            sndManager.PlaySFX(sndManager.characterJump, 0.25f);

            var adjustedJumpSpeed = jumpSpeed;

            if (IsPowerUpActive(PowerUpType.Green))
            {
                adjustedJumpSpeed *= 1.4f;
            }

            rb.velocity = new Vector2(rb.velocity.x, adjustedJumpSpeed);
        }
    }

    // Called when Pause button is pressed
    public void OnPause()
    {
        Debug.Log("Called onpause");
        gControl.PauseMenu();
    }

    // Called when fire bullet ability button is pressed

    public void OnFireBullet()
    {
        if (gControl.CurrentGameState() == gameController.gameState.running && IsPowerUpActive(PowerUpType.Red))
        {
            sndManager.PlaySFX(sndManager.powerUpFireRed);
            Instantiate(bullet, transform.position + fireOffset, transform.rotation);
        }
    }

    // Called when fire pulse ability button is pressed
    public void OnFirePulse()
    {
        // Spawn Blue pulses above and below the player
        if (gControl.CurrentGameState() == gameController.gameState.running && IsPowerUpActive(PowerUpType.Blue))
        {
            sndManager.PlaySFX(sndManager.powerUpFirePulse, 0.5f);

            Instantiate(pulseUp,
                gameObject.transform.position + Vector3.up * 6 + fireOffset,
                gameObject.transform.rotation);
            Instantiate(pulseDown,
                gameObject.transform.position + Vector3.down * 6 + fireOffset,
                gameObject.transform.rotation);
        }
    }

    public void OnRestart()
    {
        var restartCanvas = GameObject.FindGameObjectWithTag("Respawn");
        if (restartCanvas)
        {
            restartCanvas.GetComponent<fadeCanvas>().destroyCanvas();
        }
        endTrack.ResetAll();
        scnManager.RestartLevel();
    }

}