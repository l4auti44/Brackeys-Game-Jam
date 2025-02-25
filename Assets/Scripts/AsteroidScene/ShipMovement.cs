using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipMovement : MonoBehaviour
{
    public float speed; // Speed of the ship
    public GameObject missilePrefab; // Missile prefab to shoot
    public float energyMissile; //Cost of energy per missile
    public GameManager gameManager; //Game manager reference
    public GameObject shipMovementSlider;
    public Transform missileSpawnPoint; // Spawn point for the missile

    public GameFeelManager gameFeelManager; //reference to the script for game feel elements
    public float timeStopWhenHitEnemy, shakeAmountWhenHitEnemy, shakeTimeWhenHitEnemy;
    public RectTransform cabinUI_gameObject, frame_asteriodsUI; //reference to the cabin UI object to trigger the shake effect

    public float energyCollectable; //Energy replenished when collects an energy collectable
    private float energy;

    public float energyDecreaseEnemyHit; //Amount of energy lost when hitting an enemy

    private float minX; // Minimum X boundary
    private float maxX;  // Maximum X boundary
    //private float moveXTowards;
    //private Vector3 moveTowards;
    //private float moveSpeedB = 50;

    public float moveSpeed = 5f; // Speed at which to move the ship horizontally

    public float moveDuration = 2f; // Time it takes to complete the movement
    private float moveTimer = 0f;
    private Vector3 initialPosition; // Starting position for the movement
    private bool isMovingToLevel = false;

    public bool isMoving = false; // Whether the ship is currently moving
    private Vector3 targetPosition; // The target position for vertical movement

    private SoundManager.Sound[] collectSoundsFX = { SoundManager.Sound.Collect1, SoundManager.Sound.Collect2 }; //List of possible sounds for a collected energy

    private void Start()
    {
        minX = gameManager.minX;
        maxX = gameManager.maxX;

    }


    void Update()
    {
        // Get updated speed and energy from game manager
        speed = gameManager.shipSpeed;
        energy = gameManager.energy;

        // Get horizontal input (left/right arrows or A/D keys)
        float move = Input.GetAxis("Horizontal");

        // Calculate new horizontal position but do not apply it directly
        float newX = Mathf.Clamp(transform.position.x + move * speed * Time.deltaTime, minX, maxX);

        // If moving to level, update only the vertical position
        float newY = transform.position.y;
        if (isMovingToLevel)
        {
            moveTimer += Time.deltaTime;

            // Calculate how far we are through the movement
            float t = moveTimer / moveDuration;

            // Apply an S-curve easing function to the time variable 't'
            t = EaseInFastOutSlow(t);

            // Interpolate only the Y-axis between initial and target positions
            newY = Mathf.Lerp(initialPosition.y, targetPosition.y, t);

            // Stop moving when we reach the target position
            if (t >= 1f)
            {
                isMovingToLevel = false;
            }
        }

        // Update the transform position with the new X and Y values
        transform.position = new Vector2(newX, newY);


        //Slider movement
        //moveXTowards = shipMovementSlider.GetComponent<Slider>().value * (maxX - minX);
        //moveTowards = new Vector3(moveXTowards, transform.position.y, transform.position.z);
        //transform.position = Vector3.MoveTowards(transform.position, moveTowards, moveSpeedB * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnergyCollectable"))
        {
            gameManager.IncreaseEnergy(energyCollectable);
            SoundManager.PlaySound(collectSoundsFX);

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Trigger crew animator with this event
            EventManager.Player.OnImpact.Invoke(this);

            gameManager.DecreaseMaxEnergy(energyDecreaseEnemyHit);

            SoundManager.PlaySound(SoundManager.Sound.HitByAsteroid);

            Destroy(collision.gameObject);

            //Trigger screen shake
            gameFeelManager.StartShake(shakeTimeWhenHitEnemy, shakeAmountWhenHitEnemy);
            gameFeelManager.ShakeObjectUI(cabinUI_gameObject, shakeAmountWhenHitEnemy, shakeTimeWhenHitEnemy);
            gameFeelManager.ShakeObjectUI(frame_asteriodsUI, shakeAmountWhenHitEnemy, shakeTimeWhenHitEnemy);



            //Trigger time stop
            gameFeelManager.StopFrame(timeStopWhenHitEnemy);
        }
    }


    // Function to shoot a missile
    public void ShootMissile()
    {

        if (energy > 0)
        {
            Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
        }

        else
        {
            //Negative feedback to the player that they are doing something without energy
        }
    }
        
    



    // Method to move the player to the specified Y position based on level with S-curve
    public void MoveToLevel(Transform targetPositionObject)
    {
        float targetY = targetPositionObject.position.y;
        targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

        initialPosition = transform.position; // Store the starting position
        moveTimer = 0f; // Reset the timer
        isMovingToLevel = true; // Start moving
    }

    // Custom easing function: fast initial growth, slower end
    float EaseInFastOutSlow(float t)
    {
        // Cubic ease in-out, with adjustments to make initial part faster and the final part slower
        return t < 0.5f
            ? 5 * t * t * t  // Faster growth in the first half
            : 1 - Mathf.Pow(-2 * t + 2, 3) / 2; // Slower growth in the second half
    }


}
