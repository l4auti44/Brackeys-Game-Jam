using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySpawner : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;
    public GameObject[] energyPrefabs;   // Array of different energy drop prefabs
    public float moveSpeed = 2f;         // Speed at which the spawner moves between pos1 and pos2
    public float minSpawnInterval = 1f;  // Minimum time between spawns
    public float maxSpawnInterval = 5f;  // Maximum time between spawns
    public float minEnergySpeed = 0.5f;  // Minimum speed for energy drop
    public float maxEnergySpeed = 2.5f;  // Maximum speed for energy drop
    public float minRotationSpeed = 10f; // Minimum rotation speed for energy drop
    public float maxRotationSpeed = 100f;// Maximum rotation speed for energy drop

    private Vector3 pos1Value;
    private Vector3 pos2Value;
    private Vector3 targetPos;
    private float spawnTimer;

    void Start()
    {
        // Store the positions of pos1 and pos2
        pos1Value = pos1.position;
        pos2Value = pos2.position;
        targetPos = pos2Value;

        // Randomize the initial spawn timer
        spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        MoveBetweenPositions();      // Move the spawner between pos1 and pos2
        HandleEnergySpawning();      // Handle the spawning of energy drops
    }

    void MoveBetweenPositions()
    {
        // Move the spawner between pos1 and pos2 using MoveTowards
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // Switch the target position when the spawner reaches its current target
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            targetPos = targetPos == pos1Value ? pos2Value : pos1Value;
        }
    }

    void HandleEnergySpawning()
    {
        // Decrease the timer
        spawnTimer -= Time.deltaTime;

        // When the timer hits 0, spawn an energy drop
        if (spawnTimer <= 0f)
        {
            SpawnEnergy();
            spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval); // Randomize the next spawn timer
        }
    }

    void SpawnEnergy()
    {
        // Randomly choose an energy prefab from the array
        GameObject randomEnergy = energyPrefabs[Random.Range(0, energyPrefabs.Length)];
        GameObject energyDrop = Instantiate(randomEnergy, transform.position, Quaternion.identity);

        // Randomize the speed for the energy drop
        float randomSpeed = Random.Range(minEnergySpeed, maxEnergySpeed);
        Rigidbody2D rb = energyDrop.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.down * randomSpeed;
        }

        // Randomize the rotation speed for the energy drop
        float randomRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        rb.angularVelocity = randomRotationSpeed;
    }
}
