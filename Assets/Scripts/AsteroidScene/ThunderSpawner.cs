using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderSpawner : MonoBehaviour
{
    public GameObject objectPrefab;            // The object to spawn
    public Transform parentObject;             // Parent object containing all spawn position children
    public float minSpawnInterval = 1f;        // Minimum interval between spawns (in seconds)
    public float maxSpawnInterval = 5f;        // Maximum interval between spawns (in seconds)

    private List<Transform> spawnPositions;    // List to store spawn positions
    private bool isSpawning = true;            // Control whether spawning continues

    void Start()
    {
        // Initialize the list of spawn positions and populate it from the parent object's children
        spawnPositions = new List<Transform>();

        foreach (Transform child in parentObject)
        {
            spawnPositions.Add(child);
        }

        // Check if positions were added
        if (spawnPositions.Count == 0)
        {
            Debug.LogWarning("No spawn positions found in the parent object.");
        }
        else
        {
            // Start the spawning coroutine
            StartCoroutine(SpawnAtIntervals());
        }
    }

    IEnumerator SpawnAtIntervals()
    {
        while (isSpawning)
        {
            SpawnRandomObject();

            // Randomize the interval for the next spawn
            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomInterval); // Wait for the randomized interval before spawning the next object
        }
    }

    public void SpawnRandomObject()
    {
        // Check if there are available positions in the list
        if (spawnPositions.Count == 0)
        {
            Debug.LogWarning("No spawn positions available.");
            return;
        }

        // Get a random spawn position from the list of predefined positions
        Transform randomPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];

        // Instantiate the object at the random position
        Instantiate(objectPrefab, randomPosition.position, Quaternion.identity);
    }

    public void StopSpawning()
    {
        isSpawning = false;  // You can call this method to stop spawning objects if needed
    }
}
