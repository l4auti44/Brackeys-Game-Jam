using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab; // The asteroid prefab
    public Sprite[] asteroidSprites; // Array of asteroid sprites
    public float spawnRate = 1f; // Time between spawns
    public float minX, maxX; // Horizontal range for spawning asteroids
    public float spawnY = 6f; // Y position where asteroids spawn
    public float minSize = 0.5f; // Minimum asteroid scale
    public float maxSize = 1.5f; // Maximum asteroid scale

    public float minSpeed = 2f; // Minimum speed for asteroid movement
    public float maxSpeed = 5f; // Maximum speed for asteroid movement
    public float minRotationSpeed = 20f; // Minimum rotation speed
    public float maxRotationSpeed = 100f; // Maximum rotation speed
    public float minDirectionAngle = 20f; // Minimum direction angle
    public float maxDirectionAngle = 100f; // Maximum direction angle

    // Start is called before the first frame update
    void Start()
    {
        // Start spawning asteroids repeatedly
        InvokeRepeating("SpawnAsteroid", 0f, spawnRate);
    }

    // Function to spawn an asteroid
    void SpawnAsteroid()
    {
        // Randomize the spawn position within horizontal limits
        float spawnX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        // Instantiate the asteroid
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        // Randomly assign a sprite to the asteroid
        SpriteRenderer spriteRenderer = asteroid.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];
        }

        // Randomize asteroid size (scale)
        float randomScale = Random.Range(minSize, maxSize);
        asteroid.transform.localScale = new Vector3(randomScale, randomScale, 1f);

        // Adjust the collider size to match the new scale
        AdjustColliderSize(asteroid);

        // Apply random movement direction and speed
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Randomize direction (e.g., between -45 and +45 degrees from downwards)
            float randomAngle = Random.Range(minDirectionAngle, maxDirectionAngle);
            Vector2 movementDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.down;

            // Randomize speed between minSpeed and maxSpeed
            float moveSpeed = Random.Range(minSpeed, maxSpeed);
            rb.velocity = movementDirection * moveSpeed;
        }

        // Randomize rotation speed
        AsteroidMovement asteroidRotation = asteroid.GetComponent<AsteroidMovement>();
        if (asteroidRotation != null)
        {
            asteroidRotation.rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        }
    }

    // Function to adjust the collider size after scaling the asteroid
    void AdjustColliderSize(GameObject asteroid)
    {
        BoxCollider2D boxCollider = asteroid.GetComponent<BoxCollider2D>();
        CircleCollider2D circleCollider = asteroid.GetComponent<CircleCollider2D>();

        if (boxCollider != null)
        {
            // Adjust BoxCollider2D size based on the asteroid's scale and sprite size
            boxCollider.size = asteroid.GetComponent<SpriteRenderer>().bounds.size;
        }
        else if (circleCollider != null)
        {
            // Adjust CircleCollider2D radius based on the asteroid's scale
            float maxSize = Mathf.Max(asteroid.GetComponent<SpriteRenderer>().bounds.extents.x, asteroid.GetComponent<SpriteRenderer>().bounds.extents.y);
            circleCollider.radius = maxSize;
        }
    }
}
