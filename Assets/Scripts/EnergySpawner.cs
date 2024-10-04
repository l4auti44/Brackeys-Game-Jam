using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySpawner : MonoBehaviour
{
    public GameObject[] energyPrefabs;  // Energy drop prefabs
    public GameObject arrow1Prefab;     // Arrow prefab for slow speed
    public GameObject arrow2Prefab;     // Arrow prefab for medium speed
    public GameObject arrow3Prefab;     // Arrow prefab for fast speed
    public List<Transform> spawnPositions;  // List of spawn positions for the energy drops
    public float minSpawnInterval = 1f;  // Minimum spawn interval
    public float maxSpawnInterval = 5f;  // Maximum spawn interval
    public float minEnergySpeed = 0.1f;  // Minimum speed for energy drops
    public float maxEnergySpeed = 5f;    // Maximum speed for energy drops
    public float arrowXOffset = 1f;      // X offset for the arrow

    public float pos1Probability = 0.33f;  // Probability for spawn position 1
    public float pos2Probability = 0.33f;  // Probability for spawn position 2
    public float pos3Probability = 0.34f;  // Probability for spawn position 3

    private float spawnTimer;

    void Start()
    {
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

        // Randomly choose a spawn position based on the probabilities
        Transform spawnPoint = GetRandomSpawnPosition();

        // Instantiate the energy drop at the chosen position
        GameObject newEnergyDrop = Instantiate(randomEnergy, spawnPoint.position, Quaternion.identity);

        // Randomize the speed for the energy drop
        float randomSpeed = Random.Range(minEnergySpeed, maxEnergySpeed);
        Rigidbody2D rb = newEnergyDrop.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.right * randomSpeed;
        }

        // Determine which arrow to spawn based on the speed of the energy drop
        SpawnArrow(randomSpeed, newEnergyDrop.transform.position);
    }

    void SpawnArrow(float speed, Vector3 energyDropPosition)
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

        // Set the arrow's position with the X offset
        Vector3 arrowPosition = energyDropPosition + new Vector3(arrowXOffset, 0, 0);

        // Calculate the lifetime based on speed and offset distance
        float distance = Mathf.Abs(arrowXOffset);
        float lifetime = distance / speed;

        // Instantiate the arrow at the specified position
        GameObject arrow = Instantiate(selectedArrowPrefab, arrowPosition, Quaternion.identity);

        // Set the lifetime for the arrow
        ArrowController arrowSelfDestruct = arrow.GetComponent<ArrowController>();
        if (arrowSelfDestruct != null)
        {
            arrowSelfDestruct.SetLifetime(lifetime);
        }
    }

    Transform GetRandomSpawnPosition()
    {
        float randomValue = Random.value;
        if (randomValue < pos1Probability)
        {
            return spawnPositions[0];
        }
        else if (randomValue < pos1Probability + pos2Probability)
        {
            return spawnPositions[1];
        }
        else
        {
            return spawnPositions[2];
        }
    }
}
