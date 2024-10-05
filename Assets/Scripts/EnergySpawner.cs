using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySpawner : MonoBehaviour
{
    public GameObject[] energyPrefabs;  // Energy drop prefabs
    public GameObject arrow1Prefab;     // Arrow prefab for slow speed
    public GameObject arrow2Prefab;     // Arrow prefab for medium speed
    public GameObject arrow3Prefab;     // Arrow prefab for fast speed

    public Transform leftParent;  // Parent object holding right-side spawn positions
    public Transform rightParent;   // Parent object holding left-side spawn positions
    private List<Transform> rightSpawnPositions = new List<Transform>();  // List of right-side spawn positions
    private List<Transform> leftSpawnPositions = new List<Transform>();   // List of left-side spawn positions

    public float minSpawnInterval = 1f;  // Minimum spawn interval
    public float maxSpawnInterval = 5f;  // Maximum spawn interval
    public float minEnergySpeed = 0.1f;  // Minimum speed for energy drops
    public float maxEnergySpeed = 5f;    // Maximum speed for energy drops
    public float arrowXOffset = 1f;      // X offset for the arrow

    private float spawnTimer;
    private bool spawnOnRight = true; // Bool to toggle between right and left spawns

    void Start()
    {
        // Populate rightSpawnPositions and leftSpawnPositions from child objects
        foreach (Transform child in leftParent)
        {
            rightSpawnPositions.Add(child);
        }

        foreach (Transform child in rightParent)
        {
            leftSpawnPositions.Add(child);
        }

        // Randomize the initial spawn timer
        spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        HandleEnergySpawning();
    }

    void HandleEnergySpawning()
    {
        // Decrease the spawn timer
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnergy();
            spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);  // Reset the spawn timer
        }
    }

    void SpawnEnergy()
    {
        // Randomly choose an energy prefab
        GameObject randomEnergy = energyPrefabs[Random.Range(0, energyPrefabs.Length)];

        // Toggle between right and left spawn positions
        List<Transform> spawnPositions = spawnOnRight ? rightSpawnPositions : leftSpawnPositions;
        spawnOnRight = !spawnOnRight; // Switch to the other side for the next spawn

        // Randomly choose a spawn position from the selected side
        Transform spawnPoint = spawnPositions[Random.Range(0, spawnPositions.Count)];

        // Instantiate the energy drop at the chosen position
        GameObject newEnergyDrop = Instantiate(randomEnergy, spawnPoint.position, Quaternion.identity);

        // Randomize the speed for the energy drop and flip direction if spawning on the right
        float randomSpeed = Random.Range(minEnergySpeed, maxEnergySpeed);
        Rigidbody2D rb = newEnergyDrop.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (spawnOnRight)
            {
                rb.velocity = Vector2.left * randomSpeed; // Move left if on the right side
            }
            else
            {
                rb.velocity = Vector2.right * randomSpeed; // Move right if on the left side
            }
        }

        // Determine which arrow to spawn based on the speed of the energy drop
        SpawnArrow(randomSpeed, newEnergyDrop.transform.position, spawnOnRight);
    }

    void SpawnArrow(float speed, Vector3 energyDropPosition, bool isOnRight)
    {
        float speedRange = maxEnergySpeed - minEnergySpeed;
        GameObject selectedArrowPrefab;

        // Determine which arrow to spawn based on speed thresholds
        if (speed > minEnergySpeed + (2f / 3f) * speedRange)
        {
            selectedArrowPrefab = arrow3Prefab;
        }
        else if (speed > minEnergySpeed + (1f / 3f) * speedRange)
        {
            selectedArrowPrefab = arrow2Prefab;
        }
        else
        {
            selectedArrowPrefab = arrow1Prefab;
        }

        // Adjust X offset for the direction of spawn (right or left)
        float adjustedXOffset = isOnRight ? -arrowXOffset : arrowXOffset;

        // Set the arrow's position with the adjusted X offset
        Vector3 arrowPosition = energyDropPosition + new Vector3(adjustedXOffset, 0, 0);

        // Calculate the lifetime based on speed and offset distance
        float distance = Mathf.Abs(adjustedXOffset);
        float lifetime = distance / speed;

        // Instantiate the arrow at the specified position
        GameObject arrow = Instantiate(selectedArrowPrefab, arrowPosition, Quaternion.identity);

        // Flip the arrow's sprite if it spawns on the right side
        SpriteRenderer arrowSpriteRenderer = arrow.GetComponent<SpriteRenderer>();
        if (arrowSpriteRenderer != null && isOnRight)
        {
            arrowSpriteRenderer.flipX = true;  // Flip the sprite on the X axis if spawning on the right
        }

        // Set the lifetime for the arrow
        ArrowController arrowSelfDestruct = arrow.GetComponent<ArrowController>();
        if (arrowSelfDestruct != null)
        {
            arrowSelfDestruct.SetLifetime(lifetime);
        }
    }
}


