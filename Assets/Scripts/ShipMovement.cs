using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipMovement : MonoBehaviour
{
    public float speed; // Speed of the ship
    public GameObject missilePrefab; // Missile prefab to shoot
    public float energyMissile; //Cost of energy per missile
    public GameObject gameManager; //Game manager reference
    public Transform missileSpawnPoint; // Spawn point for the missile

    public float energyCollectable; //Energy replenished when collects an energy collectable
    private float energy;

    public float energyDecreaseEnemyHit; //Amount of energy lost when hitting an enemy

    public float minX; // Minimum X boundary
    public float maxX;  // Maximum X boundary

    public float moveAmount = 5f; // Amount to move the ship vertically
    public float moveSpeed = 5f; // Speed at which to move the ship
    private Vector3 targetPosition; // The target position for vertical movement
    private bool isMoving = false; // Whether the ship is currently moving



    private void Start()
    {
        
    }


    void Update()
    {
        //Get updated speed and energy from game manager
        speed = gameManager.GetComponent<GameManager>().shipSpeed;
        energy = gameManager.GetComponent<GameManager>().energy;
        
        // Get horizontal input (left/right arrows or A/D keys)
        float move = Input.GetAxis("Horizontal");

        // Move the ship horizontally
        transform.Translate(Vector2.right * move * speed * Time.deltaTime);

        // Clamp the ship's X position to stay within the bounds
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector2(clampedX, transform.position.y);

        if (isMoving)
        {
            // Move the ship towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Check if the ship has reached the target position
            if (transform.position == targetPosition)
            {
                isMoving = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnergyCollectable"))
        {
            gameManager.GetComponent<GameManager>().IncreaseEnergy(energyCollectable);

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameManager.GetComponent<GameManager>().DecreaseEnergy(energyDecreaseEnemyHit);

            Destroy(collision.gameObject);
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




}
