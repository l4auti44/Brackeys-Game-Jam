using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    private float screenBottomY; // Y position of the bottom of the screen

    public GameObject asteroid2Prefab;
    public GameObject asteroid3Prefab;
    public GameObject asteroid4Prefab;
    public GameObject energyDropPrefab;
    public int asteroidType;
    private float spawnForce = 2f; // Force applied to spawned asteroids
    public float energyDropSpeed = 2f; // Force applied to energy drop

    // Start is called before the first frame update
    void Start()
    {
        // Get the screen bottom Y position
        screenBottomY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;

    }

    // Update is called once per frame
    void Update()
    {
        // Destroy the asteroid if it goes off the screen
        if (transform.position.y < screenBottomY - 1f) // Adding some margin
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); // Destroy the asteroid upon collision 
        }
    }

    

    // This function is called when the asteroid is destroyed
    public void BreakAsteroid(int asteroidType)
    {
        switch (asteroidType)
        {
            case 1:
                // Break asteroid1
                SpawnAsteroids(asteroid2Prefab, 2, 180f); // Spawn 2 asteroid2
                SpawnEnergyDrop();
                break;
            case 2:
                // Break asteroid2
                SpawnAsteroids(asteroid3Prefab, 2, 120f); // Spawn 2 asteroid3
                SpawnAsteroids(asteroid4Prefab, 1, 120f); // Spawn 1 asteroid4
                SpawnEnergyDrop();
                break;
            default:
                break;
        }
        Destroy(gameObject); // Destroy the current asteroid
    }

    // Spawns a specified number of asteroids, with a given angle offset for separation
    private void SpawnAsteroids(GameObject asteroidPrefab, int count, float angleOffset)
    {
        float startAngle = Random.Range(0f, 360f); // Randomize initial angle

        for (int i = 0; i < count; i++)
        {
            GameObject newAsteroid = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = newAsteroid.GetComponent<Rigidbody2D>();

            float angle;

            // If there's more than one asteroid, calculate the angle
            if (count > 1)
            {
                float angleStep = angleOffset / (count - 1); // Calculate angle between asteroids
                angle = startAngle + (i * angleStep); // Separate them by angleStep
            }
            else
            {
                angle = startAngle; // If only one asteroid, no need for angleStep
            }

            Vector2 forceDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            if (rb != null)
            {
                rb.AddForce(forceDirection * spawnForce, ForceMode2D.Impulse); // Apply force in the calculated direction
            }
        }
    }

    // Spawns an energy drop at the position of the destroyed asteroid
    private void SpawnEnergyDrop()
    {
        GameObject energyDrop = Instantiate(energyDropPrefab, transform.position, Quaternion.identity);

        // Find the player's position
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // Calculate direction towards the player
            Vector2 directionToPlayer = (player.transform.position - energyDrop.transform.position).normalized;

            // Set the speed of the energy drop
            Rigidbody2D rb = energyDrop.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = directionToPlayer * energyDropSpeed;
            }
        }
    }



}

