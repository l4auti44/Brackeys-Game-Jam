using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySpawner : MonoBehaviour
{
    public GameObject[] energyPrefabs;        // Energy drop prefabs
    public List<float> energyWeights;         // List of weights for each energy prefab
    public GameObject arrow1Prefab;           // Arrow prefab for slow speed
    public GameObject arrow2Prefab;           // Arrow prefab for medium speed
    public GameObject arrow3Prefab;           // Arrow prefab for fast speed

    public Transform leftParent;              // Parent object holding right-side spawn positions
    public Transform rightParent;             // Parent object holding left-side spawn positions
    private List<Transform> rightSpawnPositions = new List<Transform>();  // List of right-side spawn positions
    private List<Transform> leftSpawnPositions = new List<Transform>();   // List of left-side spawn positions

    public float minSpawnInterval = 1f;       // Minimum spawn interval
    public float maxSpawnInterval = 5f;       // Maximum spawn interval
    public float minEnergySpeed = 0.1f;       // Minimum speed for energy drops
    public float maxEnergySpeed = 5f;         // Maximum speed for energy drops
    public float arrowXOffset = 1f;           // X offset for the arrow

    private float spawnTimer;
    private bool spawnOnRight = true;         // Bool to toggle between right and left spawns
    public bool spawnArrow = false;           // Bool to toggle between on and off for spawning arrows

    void Start()
    {
        foreach (Transform child in leftParent)
        {
            rightSpawnPositions.Add(child);
        }

        foreach (Transform child in rightParent)
        {
            leftSpawnPositions.Add(child);
        }

        spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        if (!SceneController.isGameStopped)
        {
            HandleEnergySpawning();
        }
        
    }

    void HandleEnergySpawning()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnergy();
            spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    void SpawnEnergy()
    {
        GameObject selectedEnergyPrefab = GetRandomWeightedEnergyPrefab();

        List<Transform> spawnPositions = spawnOnRight ? rightSpawnPositions : leftSpawnPositions;
        spawnOnRight = !spawnOnRight;

        Transform spawnPoint = spawnPositions[Random.Range(0, spawnPositions.Count)];
        GameObject newEnergyDrop = Instantiate(selectedEnergyPrefab, spawnPoint.position, Quaternion.identity);

        float randomSpeed = Random.Range(minEnergySpeed, maxEnergySpeed);
        Rigidbody2D rb = newEnergyDrop.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = (spawnOnRight ? Vector2.left : Vector2.right) * randomSpeed;
        }

        if (spawnArrow)
        {
            SpawnArrow(randomSpeed, newEnergyDrop.transform.position, spawnOnRight);
        }
    }

    GameObject GetRandomWeightedEnergyPrefab()
    {
        float totalWeight = 0f;
        foreach (float weight in energyWeights)
        {
            totalWeight += weight;
        }

        float randomWeightPoint = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < energyPrefabs.Length; i++)
        {
            cumulativeWeight += energyWeights[i];
            if (randomWeightPoint < cumulativeWeight)
            {
                return energyPrefabs[i];
            }
        }

        return energyPrefabs[energyPrefabs.Length - 1];
    }

    void SpawnArrow(float speed, Vector3 energyDropPosition, bool isOnRight)
    {
        float speedRange = maxEnergySpeed - minEnergySpeed;
        GameObject selectedArrowPrefab;

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

        float adjustedXOffset = isOnRight ? -arrowXOffset : arrowXOffset;
        Vector3 arrowPosition = energyDropPosition + new Vector3(adjustedXOffset, 0, 0);

        float distance = Mathf.Abs(adjustedXOffset);
        float lifetime = distance / speed;

        GameObject arrow = Instantiate(selectedArrowPrefab, arrowPosition, Quaternion.identity);

        SpriteRenderer arrowSpriteRenderer = arrow.GetComponent<SpriteRenderer>();
        if (arrowSpriteRenderer != null && isOnRight)
        {
            arrowSpriteRenderer.flipX = true;
        }

        ArrowController arrowSelfDestruct = arrow.GetComponent<ArrowController>();
        if (arrowSelfDestruct != null)
        {
            arrowSelfDestruct.SetLifetime(lifetime);
        }
    }
}


