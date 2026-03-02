using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThunderSpawner : MonoBehaviour
{
    public GameObject objectPrefab;            // The object to spawn
    public Transform parentObject;             // Parent object containing all spawn position children
    public float minSpawnInterval = 1f;        // Minimum interval between spawns (in seconds)
    public float maxSpawnInterval = 5f;        // Maximum interval between spawns (in seconds)
    public float RandomXPositionRange = 5f;        // Maximum interval between spawns (in seconds)

    private List<Transform> spawnPositions;    // List to store spawn positions


    private float timer;

    void Start()
    {
        timer = GetRandomSpawnInterval();
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
    }

    void Update()
    {
        if (SceneController.isGamePaused || SceneController.isGameStopped)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnRandomObject();
            timer = GetRandomSpawnInterval(); // Reset the timer with a new random interval
        }

    }


    public float GetRandomSpawnInterval()
    {
        return Random.Range(minSpawnInterval, maxSpawnInterval);
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
        float randomX = Random.Range(-RandomXPositionRange, RandomXPositionRange);
        Vector3 spawnPos = randomPosition.position + Vector3.right * randomX;
        // Instantiate the object at the random position
        Instantiate(objectPrefab, spawnPos, Quaternion.identity);
    }

}
