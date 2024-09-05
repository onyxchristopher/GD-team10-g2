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

    [SerializeField] GameObject bullet;

    [SerializeField] GameObject pulseUp;
    [SerializeField] GameObject pulseDown;

    [SerializeField] float jumpSpeed = 23.5f;

    [SerializeField] float moveSpeed = 10f;

    [SerializeField] float fallAugmentMultiplier = 0.06f;

    [SerializeField] float fallAugmentThreshold = 0;

    private PlayerInput playerInput;
    private InputAction playerMove;

    private LayerMask ignorePlayerMask;

    private bool facingRight = true;

    // A list of the power ups active on this player
    [HideInInspector] public List<powerUp> powerUpsActive = new List<powerUp>();

    private Vector3 fireOffset = new Vector3(0, 0.5f, 0);

    // Particle system references
    private ParticleSystem redParticles;
    private ParticleSystem greenParticles;
    private ParticleSystem blueParticles;

    private bool isFastFalling = false;
    private InputAction playerVerticalMove;
    [SerializeField] float fastFallMultiplier = 1.5f;
    private Animator anim;

    [SerializeField] float maxFallSpeed = 60f;
    [SerializeField] float fastFallSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
        scnManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<sceneManager>();

        ignorePlayerMask = LayerMask.GetMask("Structure", "Object");

        // Make a reference for the Move action from the player input
        playerInput = GetComponent<PlayerInput>();
        playerMove = playerInput.actions.FindAction("Move");

        // Make a reference for the rigidbody attached to the player
        rb = GetComponent<Rigidbody2D>();

        // Freeze the rotation of the player
        rb.freezeRotation = true;

        // Get player box collider
        bc = GetComponent<BoxCollider2D>();

        redParticles = GameObject.Find("Red Particles").GetComponent<ParticleSystem>();
        greenParticles = GameObject.Find("Green Particles").GetComponent<ParticleSystem>();
        blueParticles = GameObject.Find("Blue Particles").GetComponent<ParticleSystem>();

        anim = gameObject.GetComponentInChildren<Animator>();
        playerVerticalMove = playerInput.actions.FindAction("Move");
    }

    // Clears active powerup
    public void ClearPowerups()
    {
        powerUpsActive.Clear();
        RemovePowerupParticles();
    }

    // Removes active powerup particles from the player
    private void RemovePowerupParticles()
    {
        if (redParticles.isPlaying)
        {
            redParticles.Stop();
        }
        else if (greenParticles.isPlaying)
        {
            greenParticles.Stop();
        }
        else if (blueParticles.isPlaying)
        {
            blueParticles.Stop();
        }
    }

    // Makes a powerup usable for the player and adds particle indicators
    public bool AddPowerUp(powerUp powerUp)
    {
        powerUpsActive.Clear();

        RemovePowerupParticles();

        if (powerUp.powerUpType == PowerUpType.Red)
        {
            redParticles.Play();
        }
        else if (powerUp.powerUpType == PowerUpType.Green)
        {
            greenParticles.Play();
        }
        else
        {
            blueParticles.Play();
        }

        // Make sure no duplicate powerups
        if (powerUpsActive.All(x => x.powerUpType != powerUp.powerUpType))
        {
            powerUpsActive.Add(powerUp);

            return true;
        }

        return false;
    }

    // TPs player
    public void MovePlayer(Vector3 location)
    {
        transform.position = location;
    }

    void FixedUpdate()
    {
        // Move the player left/right using input
        var movement = playerMove.ReadValue<Vector2>();
        var xVelocity = movement.x;
        var yVelocity = rb.velocity.y;

        // Check for downward input to increase falling speed
        // Check for fast fall input
        if (movement.y < -0.5f && !isGrounded())
        {
            if (!isFastFalling)
            {
                // Initiate fast fall
                isFastFalling = true;
                yVelocity = -fastFallSpeed; // Immediately set a downward velocity
            }
            else
            {
                // Continue fast falling
                yVelocity = Mathf.Max(yVelocity * fastFallMultiplier, -fastFallSpeed);
            }
        }
        else
        {
            isFastFalling = false;

            // Normal fall augmentation
            if (yVelocity < fallAugmentThreshold && yVelocity > -maxFallSpeed)
            {
                float fallAugment = (fallAugmentThreshold - yVelocity) * fallAugmentMultiplier;
                yVelocity -= fallAugment;
            }
        }

        // Clamp the fall speed to prevent excessive velocity
        yVelocity = Mathf.Max(yVelocity, -maxFallSpeed);
        
        rb.velocity = new Vector2(moveSpeed * xVelocity, yVelocity);


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

            // Animate player jumping
            anim.SetTrigger("Jump");
        }
    }

    // Called when fire bullet ability button is pressed
    public void OnFireBullet()
    {
        if (gControl.CurrentGameState() == gameController.gameState.running && IsPowerUpActive(PowerUpType.Red))
        {
            if (!GameObject.FindGameObjectWithTag("Bullet"))
            {
                sndManager.PlaySFX(sndManager.powerUpFireRed);
                Instantiate(bullet, transform.position + fireOffset, transform.rotation);
            }
        }
    }

    // Called when fire pulse ability button is pressed
    public void OnFirePulse()
    {
        // Spawn Blue pulses above and below the player
        if (gControl.CurrentGameState() == gameController.gameState.running && IsPowerUpActive(PowerUpType.Blue))
        {
            if (!GameObject.FindGameObjectWithTag("Pulse"))
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
    }

    public void OnRestart()
    {
        if (gControl.CurrentGameState() == gameController.gameState.pause) return;

        var restartCanvas = GameObject.FindGameObjectWithTag("Respawn");
        if (restartCanvas)
        {
            restartCanvas.GetComponent<fadeCanvas>().destroyCanvas();
        }

        GameObject activeFireball = GameObject.FindGameObjectWithTag("Bullet");
        GameObject activePulse = GameObject.FindGameObjectWithTag("Pulse");
        if (activeFireball)
        {
            Destroy(activeFireball);
        }
        else if (activePulse)
        {
            Destroy(activePulse);
        }

        scnManager.RestartLevel();
        sndManager.PlayBGM();
    }
}