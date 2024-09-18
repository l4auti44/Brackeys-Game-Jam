using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipMovement : MonoBehaviour
{
    public float speed = 10f; // Speed of the ship
    public GameObject missilePrefab; // Missile prefab to shoot
    public Transform missileSpawnPoint; // Spawn point for the missile

    public float energyMissile; //Cost of energy per missile

    public float minX; // Minimum X boundary
    public float maxX;  // Maximum X boundary

    public float moveAmount = 5f; // Amount to move the ship vertically
    public float moveSpeed = 5f; // Speed at which to move the ship
    private Vector3 targetPosition; // The target position for vertical movement
    private bool isMoving = false; // Whether the ship is currently moving

    public float energy; // Amount of energy
    public GameObject energySlider;



    private void Start()
    {
        
        
    }


    void Update()
    {
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

        //Update the energy value of the slider
        energySlider.GetComponent<Slider>().value = energy * 0.01f;

    }

    // Function to shoot a missile
    public void ShootMissile()
    {
        if (energy > 0)
        {
            Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
 
            DecreaseEnergy (energyMissile);
        }

        else
        {
            //Negative feedback to the player that they are doing something without energy
        }
        
    }

    public void AccelerateShip()
    {
        // Calculate the new target position
        targetPosition = transform.position + new Vector3(0, moveAmount, 0);
        isMoving = true;
    }


    public void DecreaseEnergy (float energyToDecrease)
    {
            energy -= energyToDecrease;
    }




}
