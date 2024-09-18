using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySpawner : MonoBehaviour
{
    public GameObject energyCollectablePrefab; // The prefab to spawn
    public float minInterval, maxInterval = 5f; // Time between each spawn
    public float minX = -8f; // Minimum X position for spawning
    public float maxX = 8f; // Maximum X position for spawning
    public float spawnY = 5f; // Y position where the collectable will spawn

    // Start is called before the first frame update
    void Start()
    {
        // Start spawning collectables at regular intervals
        InvokeRepeating("SpawnCollectable", 0f, Random.Range(minInterval, maxInterval));
    }

    // Function to spawn the energy collectable
    void SpawnCollectable()
    {
        // Generate a random X position within the specified range
        float spawnX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        // Instantiate the energy collectable at the random position
        Instantiate(energyCollectablePrefab, spawnPosition, Quaternion.identity);
    }
}
