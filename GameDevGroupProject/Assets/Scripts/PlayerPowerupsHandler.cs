using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPowerupsHandler : MonoBehaviour
{
    public List<PowerUp> powerUpsActive = new List<PowerUp>();

    private CircleCollider2D empCollider;
    private PlayerMovement playerMovement;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = this.GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
    }

    void OnSkill()
    {
        var bluePowerUp = powerUpsActive
            .FirstOrDefault(x => x.powerUpType == PowerUpType.Blue);

        if (bluePowerUp)
        {
            CreateEMPPulse();
        }

        var redPowerUp = powerUpsActive
            .FirstOrDefault(x => x.powerUpType == PowerUpType.Red);

        if (redPowerUp)
        {
            LaunchFireball();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PowerUp powerUpComponent = other.gameObject.GetComponent<PowerUp>();

        if (powerUpComponent != null)
        {
            if (!powerUpsActive.Contains(powerUpComponent))
            {
                AddPowerUp(powerUpComponent);

                Debug.Log("Player collided with a power-up!");
            }
        }
    }

    public void AddPowerUp(PowerUp powerUp)
    {
        foreach (var activePowerUp in powerUpsActive)
        {
            GameObject.Destroy(activePowerUp.gameObject);
        }
        powerUpsActive.Clear(); // make sure one powerup at a time

        powerUpsActive.Add(powerUp);
    }

    public GameObject empPrefab; // Assign the EMP prefab in the inspector
    public float empOffset = 1.0f; // Offset for the EMP position above and below the player

    void CreateEMPPulse()
    {
        Vector3 abovePosition = transform.position + Vector3.up * empOffset;
        Instantiate(empPrefab, abovePosition, Quaternion.identity);

        Vector3 belowPosition = transform.position + Vector3.down * empOffset;
        Instantiate(empPrefab, belowPosition, Quaternion.identity);
    }

    public GameObject fireballPrefab; // Assign the Fireball prefab in the inspector


    void LaunchFireball()
    {
        Vector3 fireballPosition =
            transform.position + ((Vector3)playerMovement.FacingDirection) * 1.0f; // Adjust position if needed

        var fireballInstance = Instantiate(fireballPrefab, fireballPosition, Quaternion.identity);

        Fireball fireballScript = fireballInstance.GetComponent<Fireball>();

        fireballScript.speed = 5 * playerMovement.moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Debug.Log("pwh Enemy hit by EMP!");
            // Destroy(other.gameObject);
        }
    }
}