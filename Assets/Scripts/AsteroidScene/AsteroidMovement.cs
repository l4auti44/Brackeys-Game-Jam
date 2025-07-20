using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public enum AsteroidSize { Big, Medium, Small }

    [Header("Asteroid Settings")]
    public AsteroidSize asteroidCategory; // Dropdown to set the category (Big, Medium, Small)
    public GameObject energyDropPrefab;   // The energy drop prefab
    private float spawnSeparationAngle = 120f; // Angle separation for spawned objects
    private float spawnForce = 5f; // Force applied to spawned asteroids
    private float collisionActivationDelay = 0.2f; // Delay before asteroids can collide with each other
    public float asteroidLifespan = 10f; // Lifespan of the asteroid in seconds
    

    [SerializeField] private int scoreAmountOnDestroy = 10;

    [Header("Asteroid Prefabs")]
    public List<GameObject> bigAsteroids;    // Pool of big asteroids
    public List<GameObject> mediumAsteroids; // Pool of medium asteroids
    public List<GameObject> smallAsteroids;  // Pool of small asteroids

    private Rigidbody2D rb;
    private bool canCollideWithAsteroids = false; // Track if the asteroid can collide with others
    private float screenBottomY; // Y position of the bottom of the screen
    private float energyDropSpeed = 2f; // Force applied to energy drop
    [Range(0f, 100f)]
    [SerializeField] private float energyDropRate = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Get the screen bottom Y position
        screenBottomY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;

        // Initialize delay for asteroid collision
        StartCoroutine(EnableAsteroidCollision());

        // Destroy the asteroid if it goes off the screen
        if (transform.position.y < screenBottomY - 1f) // Adding some margin
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Any code for asteroid movement or rotation can go here
        if (SceneController.isGameStopped)
        {
            rb.simulated = false;
        }
        else
        {
            rb.simulated = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the asteroid collided with something that should trigger its destruction
        if (other.CompareTag("Player"))
        {
            DestroyAsteroid();
        } else if (other.CompareTag("Missile")) {
            ScoreManager.AddScore(scoreAmountOnDestroy);
            DestroyAsteroid();
        }
        // Check if the asteroid collided with another asteroid after the delay
        else if (canCollideWithAsteroids && other.CompareTag("Enemy"))
        {
            DestroyAsteroid();
        }
    }

    private IEnumerator EnableAsteroidCollision()
    {
        yield return new WaitForSeconds(collisionActivationDelay);
        canCollideWithAsteroids = true; // Enable collision with other asteroids
    }

    public void DestroyAsteroid()
    {
        SpawnOnDestruction();
        Destroy(gameObject);
    }

    private void SpawnOnDestruction()
    {
        List<GameObject> selectedPool = null;
        int spawnCount = 0;

        // Determine what to spawn based on asteroid category
        if (asteroidCategory == AsteroidSize.Big)
        {
            selectedPool = mediumAsteroids; // Use the pool of medium asteroids
            spawnCount = 2;
            SpawnEnergyDrop();
        }
        else if (asteroidCategory == AsteroidSize.Medium)
        {
            selectedPool = smallAsteroids; // Use the pool of small asteroids
            spawnCount = 2;
            SpawnEnergyDrop();
        }
        else if (asteroidCategory == AsteroidSize.Small)
        {
            SpawnEnergyDrop(); // Only spawn energy drop for small asteroids
            return;
        }

        // Spawn asteroids separated by a certain angle
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject asteroidPrefab = selectedPool[Random.Range(0, selectedPool.Count)];
            GameObject spawnedAsteroid = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);

            // Set the direction and apply force for each spawned asteroid
            float angle = i * spawnSeparationAngle;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right; // Rotate right vector
            Rigidbody2D asteroidRb = spawnedAsteroid.GetComponent<Rigidbody2D>();
            asteroidRb.AddForce(direction.normalized * spawnForce, ForceMode2D.Impulse); // Apply force as impulse
        }
    }

    // Spawns an energy drop at the position of the destroyed asteroid
    private void SpawnEnergyDrop()
    {
        if (Random.value < energyDropRate / 100f)
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

    private IEnumerator DestroyAfterLifespan()
    {
        yield return new WaitForSeconds(asteroidLifespan);
        DestroyAsteroid(); // Automatically destroy asteroid after lifespan ends
    }
}

